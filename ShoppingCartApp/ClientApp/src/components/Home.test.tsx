import { render, screen } from '@testing-library/react';
import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import { Home } from './Home';

it('check if home page renders heading', async () => {
  render(<Home />);
  const heading = screen.getByText(/Online Shopping App/i);
  expect(heading).not.toBeNull();
});
