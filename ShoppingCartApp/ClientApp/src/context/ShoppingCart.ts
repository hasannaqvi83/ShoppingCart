import { action, computed, observable, reaction } from 'mobx';
import { toast } from 'react-toastify';
import agent from '../api/agent';
import { Basket } from '../models/basket';
import { Country } from '../models/country';
import { OrderInfo } from '../models/order';
import { History } from 'history';

const EMPTY_BASKET: Basket = { id: -1, buyerId: '', items: [] };
export class ShoppingCart {
  constructor(private history: History) {
    reaction(() => this.basket.items, (/* we dont need value */) => {
      agent.Basket.getShippingCost().then(shippingCost => {
        this.setShippingCost(shippingCost);
      });
    });
  }

  @observable public basket: Basket = EMPTY_BASKET;
  @observable public loadingCart: boolean = false;
  @observable public basketLoaded: boolean = false;
  @observable public countries: Country[] = [];
  @observable public selectedCountryId: number = 1;
  @observable public shippingCost: number = 0;
  @computed public get totalItems(): number {
    return this.basket.items.reduce((acc, item) => acc + item.quantity, 0);
  }
  @computed public get totalPrice(): number {
    return this.basket.items.reduce((acc, item) => acc + item.quantity * item.price, 0);
  }

  @computed public get isCartReady(): boolean {
    return this.basketLoaded && this.countriesLoaded;
  }

  @computed public get countriesLoaded(): boolean {
    return this.countries && this.countries.length > 0;
  }

  @computed public get selectedCountry(): Country {
    if (this.countries.length > 0) {
      const country = this.countries.find(c => c.id === this.selectedCountryId);
      return country;
    }
    return null;
  }

  @computed public get currencyRate(): number {
    if (this.selectedCountry) {
      return this.selectedCountry.currencyRate;
    }
    return 1;
  }

  @action
  public setShippingCost = (shippingCost: number): void => {
    this.shippingCost = shippingCost;
  };

  @action
  public setBasket = (basket: Basket): void => {
    if (basket) {
      this.basket = basket;
    }
  };

  private handleException(methodName: string, ex: any) {
    console.log(`${methodName}: Caught exception `, ex);
    if (ex && ex.errorMessage)
      toast.error(ex.errorMessage);
    else {
      toast.error('An unexpected error occurred.');
    }
  }

  @action
  public loadBasket = async (): Promise<void> => {
    try {
      this.loadingCart = true;
      const basket = await agent.Basket.get() as Basket;
      if (basket)
        this.setBasket(basket);
      this.basketLoaded = true;
      console.log('Cart loaded ', basket);
    } catch (ex) {
      this.setBasket(EMPTY_BASKET);
      if (ex.status === 404) { // not found 
        this.basketLoaded = true;
      } else {
        this.handleException('loadCart', ex);
      }
    } finally {
      this.loadingCart = false;
    }
  };

  @action
  public setCountries = (countries: Country[]): void => {
    this.countries = countries;
  };

  @action
  public setSelectedCountryId = (countryId: number): void => {
    localStorage.setItem('country', countryId.toString());
    this.selectedCountryId = countryId;
  };

  public loadCountries = async (): Promise<void> => {
    try {
      const countries = await agent.Countries.get();
      if (countries) {
        this.setCountries(countries);
      }
    } catch (ex) {
      this.handleException('loadCountries', ex);
    } finally {

    }
  };

  @action
  public loadCountrySelection() {
    const savedCountryId = localStorage.getItem('country');
    if (savedCountryId) {
      this.selectedCountryId = parseInt(savedCountryId);
    }
  }

  public async ensureCartLoaded() {
    if (!this.isCartReady) {
      await this.loadBasket();
      await this.loadCountries();
      this.loadCountrySelection();
    }
  }

  public addItem = async (productId: number, quantity: number = 1): Promise<number> => {
    let newQuantity = 0;
    try {
      const basket = await agent.Basket.addItem(productId) as Basket;
      console.log('addItem ', basket);
      this.setBasket(basket);
      toast.info('Item added to cart successfully.');
      if (basket) {
        newQuantity = this.getQuantity(productId);
      }
    } catch (ex) {
      this.handleException('addItem', ex);
      newQuantity = this.getQuantity(productId);
    }
    return newQuantity;
  };

  public updateItem = async (productId: number, totalQuantity: number): Promise<number> => {
    try {
      console.log('updating Item ', productId, totalQuantity);
      const basket = await agent.Basket.updateItem(productId, totalQuantity) as Basket;
      if (basket) {
        console.log('updateItem ', basket);
        this.setBasket(basket);
        toast.info('Item updated in cart successfully.');
        return this.getQuantity(productId);
      }
    } catch (ex) {
      this.handleException('updateItem', ex);
    }
  };

  private getQuantity = (productId: number): number => {
    const item = this.basket.items.find(i => i.productId === productId);
    return item.quantity;
  };

  public removeItem = async (productId: number): Promise<boolean> => {
    let success = false;
    try {
      const resp = await agent.Basket.removeItem(productId) as Basket;
      console.log('removeItem ', resp);
      let basket = this.basket;
      if (!basket) return;
      const items = [...basket.items];
      const itemIndex = items.findIndex(i => i.productId === productId);
      if (itemIndex >= 0) {
        items.splice(itemIndex, 1);
        basket = { ...this.basket, items };
        this.setBasket(basket);
      }
      success = true;
      toast.info('Item removed from cart successfully.');
    } catch (ex) {
      this.handleException('removeItem', ex);
    }
    return success;
  };

  public createOrder = async (orderInfo: OrderInfo): Promise<void> => {
    try {
      const history: History = this.history;
      const result = await agent.Order.create(orderInfo);
      console.log('createOrder ', result);
      history.push('/thank', { orderId: result });
      await this.loadBasket();
      toast.info('Order created successfully.');
    } catch (ex) {
      this.handleException('createOrder', ex);
    }
  };
}