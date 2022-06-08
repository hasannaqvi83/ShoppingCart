import React from 'react';
import { CardGroup, Spinner } from 'reactstrap';
import { PRODUCT } from '../../../common/constants';
import { Product } from '../../../models/product';
import { CountryDropDown } from '../../core/CountryDropDown';
import { ProductCard } from '../item/ProductCard';

export interface IProductsProps {
  data: Array<Product>;
  loading: boolean;
  itemsPerPage: number;
  startFrom?: number;
}

export const Products: React.FunctionComponent<IProductsProps> = ({
  data,
  loading,
  itemsPerPage,
  startFrom
}: React.PropsWithChildren<IProductsProps>) => {
  return (
    <div className="container">
      <div className="container-fluid mt-5 ml-2">
        {/* Search comes here */}
      </div>
      <div className="container-fluid mt-3 ml-2">
        <div className="row">
          <div className="col-md-6">
            {/* pagination to be done here */}
          </div>
          <div className="col-md-6 d-flex justify-content-end">
            <ul className="pagination">
            <CountryDropDown />
              {/* <li className="page-item">
                <a className="page-link" href="#" onClick={(e) => { e.preventDefault(); alert('Not implemented :('); }}>
                  Sort by price
                </a>
              </li> */}
              {/* sort button to be done here */}
            </ul>
          </div>
        </div>
        {loading ? (
          <Spinner />
        ) : (
          <>  
            <div className="row">
              <CardGroup>
                {data.map((product: Product) =>
                  <ProductCard
                    key={product.id}
                    product={product}
                    colSize={3}
                    link={PRODUCT}
                    btnName={'SHOW MORE'}
                  />)}
              </CardGroup>
            </div>
          </>
        )}
      </div>
    </div>
  );
};

export default Products;
