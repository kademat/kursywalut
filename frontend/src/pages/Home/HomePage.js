import React from 'react';
import CurrencyList from '../../components/CurrencyList';
import useCurrencyList from '../../hooks/useCurrencyData';

const HomePage = () => {
    const { data, loading } = useCurrencyList();

    if (loading) return <p>Loading...</p>;

    return (<CurrencyList data={data} showDetailsButton={false} />);
};

export default HomePage;