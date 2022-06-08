import axios, { AxiosResponse, AxiosInstance } from 'axios';
import { toast } from 'react-toastify';
import authService from '../components/api-authorization/AuthorizeService';
import { history } from '../index';
import { Country } from '../models/country';
import { OrderInfo, ShippingAddress } from '../models/order';

export type AccessToken = {
  access_token: string;
  expires_at: string;
  is_newly_created: boolean;
};

const cartApi: AxiosInstance = axios.create({
  baseURL: `${window.location.origin}/api`
});

cartApi.interceptors.request.use(
  async config => {
    const token = await authService.getAccessToken();
    // const token = window.localStorage.getItem('jwt');
    if (token) config.headers.Authorization = `Bearer ${token}`;
    return config;
  },
  error => {
    return Promise.reject(error);
  }
);

cartApi.interceptors.response.use(undefined, error => {
  if (error.message === 'Network Error' && !error.response) {
    toast.error('Network error - make sure API is running!');
  }
  const { status, data, config, headers } = error.response;
  // if (status === 404) {
  //     history.push('/notfound1');
  // }
  if (status === 401 && headers['www-authenticate'] === 'Bearer error="invalid_token", error_description="The token is expired"') {
    window.localStorage.removeItem('jwt');
    history.push('/');
    // toast.info('Your session has expired, please login again')
  }
  if (status === 500) {
    toast.error('Server error - check the terminal for more info!');
  }
  throw error?.response?.data;
});

const responseBody = (response: AxiosResponse) => response.data;

const requests = {
  get: (url: string, params?: URLSearchParams) =>
    cartApi
      .get(url, { params })
      .then(responseBody),
  post: <T>(url: string, body: T) =>
    cartApi
      .post(url, body)
      .then(responseBody),
  put: (url: string, body: any) =>
    cartApi
      .put(url, body)
      .then(responseBody),
  del: (url: string) =>
    cartApi
      .delete(url)
      .then(responseBody),
  postForm: (url: string, file: Blob) => {
    const formData = new FormData();
    formData.append('File', file);
    return cartApi
      .post(url, formData, {
        headers: { 'Content-type': 'multipart/form-data' }
      })
      .then(responseBody);
  }
};

// const selectedCountry = (): number => {
//   const countryId = localStorage.getItem('country');
//   return countryId ? parseInt(countryId) : 1;
// };

// const countryFilter = () => {
//   return 'country=' + selectedCountry();
// };

const Basket = {
  get: () => requests.get('basket'),
  addItem: (productId: number, quantity: number = 1) => requests.post(`basket?productId=${productId}&quantity=${quantity}`, {}),
  updateItem: (productId: number, totalQuantity: number) => requests.put(`basket?productId=${productId}&totalQuantity=${totalQuantity}`, {}),
  removeItem: (productId: number, quantity: number = 1) => requests.del(`basket?productId=${productId}&quantity=${quantity}`),
  getShippingCost: (): Promise<number> => requests.get('basket/shippingCost')

};

const Order = {
  create: (orderInfo: OrderInfo) => requests.post('orders', orderInfo),
};


const Catalog = {
  list: (params: URLSearchParams) => requests.get('products', params),
  // below are not implemented yet due to time constraint :-(
  // details: (id: number) => requests.get(`products/${id}`),
  // fetchFilters: () => requests.get('products/filters')
};

const Countries = {
  get: async (): Promise<Country[]> => {
    return requests.get('countries');
  }
};

export default {
  Catalog,
  Basket,
  Countries,
  Order
};
