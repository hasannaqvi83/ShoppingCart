import React, { ChangeEvent, FC, ReactElement, useRef, useState } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faChevronDown, faChevronUp, faMinusSquare, faSave } from '@fortawesome/free-solid-svg-icons';
import { BasketItem } from '../../models/basket';
import { LazyLoadImage } from 'react-lazy-load-image-component';
import './Cart.css';
import { currencyFormat } from '../../common/util';
import { useStoreContext } from '../../context/StoreContext';
import { observer } from 'mobx-react';
import { calculatePrice } from '../../models/country';

export interface ICartItemProps {
  item: BasketItem;
}

export const CartItem: React.FunctionComponent<ICartItemProps> = observer(({ item }: React.PropsWithChildren<ICartItemProps>) => {
  const { addItem, updateItem, removeItem, selectedCountry } = useStoreContext();
  const refTxtQuantity = useRef<HTMLInputElement>();
  const [quantity, setQuantity] = useState(item.quantity);
  return (
    <div key={item.id} className="card mb-3 mx-auto cart_item_wrapper">
      <div className="row no-gutters">
        <div className="col-2 mx-3 my-3">
          <LazyLoadImage className=" img-fluid" effect="blur" src={item.pictureUrl} />

        </div>
        <div className="col-6">
          <div className="card-body">
            <h4 className="card-title">{item.name}</h4>
            <p className="card-text "><span className="cart_item_price">{currencyFormat(selectedCountry.currencySymbol, calculatePrice(item.price, selectedCountry.currencyRate))} </span><span>x {item.quantity}</span></p>
          </div>
        </div>
        <div className="col-1 mt-3">
          <button
            className="btn btn-default"
            title={item.quantity == 10 ? 'You are not allowed to add more than 10 items' : 'Add an extra item'}
            // disabled={item.quantity == 10} // we are not allowing more than 10 items of a product in the cart
            onClick={async () => {
              const addItemCount = await addItem(item.productId);
              if (addItemCount) {
                setQuantity(addItemCount);
              }
            }}
          >
            <FontAwesomeIcon size="lg" icon={faChevronUp} />
          </button>
          <div className="cart_update_section">
            <input
              type="text"
              className="form-control input-number cart_input_count"
              value={quantity}
              onKeyDown={(e) => {
                if (e.key === 'Enter') {
                  updateItem(item.productId, quantity);
                }
              }} onChange={(e) => {
                const newQuantity = e.target.value ? parseInt(e.target.value) : 0;
                setQuantity(newQuantity);
              }}
            />
            <FontAwesomeIcon size="lg" icon={faSave} className="cart_update_button" onClick={(event) => {
              updateItem(item.productId, quantity);
            }} />
          </div>
          <button
            className="btn btn-default"
            title={item.quantity == 1 ? 'Use the remove button to remove this item from the cart.' : 'Remove an item'}
            disabled={item.quantity === 1}
            onClick={async () => {
              const newQuantity = await updateItem(item.productId, quantity - 1);
              if (newQuantity)
                setQuantity(newQuantity);
            }}
          >
            <FontAwesomeIcon size="lg" icon={faChevronDown} />
          </button>
        </div>
        <div className="col-2">
          <div className="card-body">
            <h5 className="card-title">
              <span>{currencyFormat(selectedCountry.currencySymbol, calculatePrice(item.price, selectedCountry.currencyRate) * item.quantity)}</span>
            </h5>
            <button className="btn btn-warning mb-2" onClick={() => removeItem(item.productId)}>
              <FontAwesomeIcon className="mr-2" icon={faMinusSquare} /> Remove
            </button>
          </div>
        </div>
      </div>
    </div >
  );
});