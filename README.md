# Aplikacja do pobierania kursów walut z API NBP wraz z ich zapisem do bazy danych

## Opis projektu

### Aplikacja w wersji online jest dostępna na mojej stronie [tlmap](https://tlmap.com), gdzie można zobaczyć ją w działaniu bez potrzeby lokalnej konfiguracji

Aplikacja webowa umożliwia pobieranie aktualnych kursów walut z [NBP API](http://api.nbp.pl/), zapisywanie ich w repozytorium oraz wyświetlanie na stronie internetowej.
Projekt został zrealizowany w technologii ASP.NET Core 8 (backend) oraz React (frontend).
Aplikacja generuje również wykresy pokazujące zmiany kursów walut.

### Strona główna - https://tlmap.com:

![image](https://github.com/user-attachments/assets/d46d5f3a-d121-4588-8f6a-a0de3b3e5a80)
### Kursy rzadkich walut - https://tlmap.com/minor:
![image](https://github.com/user-attachments/assets/c714df59-89d7-47e3-8651-1a20b31b0918)
### Wykres pokazujący zmianę wartości waluty w czasie 90 dni - https://tlmap.com/details/USD:
![image](https://github.com/user-attachments/assets/e4811357-7c3c-4fea-89e0-8d6a474a93fa)



## Struktura projektu

### `/backend`
- `Configurations/`     # Ustawienia aplikacji, integracje
- `Controllers/`        # Obsługa żądań HTTP
- `Data/`               # Konfiguracja kontekstu bazy danych + baza danych
- `Dtos/`               # Data Transfer Objects
- `logs/`		# logi zapisywane w pliku
- `Mappers/`		# Mapowanie modeli biznesowych na dto i dto na modele biznesowe
- `Models/`             # Modele danych
- `Repositories/`       # Logika dostępu do bazy danych
- `Services/`           # Logika biznesowa
### `/backend.tests`
- `Controllers/`	# Testy kontrollerów
- `Services/`		# Testy serwisów
  
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
 - Implementacja interfejsów `IRepository` i `ICurrencyService` oraz ich wstrzykiwanie do zależnych komponentów
2. **Repository Pattern**
 - `EfRepository` i `InMemoryRepository` abstrahują dostęp do danych, izolując logikę aplikacji od szczegółów dotyczących bazy
3. **Factory Pattern**
 - `IHttpClientFactory` pozwala na tworzenie skonfigurowanych instancji `HttpClient`, zarządzając ich cyklem życia.
4 **Strategy Pattern**
 - `NbpHttpClientConfig` i konfiguracja klienta HTTP w `RegisterServices` implementują strategię konfiguracji klientów HTTP.
5. **Adapter Pattern**
 - HttpClientConfig.Configure działa jako adapter do konfiguracji klienta HTTP.
6. **Configuration Pattern**
 - Wzorzec jest zaimplementowany w klasie `AppSettingsConfig`
7. **Facade Pattern**
- `CurrencyController` działa jako warstwa upraszczająca dostęp do API, ukrywając złożoność wewnętrznej logiki.
8. **Builder Pattern**
- Wykorzystanie `IServiceCollection` w `Program.cs` do rejestrowania usług i konfiguracji aplikacji.
9. **Decorator Pattern**
- Dodanie obsługi retry dla klienta HTTP przy użyciu `AddTransientHttpErrorPolicy`.

## Linki
- [Dokumentacja NBP API](http://api.nbp.pl/)

## Uwagi dla osoby sprawdzającej
1. **Foldery i struktura projektu:**
   - Projekt podzielony jest na trzy główne foldery: `backend`, `frontend` oraz `backend.tests`.
   - W folderze `backend` znajdują się pliki odpowiedzialne za API oraz integrację z bazą danych.
   - W folderze `frontend` znajduje się aplikacja React, która komunikuje się z API.
   - W folderze `backend.tests` znajdują się testy jednostkowe

2. **Priorytety projektu:**
   - Główny nacisk położony został na prostotę, czytelność oraz możliwość rozszerzania kodu z zastosowaniem wzorców projektowych.
   - Teksty i komunikaty są w języku polskim - założeniem projektu jest współpraca z polskojęzycznymi programistami oraz polskojęzycznym klientem.

3. **Testy:**
   - W projekcie dodano podstawową strukturę dla testów zarówno dla backendu (xUnit), jak i frontendu (Jest).
   - Przykładowe testy zostały umieszczone w odpowiednich katalogach, aby pokazać podejście do testowania.

4. **Hosting:**
   - Aplikacja została wdrożona na serwer HTTPS. Link do działającej wersji: [https://tlmap.com](https://tlmap.com). Celem wdrożenia na serwer jest pokazanie w pełni działającej aplikacji
   w środowisku webowym.
   - Użyty certyfikat SSL pochodzi z Let's Encrypt.
