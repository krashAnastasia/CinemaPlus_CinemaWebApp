# CinemaPlus

`CinemaPlus` — вебзастосунок кінотеатру на `ASP.NET Core MVC` для дипломного проєкту.  
Клієнтська частина зберігає наявний дизайн, адміністративна панель перенесена на `.NET`, база даних працює через `EF Core` і `MySQL`.

## Що є в проєкті

- каталог `У КІНО` і сторінка `СКОРО`
- сторінка `СЕАНСИ` з фільтрами
- сторінка фільму з сеансами
- динамічна схема залу та вибір місць
- оформлення замовлення, емуляція оплати, створення квитка
- `QR` і завантаження квитка у `PDF`
- реєстрація, вхід, кабінет користувача, історія квитків
- ролі `Guest`, `Client`, `Admin`
- адміністрування фільмів, залів, сеансів, бронювань
- статистика та експорт звітів у `Excel`

## Технології

- `ASP.NET Core MVC`
- `Entity Framework Core`
- `MySql.EntityFrameworkCore`
- `PdfSharpCore`
- `QRCoder`
- `xUnit` для тестів

## Вимоги

- `.NET SDK 10`
- `MySQL`

## Налаштування

Основні параметри зберігаються в `appsettings.Development.json`.

- `ConnectionStrings:DefaultConnection` — підключення до локальної `MySQL`
- `Storage:UploadsPath` — папка для постерів і трейлерів
- `Storage:ReportsPath` — папка для згенерованих звітів

## Запуск

```bash
dotnet build
dotnet tool run dotnet-ef database update
dotnet run
```

## Тестування

```bash
dotnet test
```

Автотести запускаються на `SQLite`, щоб не залежати від локальної `MySQL`.
