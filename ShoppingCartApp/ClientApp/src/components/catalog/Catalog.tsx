import React, { FC, ReactElement, useEffect, useState } from 'react';
import { Route, useLocation } from 'react-router-dom';
import Products from './list/Products';
import { Product, ProductParams } from '../../models/product';
import agent from '../../api/agent';

function getAxiosParams(productParams: ProductParams) {
  const params = new URLSearchParams();
  params.append('pageNumber', productParams.pageNumber.toString());
  params.append('pageSize', productParams.pageSize.toString());
  params.append('orderBy', productParams.orderBy);
  if (productParams.searchTerm) params.append('searchTerm', productParams.searchTerm);
  return params;
}

export const Catalog: FC = (): ReactElement => {
  const [products, setProducts] = useState<Product[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [searchParams, setSearchParams] = useState<ProductParams>({
    pageNumber: 1,
    pageSize: 10,
    orderBy: 'name',
    searchTerm: ''
  });

  useEffect(() => {
    let isMounted = true;
    window.scrollTo(0, 0);
    setIsLoading(true);
    const params = getAxiosParams(searchParams);
    try {
      agent.Catalog.list(params).then(response => {
        if (isMounted)
          setProducts(response);
      });
    } catch (error: any) {
      console.error(error.data);
    } finally {
      if (isMounted)
        setIsLoading(false);
    }
    return () => {
      isMounted = false;
    };
  }, []);


  return (
    <div className="container d-flex">
      <nav id="sidebar">
        <div className="sidebar-header">
          <h3>Products</h3>
        </div>
      </nav>
      <Route
        exact
        component={() => (
          <Products
            data={products}
            loading={isLoading}
            itemsPerPage={16}
          />
        )}
      />
    </div>
  );
};

