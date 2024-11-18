# Aplikacja do pobierania kursów walut wraz z ich zapisem do bazy danych

## Opis projektu
Aplikacja webowa umożliwia pobieranie aktualnych kursów walut z [NBP API](http://api.nbp.pl/), zapisywanie ich w bazie danych oraz wyświetlanie na stronie internetowej.
Projekt został zrealizowany w technologii ASP.NET Core (backend) oraz React (frontend).

## Struktura projektu
`/backend`
  `Controllers/`		# Obsługa żądań HTTP
  `Models/`				# Modele danych
  `Repositories/`		# Logika dostępu do bazy danych
  `Services/`			# Logika biznesowa
  `Data/`				# Konfiguracja bazy danych, migracje
  `Configurations/`		# Ustawienia aplikacji, integracje
  `Tests/`				# Testy jednostkowe i integracyjne

`/frontend`
  `src/`
    `components/`		# Reużywalne komponenty UI
    `pages/`			# Główne strony aplikacji
    `services/`			# Komunikacja z backendem
    `styles/`			# Pliki CSS/SCSS
    `utils/`			# Funkcje pomocnicze
    `hooks/`			# Custom hooks React
    `config/`			# Konfiguracja frontendu (np. API URL)
    `__tests__/`		# Testy jednostkowe dla frontendu
  
## Wymagania systemowe
- Node.js v16+ (dla frontendu)
- .NET 8 (dla backendu)
- SQL Server/MySQL/PostgreSQL (dla bazy danych)

## Uruchomienie lokalne
1. Backend: `cd backend && dotnet run`
2. Frontend: `cd frontend && npm start`

## Testy
- Backend: `cd backend && dotnet test`
- Frontend: `cd frontend & npm test`

## Plan rozwoju
- [✅] Przygotowanie struktury projektu (v 0.1.0)
- [ ] Wyświetlanie kursów walut (bez pobierania ich z API) (v 0.2.0)
	- Statyczne dane w tabeli React
- [ ] Pobieranie danych z API NBP (v 0.3.0)
	- Użycie HttpClient do integracji z NBP API
- [ ] Zapisywanie kursów do bazy (v 0.4.0)
- [ ] Wdrożenie projektu na produkcję (v 1.0.0)

## Linki
- [Dokumentacja NBP API](http://api.nbp.pl/)

## Uwagi dla osoby sprawdzającej
1. **Foldery i struktura projektu:**
   - Projekt podzielony jest na dwa główne foldery: `backend` i `frontend`.
   - W folderze `backend` znajdują się pliki odpowiedzialne za API oraz integrację z bazą danych.
   - W folderze `frontend` znajduje się aplikacja React, która komunikuje się z API.

2. **Priorytety projektu:**
   - Główny nacisk położony został na prostotę i czytelność kodu.
   - Wszystkie teksty i komunikaty są w języku polskim - założeniem projektu jest współpraca z polskojęzycznymi programistami oraz polskojęzycznym klientem.

3. **Testy:**
   - W projekcie dodano podstawową strukturę dla testów zarówno dla backendu (xUnit), jak i frontendu (Jest/React Testing Library).
   - Przykładowe testy zostały umieszczone w odpowiednich katalogach, aby pokazać podejście do testowania.

4. **Hosting:**
   - Aplikacja została wdrożona na serwer HTTPS. Link do działającej wersji: [https://tlmap.com](https://tlmap.com). Celem wdrożenia na serwer jest pokazanie w pełni działającej aplikacji
   w środowisku webowym.
   - Użyty certyfikat SSL pochodzi z Let's Encrypt.