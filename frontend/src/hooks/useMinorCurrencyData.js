import { useState, useEffect } from 'react';
import { fetchMinorCurrencyData } from '../services/minorCurrencyApi';

const useCurrencyData = () => {
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const getData = async () => {
            const response = await fetchMinorCurrencyData();
            setData(response);
            setLoading(false);
        };

        getData();
    }, []);

    return { data, loading };
};

export default useCurrencyData;