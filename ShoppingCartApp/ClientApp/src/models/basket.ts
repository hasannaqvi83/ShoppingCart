export interface BasketItem {
  id: number;
  productId: number;
  name: string;
  price: number;
  displayedPrice: number;
  pictureUrl: string;
  brand: string;
  type: string;
  quantity: number;
}

export interface Basket {
  id: number;
  buyerId: string;
  items: BasketItem[];
}