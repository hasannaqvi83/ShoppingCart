import React, { Component, Fragment } from 'react';
import { NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import authService from './AuthorizeService';
import { ApplicationPaths } from './ApiAuthorizationConstants';
import { ShoppingCart } from '../../context/ShoppingCart';
import { StoreContext } from '../../context/StoreContext';
import { faShoppingCart } from '@fortawesome/free-solid-svg-icons';
import { observer } from 'mobx-react';
import './LoginMenu.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

@observer
export class LoginMenu extends Component<any, any> {
  public context: ShoppingCart;
  static contextType?: React.Context<ShoppingCart> = StoreContext;
  _subscription: any;
  constructor(props) {
    super(props);

    this.state = {
      isAuthenticated: false,
      userName: null
    };
  }

  componentDidMount() {
    this._subscription = authService.subscribe(() => this.populateState());
    this.populateState();
  }

  componentWillUnmount() {
    authService.unsubscribe(this._subscription);
  }

  async populateState() {
    const [isAuthenticated, user] = await Promise.all([authService.isAuthenticated(), authService.getUser()]);
    this.setState({
      isAuthenticated,
      userName: user && user.name
    });
  }

  render() {
    const cartItemsCount = this.context?.totalItems;
    const { isAuthenticated, userName } = this.state;
    if (!isAuthenticated) {
      const registerPath = `${ApplicationPaths.Register}`;
      const loginPath = `${ApplicationPaths.Login}`;
      return this.anonymousView(registerPath, loginPath);
    } else {
      const profilePath = `${ApplicationPaths.Profile}`;
      const logoutPath = { pathname: `${ApplicationPaths.LogOut}`, state: { local: true } };
      return this.authenticatedView(userName, profilePath, logoutPath, cartItemsCount);
    }
  }

  authenticatedView(userName, profilePath, logoutPath, cartItemsCount) {
    return (<Fragment>
      <NavItem>
        <NavLink tag={Link} className="text-dark" to={profilePath}>Hello {userName}</NavLink>
      </NavItem>
      <NavItem>
        <NavLink tag={Link} className="text-dark" to="/catalog">Catalog</NavLink>
      </NavItem>

      <NavItem>
        <NavLink tag={Link} className="text-dark" to="/basket">
        <FontAwesomeIcon className="mr-2" icon={faShoppingCart} />
          <h5 className="d-inline cart_badge">
            <span className="badge badge-success">{cartItemsCount}</span>
          </h5>
        </NavLink>
      </NavItem>
      <NavItem>
        <NavLink tag={Link} className="text-dark" to={logoutPath}>Logout</NavLink>
      </NavItem>
    </Fragment>);

  }

  anonymousView(registerPath, loginPath) {
    return (<Fragment>
      <NavItem>
        <NavLink tag={Link} className="text-dark" to={registerPath}>Register</NavLink>
      </NavItem>
      <NavItem>
        <NavLink tag={Link} className="text-dark" to={loginPath}>Login</NavLink>
      </NavItem>
    </Fragment>);
  }
}
