import React from 'react';
import { render, screen, waitFor } from '@testing-library/react';
import HomePage from '../pages/Home/HomePage';
import useCurrencyData from '../hooks/useCurrencyData';

// Mockowanie hooka useCurrencyData
jest.mock('../hooks/useCurrencyData');

describe('HomePage', () => {
    it('na początku powinien wyświetlić status ładowania', () => {
        // Mockujemy loading = true i brak danych
        useCurrencyData.mockReturnValue({ data: [], loading: true });

        render(<HomePage />);

        // Sprawdzamy, czy komunikat "Loading..." jest wyświetlany
        expect(screen.getByText(/Loading.../i)).toBeInTheDocument();
    });

    it('powinien wyświetlić karty z kursem walut jak dane zostaną załadowane', async () => {
        // Mockowanie zwróconych danych po załadowaniu
        const mockData = [
            { code: 'USD', currency: 'dolar amerykański', mid: 4.5 },
            { code: 'EUR', currency: 'euro', mid: 4.2 },
            { code: 'GBP', currency: 'funt brytyjski', mid: 5.3 },
        ];

        useCurrencyData.mockReturnValue({ data: mockData, loading: false });

        render(<HomePage />);

        // Sprawdzamy, czy kursy walutowe są wyświetlane
        for (const currency of mockData) {
            const codeText = await screen.findByText(currency.code);
            const nameText = await screen.findByText(currency.currency);
            const midText = await screen.findByText(currency.mid.toString());

            expect(codeText).toBeInTheDocument();
            expect(nameText).toBeInTheDocument();
            expect(midText).toBeInTheDocument();
        }
    });

    it('nie powinien wyświetlać wiadmości o ładowaniu gdy dane zostaną załadowane', async () => {
        const mockData = [
            { code: 'USD', currency: 'USD', mid: 4.5 },
            { code: 'EUR', currency: 'EUR', mid: 4.2 },
        ];

        useCurrencyData.mockReturnValue({ data: mockData, loading: false });

        render(<HomePage />);

        // Upewniamy się, że komunikat "Loading..." nie jest widoczny po załadowaniu danych
        expect(screen.queryByText(/Loading.../i)).toBeNull();
    });
});