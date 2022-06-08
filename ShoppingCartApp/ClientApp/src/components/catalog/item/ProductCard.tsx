import React, { useState } from 'react';
import { LazyLoadImage } from 'react-lazy-load-image-component';
import 'react-lazy-load-image-component/src/effects/blur.css';
import { Product } from '../../../models/product';
import './ProductCard.css';
import { currencyFormat } from '../../../common/util';
import { Button } from 'reactstrap';
import agent from '../../../api/agent';
import { useStoreContext } from '../../../context/StoreContext';
import { observer } from 'mobx-react';
import { calculatePrice } from '../../../models/country';
import { CountryDropDown } from '../../core/CountryDropDown';

export interface IProductCardProps {
  product: Product;
  colSize: number;
  link: string;
  btnName: string;
}

export const ProductCard: React.FunctionComponent<IProductCardProps> = observer(({ product, colSize, link, btnName }: React.PropsWithChildren<IProductCardProps>) => {
  const ctx = useStoreContext();
  const [moreInfo, setMoreInfo] = useState(false);
  if (!ctx.countriesLoaded) return null;
  const { selectedCountry } = ctx;
  return (
    <div className={`col-lg-${colSize}`} title={product.description} onMouseEnter={() => {
      setMoreInfo(true);
    }} onMouseLeave={() => {
      setMoreInfo(false);
    }}>
      <div className="card mb-5 product_card_item_wrapper">
        <div className="product_card_item_image_wrapper">
          <LazyLoadImage className="product_card_item_image img-responsive" effect="blur" src={product.pictureUrl} />
        </div>
        <div className="card-body text-center">
          {/* <Rating control comes here /> */}
          <h6>{product.name}</h6>
          <h6 className="product_price">
            <span>{currencyFormat(selectedCountry.currencySymbol, calculatePrice(product.price, selectedCountry.currencyRate))}</span>
          </h6>
        </div>

        <div className="text-center align-items-end mb-3">
          <Button className={'btn btn-primary'} onClick={() => ctx.addItem(product.id)} >
            Add to cart
          </Button>
        </div>
      </div>
    </div >
  );
});