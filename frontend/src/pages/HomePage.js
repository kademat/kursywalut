import React from 'react';
import CurrencyCard from '../components/CurrencyCard';
import useCurrencyData from '../hooks/useCurrencyData';

const HomePage = () => {
    const { data, loading } = useCurrencyData();

    if (loading) return <p>Loading...</p>;

    return (
        <div>
            <h1>Kursy walut</h1>
            <table>
                <thead>
                    <tr>
                        <th>Waluta</th>
                        <th>Wartość</th>
                    </tr>
                </thead>
                <tbody>
                    {data.map((currency) => (
                        <tr key={currency.code}>
                            <td>{currency.currency}</td>
                            <td>{currency.mid}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default HomePage;