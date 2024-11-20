import React from 'react';
import CurrencyCard from "./CurrencyCard";

const CurrencyList = ({ data, showDetailsButton, onShowDetails }) => {
    return (
        <div className="currency-container">
            {data.map((currency) => (
                <CurrencyCard
                    key={currency.code}
                    currency={currency}
                    showDetailsButton={showDetailsButton}
                    onShowDetails={onShowDetails}
                />
            ))}
        </div>
    );
};

export default CurrencyList;