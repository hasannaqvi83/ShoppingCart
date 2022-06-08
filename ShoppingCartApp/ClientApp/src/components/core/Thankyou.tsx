import * as React from 'react';
import { Link, useLocation } from 'react-router-dom';

export interface IThankYouProps { }

export const ThankYou: React.FunctionComponent<IThankYouProps> = (props: React.PropsWithChildren<IThankYouProps>) => {
  const { state } = useLocation<{ orderId: string }>();
  return (
    <>
      <div className="jumbotron text-center">
        <h1 className="display-3">Thank You!</h1>
        <p className="lead"><strong>Your order with ID: {state.orderId} has been placed </strong> and an email has been sent to you with the details.</p>
        <hr />
        <p className="lead">
          <Link className="btn btn-primary btn-sm" to="/catalog" >Continue shopping</Link>
        </p>
      </div>
    </>
  );
};