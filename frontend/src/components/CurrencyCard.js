import React from 'react';

const CurrencyCard = ({ currency, showDetailsButton, onShowDetails }) => {
    return (
        <div className="currency-card">
            <h3>{currency.currency}</h3>
            <p><strong>Wartość:</strong> {currency.mid}</p>
            <p><strong>Kod:</strong> {currency.code}</p>
            {showDetailsButton && (
                <button onClick={() => onShowDetails(currency)}>
                    Pokaż wartości historyczne
                </button>
            )}
        </div>
    );
};

export default CurrencyCard;