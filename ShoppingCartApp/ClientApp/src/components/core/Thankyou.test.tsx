import React from 'react';
import ReactDOM from 'react-dom';
import { MemoryRouter } from 'react-router-dom';

import { render, fireEvent, waitFor } from '@testing-library/react';
import { shallow } from 'enzyme';
import { ThankYou } from './Thankyou';

it('renders without crashing', async () => {

  const div = document.createElement('div');
  ReactDOM.render(
    <MemoryRouter>
      <ThankYou />
    </MemoryRouter>, div);
  await new Promise(resolve => setTimeout(resolve, 1000));
});