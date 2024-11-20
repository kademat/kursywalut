import React from "react";
import "../styles/Header.css"
import { NavLink } from 'react-router-dom';

const Header = () => {
  return (
    <header className="App-header navbar navbar-expand-lg navbar-light bg-light">
      <div className="container-fluid">
        <a className="navbar-brand" href="/">
          Kursy walut
        </a>
        <button
          className="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarNav"
          aria-controls="navbarNav"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span className="navbar-toggler-icon"></span>
        </button>
        <div className="collapse navbar-collapse" id="navbarNav">
          <ul className="navbar-nav">
            <li className="nav-item">
                  <NavLink 
                    to="/details" 
                    className="nav-link" 
                    activeClassName="active-link"
                  >
                    Szczegóły
                  </NavLink>

            </li>
          </ul>
        </div>
      </div>
    </header>
  );
};

export default Header;