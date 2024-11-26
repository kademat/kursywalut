# Aplikacja do pobierania kursów walut z API NBP wraz z ich zapisem do repozytorium

## Opis projektu

### Aplikacja w wersji online jest dostępna na mojej stronie [tlmap](https://tlmap.com), gdzie można zobaczyć ją w działaniu bez potrzeby lokalnej konfiguracji

Aplikacja webowa umożliwia pobieranie aktualnych kursów walut z [NBP API](http://api.nbp.pl/), zapisywanie ich w repozytorium oraz wyświetlanie na stronie internetowej.
Projekt został zrealizowany w technologii ASP.NET Core 8 (backend) oraz React (frontend).
Aplikacja generuje również wykresy pokazujące zmiany kursów walut.

### Strona główna:
![image](https://github.com/user-attachments/assets/a76ee131-f345-48ae-b5e6-44b39df70f62)
### Wykres pokazujący zmianę wartości waluty w czasie 90 dni:
![image](https://github.com/user-attachments/assets/ad9c9c82-b2f0-4f23-912e-a2f09169b838)


## Struktura projektu

### `/backend`
- `Controllers/`        # Obsługa żądań HTTP
- `Models/`             # Modele danych
- `Repositories/`       # Logika dostępu do bazy danych
- `Services/`           # Logika biznesowa
- `Data/`               # Konfiguracja bazy danych, migracje
- `Configurations/`     # Ustawienia aplikacji, integracje
- `Tests/`              # Testy jednostkowe i integracyjne

### `/frontend`
- `src/`
  - `components/`       # Reużywalne komponenty UI
  - `pages/`            # Główne strony aplikacji
  - `services/`         # Komunikacja z backendem
  - `styles/`           # Pliki CSS/SCSS
  - `utils/`            # Funkcje pomocnicze
  - `hooks/`            # Custom hooks React
  - `config/`           # Konfiguracja frontendu (np. API URL)
  - `__tests__/`        # Testy jednostkowe dla frontendu
  
## Wymagania systemowe
- Node.js v16+ (dla frontendu)
- .NET 8 (dla backendu)

## Uruchomienie lokalne
1. Backend: `cd backend && dotnet run`
2. Frontend: `cd frontend && npm start`

## Testy
- Backend: `cd backend && dotnet test`
- Frontend: `cd frontend & npm test`

## Plan rozwoju
- [x] Przygotowanie struktury projektu (v 0.1.0)
- [x] Wyświetlanie kursów walut (bez pobierania ich z API) (v 0.2.0)
	- Statyczne dane w tabeli React. Dodanie testów do frontendu.
- [x] Pobieranie danych z API NBP (v 0.3.0)
	- Użycie HttpClient do integracji z NBP API
- [x] Zapisywanie kursów do źródła danych reprezentującego bazę (v 0.4.0)
- [x] Wdrożenie projektu na produkcję (tlmap.com) (v 1.0.0)

## Zastosowane wzorce projektowe

1. **Dependency Injection**
 - M.in. implementacja interfejsów IRepository i ICurrencyService oraz ich wstrzykiwanie do zależnych komponentów
2. **Repository**
 - IRepository wraz z zachowaniem Single Responsibility
3. **Factory**
 - IHttpClientFactory pozwala na tworzenie skonfigurowanych instancji HttpClient, zarządzając ich cyklem życia.
4 **Strategy**
 - Polityka powtórzeń błędów zaimplementowana w HttpClient za pomocą biblioteki Polly.
5. **Adapter**
 - HttpClientConfig.Configure działa jako adapter do konfiguracji klienta HTTP.
6. **Configuration**
 - Wzorzec Options jest zaimplementowany w klasie AppSettingsConfig
7. **Facade**
- CurrencyController działa jako warstwa upraszczająca dostęp do API, ukrywając złożoność wewnętrznej logiki.

## Linki
- [Dokumentacja NBP API](http://api.nbp.pl/)

## Uwagi dla osoby sprawdzającej
1. **Foldery i struktura projektu:**
   - Projekt podzielony jest na dwa główne foldery: `backend` i `frontend`.
   - W folderze `backend` znajdują się pliki odpowiedzialne za API oraz integrację z repozytorium.
   - W folderze `frontend` znajduje się aplikacja React, która komunikuje się z API.

2. **Priorytety projektu:**
   - Główny nacisk położony został na prostotę, czytelność oraz możliwość rozszerzania kodu.
   - Wszystkie teksty i komunikaty są w języku polskim - założeniem projektu jest współpraca z polskojęzycznymi programistami oraz polskojęzycznym klientem.

3. **Testy:**
   - W projekcie dodano podstawową strukturę dla testów zarówno dla backendu (xUnit), jak i frontendu (Jest/React Testing Library).
   - Przykładowe testy zostały umieszczone w odpowiednich katalogach, aby pokazać podejście do testowania.

4. **Hosting:**
   - Aplikacja została wdrożona na serwer HTTPS. Link do działającej wersji: [https://tlmap.com](https://tlmap.com). Celem wdrożenia na serwer jest pokazanie w pełni działającej aplikacji
   w środowisku webowym.
   - Użyty certyfikat SSL pochodzi z Let's Encrypt.
