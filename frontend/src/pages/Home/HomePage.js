import React from 'react';
import CurrencyList from '../../components/CurrencyList';
import useCurrencyList from '../../hooks/useCurrencyData';

const HomePage = () => {
    const { data, loading } = useCurrencyList();

    if (loading) return <p>Loading...</p>;

    return (<>
        <h1>Kursy głównych (popularnych) walut</h1>
        <CurrencyList data={data} showDetailsButton={false} />
    </>);
};

export default HomePage;