import React from "react";
import "../styles/Header.css"

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
              <a className="nav-link" href="/details">
                Szczegóły
              </a>
            </li>
            <li className="nav-item">
              {/*<a className="nav-link" href="/strona2">*/}
              {/*  Strona 2*/}
              {/*</a>*/}
            </li>
            <li className="nav-item">
              {/*<a className="nav-link" href="/strona3">*/}
              {/*  Strona 3*/}
              {/*</a>*/}
            </li>
          </ul>
        </div>
      </div>
    </header>
  );
};

export default Header;