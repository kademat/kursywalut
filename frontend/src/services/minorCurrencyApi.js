export const fetchMinorCurrencyData = async () => {
    try {
        // tutaj powinno być to odczytywane z pliku konfiguracyjnego
        const response = await fetch('https://tlmap.com/api/currency/minor');
        if (!response.ok) {
            throw new Error('Nie udało się pobrać kursów walut rzadzej występujących');
        }
        const data = await response.json();
        return data;
    } catch (error) {
        console.error('Błąd przy fetchowaniu danch o kursach walut:', error);
        return [];
    }
};