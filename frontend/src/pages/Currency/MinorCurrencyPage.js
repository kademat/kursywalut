import React from 'react';
import CurrencyList from '../../components/CurrencyList';
import useMinorCurrencyList from '../../hooks/useMinorCurrencyData';

const MinorCurrencyPage = () => {
    const { data, loading } = useMinorCurrencyList();

    if (loading) return <p>Loading...</p>;
    return (<>
        <h1>Kursy rzadszych walut</h1>
        <CurrencyList
        data={data}
    /></>);
};

export default MinorCurrencyPage;