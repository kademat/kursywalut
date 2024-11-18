import { useState, useEffect } from 'react';
import { fetchCurrencyData } from '../services/currencyApi';

const useCurrencyData = () => {
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const getData = async () => {
            const response = await fetchCurrencyData();
            setData(response);
            setLoading(false);
        };

        getData();
    }, []);

    return { data, loading };
};

export default useCurrencyData;