export const fetchCurrencyData = async () => {
    try {
        const response = await fetch('https://tlmap.com/api/currency');
        if (!response.ok) {
            throw new Error('Nie udało się pobrać kursów walut');
        }
        const data = await response.json();
        return data;
    } catch (error) {
        console.error('Błąd przy fetchowaniu danch o kursach walut:', error);
        return [];
    }
};