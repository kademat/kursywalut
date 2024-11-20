import React, { useState, useEffect } from 'react';
import { useParams, useLocation } from 'react-router-dom';
import { Line } from 'react-chartjs-2';
import { Chart as ChartJS, CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend } from 'chart.js';

// Rejestracja komponentów Chart.js
ChartJS.register(
    CategoryScale,
    LinearScale,
    PointElement,
    LineElement,
    Title,
    Tooltip,
    Legend
);

const SingleCurrencyDetailsPage = () => {
    const { currencyCode } = useParams(); // Pobranie kodu waluty z URL
    const location = useLocation();
    const currency = location.state?.currency || { currency: currencyCode, mid: 'Nieznane' };

    const [chartData, setChartData] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchCurrencyHistory = async () => {
            try {
                // jako, że wykresy nie są częścią zadania jest tu prosty fetch bezpośrednio do API. Ta część została dodana
                // ze względu na chęć sprawdzenia wykresów walut
                const response = await fetch(`https://api.nbp.pl/api/exchangerates/rates/a/${currencyCode}/last/90/?format=json`);
                const data = await response.json();

                const labels = data.rates.map(rate => rate.effectiveDate); // daty
                const values = data.rates.map(rate => rate.mid); // wartości kursów

                setChartData({
                    labels,
                    datasets: [
                        {
                            label: `Kurs ${currency.currency}`,
                            data: values,
                            borderColor: 'rgb(75, 192, 192)',
                            backgroundColor: 'rgba(75, 192, 192, 0.2)',
                            fill: true,
                        },
                    ],
                });
            } catch (error) {
                console.error("Błąd pobierania danych:", error);
            } finally {
                setLoading(false);
            }
        };

        fetchCurrencyHistory();
    }, [currencyCode]);

    if (loading) {
        return <p>Ładowanie danych...</p>;
    }

    return (
        <div>
            <h1>Wartości historyczne waluty: {currency.currency}</h1>
            <p>Wykres przedstawiający ostatnie 90 dni kursu waluty:</p>

            {chartData && (
                <div style={{ width: '80%', height: '400px', margin: 'auto' }}>
                    <Line data={chartData} />
                </div>
            )}
        </div>
    );
};

export default SingleCurrencyDetailsPage;