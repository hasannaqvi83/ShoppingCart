export interface Product {
  id: number;
  productCode: string;
  name: string;
  description: string;
  price: number;
  pictureUrl: string;
}

export interface ProductParams {
  orderBy: string;
  searchTerm?: string;
  pageNumber: number;
  pageSize: number;
}