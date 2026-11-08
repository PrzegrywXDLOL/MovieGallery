# MovieGallery

A web application for browsing a movie collection, rating films, and reading reviews. It also includes an AI assistant that answers questions using movie information available in the application.

[Read this README in Polish](#wersja-polska) — the Polish version is available below.

## Features

- Browse the movie catalogue and search for titles.
- View movie details, including description, cast, director, release date, genres, and poster.
- Read and submit movie reviews and ratings.
- Register and sign in using ASP.NET Core Identity.
- Admin features for adding and editing movies and managing user roles. Admin operations require the `Admin` role.
- Ask the AI assistant questions about movies in the catalogue.
- Retrieve the catalogue through `GET /api/Movies`.

## Technology

- C# and ASP.NET Core on .NET 9.
- ASP.NET Core MVC with Razor views; Razor Pages are used for Identity account features.
- Entity Framework Core and Microsoft SQL Server.
- ASP.NET Core Identity.
- Google Generative AI integrated through `Microsoft.Extensions.AI`.

## Requirements

- .NET 9 SDK.
- Microsoft SQL Server, either locally or on a server accessible to the application.
- A Gemini API key to use the AI assistant.

## Run locally

1. Clone the repository and enter the project directory:

   git clone https://github.com/PrzegrywXDLOL/MovieGallery.git
   cd MovieGallery

2. Check the `DefaultConnection` connection string in `appsettings.json`. The default configuration uses a local SQL Server instance `(local)` and a database named `MovieGallery`. Update it to match your SQL Server setup if needed.

3. Apply the Entity Framework Core migrations:

   dotnet ef database update

If the `dotnet ef` command is unavailable, install the EF Core CLI tool:

   dotnet tool install --global dotnet-ef

4. (Optional) Set the Gemini API key as an environment variable. The application reads it from `GEMINI_API_KEY`:

   $env:GEMINI_API_KEY = "YOUR_API_KEY"

This setting applies to the current PowerShell session. Do not commit API keys to the repository.

5. Restore dependencies and run the application:

   dotnet restore
   dotnet run

Open the HTTP or HTTPS address printed in the console.

> Account confirmation is required before sign-in. Account registration and sign-in therefore also depend on the account-confirmation setup in the environment.

## API

`GET /api/Movies` returns the movie catalogue, including reviews. The local URL will look like this:

https://localhost:<port>/api/Movies


Find the port in the console output when the application starts.

## Project structure

- `Controllers/` — application and API controllers.
- `Views/` — Razor views for the MVC interface.
- `Models/` — domain models and view models.
- `Services/` — application and data-access services.
- `Data/` — Entity Framework Core context and migrations.
- `Agents/` — AI assistant integration.
- `Areas/Identity/` — Razor Pages for user account features.
- `wwwroot/` — static files, including CSS, JavaScript, and image assets.

---

# MovieGallery

Aplikacja internetowa do przegl¹dania kolekcji filmów, oceniania ich i czytania recenzji. Zawiera równie¿ asystenta AI, który odpowiada na pytania na podstawie informacji o filmach dostêpnych w aplikacji.

## Funkcje

- Przegl¹danie katalogu filmów i wyszukiwanie tytu³ów.
- Wyœwietlanie szczegó³ów filmu, w tym opisu, obsady, re¿ysera, daty premiery, gatunków i plakatu.
- Czytanie i dodawanie recenzji oraz ocen filmów.
- Rejestracja i logowanie za pomoc¹ ASP.NET Core Identity.
- Funkcje administracyjne: dodawanie i edytowanie filmów oraz zarz¹dzanie rolami u¿ytkowników. Operacje administracyjne wymagaj¹ roli `Admin`.
- Zadawanie asystentowi AI pytañ o filmy z katalogu.
- Pobieranie katalogu przez endpoint `GET /api/Movies`.

## Technologie

- C# i ASP.NET Core na .NET 9.
- ASP.NET Core MVC z widokami Razor; Razor Pages obs³uguj¹ funkcje kont Identity.
- Entity Framework Core i Microsoft SQL Server.
- ASP.NET Core Identity.
- Google Generative AI zintegrowane przez `Microsoft.Extensions.AI`.

## Wymagania

- .NET 9 SDK.
- Microsoft SQL Server — lokalna instancja lub serwer dostêpny dla aplikacji.
- Klucz API Gemini, jeœli chcesz korzystaæ z asystenta AI.

## Uruchomienie lokalne

1. Sklonuj repozytorium i przejdŸ do katalogu projektu:

   git clone https://github.com/PrzegrywXDLOL/MovieGallery.git
   cd MovieGallery

2. SprawdŸ connection string `DefaultConnection` w pliku `appsettings.json`. Domyœlna konfiguracja korzysta z lokalnej instancji SQL Server `(local)` i bazy danych `MovieGallery`. W razie potrzeby dostosuj j¹ do swojej konfiguracji SQL Server.

3. Zastosuj migracje Entity Framework Core:

   dotnet ef database update

Jeœli polecenie `dotnet ef` nie jest dostêpne, zainstaluj narzêdzie EF Core CLI:

   dotnet tool install --global dotnet-ef

4. (Opcjonalnie) Ustaw klucz API Gemini jako zmienn¹ œrodowiskow¹. Aplikacja odczytuje go ze zmiennej `GEMINI_API_KEY`:

   $env:GEMINI_API_KEY = "TWÓJ_KLUCZ_API"

To ustawienie obowi¹zuje w bie¿¹cej sesji PowerShell. Nie umieszczaj kluczy API w repozytorium.

5. Przywróæ zale¿noœci i uruchom aplikacjê:

   dotnet restore
   dotnet run

Otwórz adres HTTP lub HTTPS wyœwietlony w konsoli.

> Przed zalogowaniem wymagane jest potwierdzenie konta. Rejestracja i logowanie zale¿¹ wiêc równie¿ od konfiguracji potwierdzania konta w danym œrodowisku.

## API

`GET /api/Movies` zwraca katalog filmów wraz z recenzjami. Lokalny adres bêdzie wygl¹da³ podobnie do poni¿szego:

https://localhost:<port>/api/Movies


Port znajdziesz w komunikacie konsoli wyœwietlanym po uruchomieniu aplikacji.

## Struktura projektu

- `Controllers/` — kontrolery aplikacji i API.
- `Views/` — widoki Razor interfejsu MVC.
- `Models/` — modele domenowe i modele widoków.
- `Services/` — us³ugi aplikacyjne i dostêpu do danych.
- `Data/` — kontekst Entity Framework Core i migracje.
- `Agents/` — integracja asystenta AI.
- `Areas/Identity/` — strony Razor zwi¹zane z kontami u¿ytkowników.
- `wwwroot/` — pliki statyczne, w tym CSS, JavaScript i grafiki.
