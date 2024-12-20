﻿import React from 'react';
import CurrencyList from '../../components/CurrencyList';
import useCurrencyList from '../../hooks/useCurrencyData';
import { useNavigate } from 'react-router-dom';

const CurrencyDetailsPage = () => {
    const navigate = useNavigate();
    const { data, loading } = useCurrencyList();

    const showSingleCurrencyDetails = (currency) => {
        navigate(`/details/${currency.code}`, { state: { currency } });
    };

    if (loading) return <p>Loading...</p>;

    return (<>
        <h1>Wartości historyczne głównych walut (wykres zmian z 90 dni)</h1>
        <CurrencyList
        data={data}
        showDetailsButton={true}
        onShowDetails={showSingleCurrencyDetails}
    /></>);
};

export default CurrencyDetailsPage;