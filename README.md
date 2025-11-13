# BeFit

BeFit to prosta aplikacja webowa do rejestrowania treningów siłowych, zbudowana w ASP.NET Core MVC z użyciem Entity Framework Core i Identity.

## Technologie

- ASP.NET Core MVC
- Entity Framework Core (Code First)
- ASP.NET Core Identity (logowanie, rejestracja, role)
- Baza danych konfigurowana w `appsettings.json` (dostarczony jest przykładowy plik bazy)

## Funkcjonalności

### Typy ćwiczeń

- Lista wszystkich dostępnych typów ćwiczeń (np. przysiad, wyciskanie, martwy ciąg)
- Dodawanie / edytowanie / usuwanie typów ćwiczeń tylko dla roli **Administrator**
- Przeglądanie listy typów ćwiczeń dostępne dla wszystkich (również niezalogowanych)

### Sesje treningowe

- Tworzenie sesji treningowych przez zalogowanych użytkowników
- Każda sesja ma datę rozpoczęcia i zakończenia (walidacja: koniec > początek)
- Sesje są powiązane z konkretnym użytkownikiem
- Użytkownik widzi, edytuje i usuwa tylko **swoje** sesje

### Wykonane ćwiczenia

- Dodawanie wykonanych ćwiczeń w ramach sesji treningowej
- Przechowywana jest liczba serii, liczba powtórzeń w serii oraz obciążenie
- Użytkownik widzi, edytuje i usuwa tylko **swoje** wykonane ćwiczenia
- Dane są powiązane z wybraną sesją treningową i typem ćwiczenia

### Statystyki (ostatnie 4 tygodnie)

Dla zalogowanego użytkownika, na podstawie sesji z ostatnich 28 dni, aplikacja wyświetla:

- ile razy dany typ ćwiczenia był wykonywany,
- łączną liczbę powtórzeń (suma: serie × powtórzenia),
- średnie obciążenie,
- maksymalne obciążenie.

Statystyki wyliczane są tylko z danych zalogowanego użytkownika.

## Nawigacja

- Strona główna (`/`) zawiera skróty do:
  - listy typów ćwiczeń,
  - listy sesji treningowych,
  - strony ze statystykami.
- W menu (layout) dostępne są linki do:
  - **Typów ćwiczeń**
  - **Sesji treningowych**
  - **Wykonanych ćwiczeń**
  - **Statystyk**

## Uruchomienie

Wymagania: zainstalowany .NET SDK (zgodny z wersją używaną w projekcie).

1. Sklonuj repozytorium:

   ```bash
   git clone https://github.com/Wriggly14/BeFit.git
   cd BeFit


2. (Opcjonalnie) Jeśli chcesz odtworzyć bazę danych od zera, usuń istniejący plik bazy i wykonaj migracje:

dotnet tool install --global dotnet-ef
dotnet ef database update


3. Uruchom aplikację:

dotnet run


4. Wejdź w przeglądarce na adres podany w konsoli (np. https://localhost:5001).


5. Konto administratora

Przy starcie aplikacji tworzona jest rola Administrator oraz konto administracyjne:

Login: admin@befit.local

Hasło: Admin123!

Konto administratora służy do zarządzania typami ćwiczeń.
Zwykły użytkownik może zarejestrować się przez formularz rejestracji i tworzyć własne sesje treningowe oraz wykonane ćwiczenia.