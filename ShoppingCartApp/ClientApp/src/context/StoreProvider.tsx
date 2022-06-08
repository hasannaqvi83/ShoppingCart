import React, { useState } from 'react';
import { useEffect } from 'react';
import { useHistory } from 'react-router-dom';
import authService from '../components/api-authorization/AuthorizeService';
import { ShoppingCart } from './ShoppingCart';
import { StoreContext } from './StoreContext';
import { History } from 'history';

export interface IStoreProviderProps { }

export const StoreProvider: React.FunctionComponent<IStoreProviderProps> = (props: React.PropsWithChildren<IStoreProviderProps>) => {
  const history: History = useHistory<any>();
  const [cart] = useState<ShoppingCart | null>(new ShoppingCart(history));
  const populateUserState = async () => {
    const isAuthenticated = await authService.isAuthenticated();
    if (isAuthenticated) {
      cart.ensureCartLoaded();
    }
  };

  useEffect(() => {
    const subscription = authService.subscribe(() => populateUserState());
    populateUserState();
    return () => {
      authService.unsubscribe(subscription);
    };
  }, []);

  return (
    <StoreContext.Provider value={cart}>
      {props.children}
    </StoreContext.Provider>
  );
};