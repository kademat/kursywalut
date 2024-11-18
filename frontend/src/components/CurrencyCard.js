import React from 'react';

const CurrencyCard = ({ currency, value }) => {
    return (
        <div className="currency-card">
            <h3>{currency}</h3>
            <p>{value}</p>
        </div>
    );
};

export default CurrencyCard;