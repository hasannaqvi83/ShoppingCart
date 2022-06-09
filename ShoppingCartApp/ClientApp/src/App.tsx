import React, { Component, Fragment } from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import AuthorizeRoute from './components/api-authorization/AuthorizeRoute';
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants';

import './custom.css';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { Catalog } from './components/catalog/Catalog';
import { Cart } from './components/cart/Cart';
import { ThankYou } from './components/core/Thankyou';

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Fragment>
        <ToastContainer position='bottom-right' />
        <Layout>
          <Route exact path='/' component={Home} />
          <AuthorizeRoute exact path='/catalog' component={Catalog} />
          <AuthorizeRoute exact path='/basket' component={Cart} />
          <AuthorizeRoute exact path='/thank' component={ThankYou} />
          <AuthorizeRoute path='/fetch-data' component={FetchData} />
          <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
        </Layout>
      </Fragment>

    );
  }
}
