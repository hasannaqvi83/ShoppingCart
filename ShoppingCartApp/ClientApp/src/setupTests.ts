const localStorageMock = {
  getItem: jest.fn(),
  setItem: jest.fn(),
  removeItem: jest.fn(),
  clear: jest.fn(),
};
(global as any).localStorage = localStorageMock;

// Mock the request issued by the react app to get the client configuration parameters.
(window as any).fetch = () => {
  return Promise.resolve(
    {
      ok: true,
      json: () => Promise.resolve({
        'authority': 'https://localhost:5001',
        'client_id': 'ShoppingCart',
        'redirect_uri': 'https://localhost:5001/authentication/login-callback',
        'post_logout_redirect_uri': 'https://localhost:5001/authentication/logout-callback',
        'response_type': 'id_token token',
        'scope': 'ShoppingCartAPI openid profile'
      })
    });
};

import { configure } from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';
configure({ adapter: new Adapter() });