import React, { FC, ReactElement } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faShoppingBag } from '@fortawesome/free-solid-svg-icons';
import { useStoreContext } from '../../context/StoreContext';
import { currencyFormat } from '../../common/util';
import { observer } from 'mobx-react';
import { calculatePrice } from '../../models/country';
import authService from '../api-authorization/AuthorizeService';

export interface ICartCheckoutProps {
  totalPrice: number;
}

export const CartCheckout: React.FunctionComponent<ICartCheckoutProps> = observer(({ totalPrice }: React.PropsWithChildren<ICartCheckoutProps>) => {
  const { selectedCountry, shippingCost, currencyRate, createOrder } = useStoreContext();
  const { currencySymbol } = selectedCountry;
  // console.log('shippingCost ', shippingCost);
  return (
    <div className="row">
      <div className="col-9">
        <p className="h5 text-right">
          Total: <span>{currencyFormat(currencySymbol, calculatePrice(totalPrice, currencyRate))} </span>
        </p>
        <p className="h5 text-right">
          Shipping Cost: <span>{currencyFormat(currencySymbol, calculatePrice(shippingCost, currencyRate))} </span>
        </p>
      </div>
      <div className="col-3">
        <div className="form-row">
          <button className="btn btn-success" onClick={async () => {
            const userInfo = await authService.getUserInfo();
            console.log('userInfo ', userInfo);
            createOrder({
              shippingAddress: {
                address: 'Dummy address',
                fullName: userInfo.name
              }
            });
          }}>
            <FontAwesomeIcon className="mr-2" icon={faShoppingBag} /> Place Order
          </button>
        </div>
      </div>
    </div>
  );
});

export default CartCheckout;
