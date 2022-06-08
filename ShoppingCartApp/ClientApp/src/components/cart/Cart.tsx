import React, { ChangeEvent, FC, ReactElement, useEffect, useState } from 'react';
import { faShoppingCart } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Dropdown, DropdownItem, DropdownMenu, DropdownToggle, Spinner } from 'reactstrap';
import { useStoreContext } from '../../context/StoreContext';
import { BasketItem } from '../../models/basket';
import { observer } from 'mobx-react';
import './Cart.css';
import { toJS } from 'mobx';
import { CartItem } from './CartItem';
import CartCheckout from './CartCheckout';
import { CountryDropDown } from '../core/CountryDropDown';

export interface ICartProps { }
export const Cart: React.FunctionComponent<ICartProps> = observer((props: React.PropsWithChildren<ICartProps>) => {
  const ctx = useStoreContext();

  let items: BasketItem[] = null;
  if (ctx && ctx.basketLoaded) {
    items = ctx.basket?.items;
    console.log('basket items length ', items.length);
  }
  const { totalPrice } = ctx;
  return (
    <div className="container mt-5 pb-5 cart_wrapper">
      {!ctx.isCartReady ? (
        <Spinner />
      ) : (ctx.basketLoaded &&
        <div>
          {!items || items.length === 0 ? (
            <h2 className="text-center">Cart is empty</h2>
          ) : (
            <div>
              <p className="h4 mb-4 text-center">
                <FontAwesomeIcon className="mr-2" icon={faShoppingCart} /> Cart
              </p>
              <CountryDropDown />
              {items.map((item: BasketItem) => <CartItem key={item.id} item={item} />)}
              <hr className="my-3" />
              <CartCheckout totalPrice={totalPrice} />
            </div>
          )}
        </div>
      )}
    </div>
  );
});