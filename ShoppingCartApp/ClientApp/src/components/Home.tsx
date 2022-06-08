import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render() {
    return (
      <div>
        <h3>Online Shopping App !</h3>
        <a href={'/catalog'}>Shop here</a>

        <br /> <br />I planned to use the following but due to limited time I couldn&lsquo;t achieve it. :-(<br />
          1 - Automapper in C# to map DTOs to Entities<br />
          2 - MediatR to handle the web APIs requests providing an abstraction layer between the request and the handler<br />


      </div>
    );
  }
}
