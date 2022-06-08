import { createContext, useContext } from 'react';
import { ShoppingCart } from './ShoppingCart';

export const StoreContext = createContext<ShoppingCart | undefined>(undefined);

export function useStoreContext() {
  const context = useContext(StoreContext);
  if (context === undefined) {
    throw Error('context must be set');
  }
  return context;
}

