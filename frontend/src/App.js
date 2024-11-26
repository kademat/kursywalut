import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import CurrencyDetailsPage from './pages/Currency/CurrencyDetailsPage';
import MinorCurrencyPage from './pages/Currency/MinorCurrencyPage';
import SingleCurrencyDetailsPage from './pages/Currency/SingleCurrencyDetailsPage';
import HomePage from './pages/Home/HomePage';
import Header from './components/Header';
import './styles/App.css';

const App = () => {
    return (
        <Router>
            <div className="App">
                <Header />
                <Routes>
                    <Route path="/" element={<HomePage />} />
                    <Route path="/minor" element={<MinorCurrencyPage />} />
                    <Route path="/details" element={<CurrencyDetailsPage />} />
                    <Route path="/details/:currencyCode" element={<SingleCurrencyDetailsPage />} />
                </Routes>
            </div>
        </Router>
    );
};

export default App;

