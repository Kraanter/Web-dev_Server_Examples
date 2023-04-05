# 1. Introductie

In dit project  ga je Entity Framework toevoegen aan je MVC app. Entity Framework is een ORM (Object Relational Mapping) framework. ORM frameworks zorgen ervoor dat je niet meer met SQL queries hoeft te werken. Je kan nu gewoon classes gebruiken om data op te slaan in de database.

## Inhoud

1. [Introductie](#1-introductie)
2. [Stappen](#2-stappen)
3. [Data modellen maken](#3-data-modellen-maken)
4. [DbContext opstellen](#4-dbcontext-opstellen-en-connectie-met-de-database-leggen)
5. [Connectie met de database leggen](#5-connectie-met-de-database-leggen)
6. [Database migreren](#6-database-migreren)
7. [Session state bijhouden](#7-session-state-bijhouden)


# 2. Stappen

1. Voeg een map genaamd Data toe aan je MVC project
    > In deze map komen alle data gerelateerde bestanden te staan zoals de DbContext en je Data modellen (de classes die je gebruikt om data op te slaan in de database)

2. Voeg een map genaamd Models toe aan de Data map
   * [Ik heb 2 models toegevoegd genaamd `Blog` en `Author`](#3-data-modellen-maken).
    > Ik heb voor de duidelijkheid de map Models genoemd, maar in de les werd deze map Entities genoemd. Ik heb gekozen voor Models omdat dit beschrijvender is voor de inhoud van de map.
    > 
    > Entities is een term die je vaak tegenkomt in de wereld van Entity Framework. Een Entity is een class die je gebruikt om data op te slaan in de database. Een Entity is dus een Data Model.

3. Voeg de volgende packages toe aan je project:
    > Om de DbContext te kunnen maken/gebruiken ben je een aantal packages nodig. Deze packages zijn te vinden in de NuGet package manager. 
    > 
    > De packages die je nodig hebt zijn:
    > * Microsoft.EntityFrameworkCore
    >   * Deze package bevat de class `DbContext` die je nodig hebt om te communiceren met de database. 
    > * Microsoft.EntityFrameworkCore.SqlServer (Als je SQL Server gebruikt `Windows`)
    >   * Deze package bevat de class `SqlServerDbContextOptionsExtensions` die je nodig hebt om de connectie met de database te leggen.
    > * Microsoft.EntityFrameworkCore.Sqlite (Als je SQLite gebruikt `Mac`)
    >   * Deze package bevat de class `SqliteDbContextOptionsExtensions` die je nodig hebt om de connectie met de database te leggen.
    > * Microsoft.EntityFrameworkCore.Tools
    >   * Deze package bevat de tools die je nodig hebt om de database te migreren.
    > 
    > `Zorg ervoor dat je de juiste versie van de packages installeert. Ik heb DOTNET 7 gebruikt. Dus zijn de versies van de packages 7.0.X`
   
4. Voeg je DbContext toe aan de Data map
    * [Ik heb een DbContext gemaakt genaamd `BlogDbContext`](TestVoorToets/Data/BlogDbContext.cs).
    > De DbContext is de class die je gebruikt om te communiceren met 1 database. De DbContext is een class die je zelf moet maken. De DbContext is een subclass van de class `DbContext` die je vindt in de namespace `Microsoft.EntityFrameworkCore`.
    >  
    > De naam maakt niet uit, maar het is wel handig om de naam van de DbContext te koppelen aan de database die je gaat gebruiken. In dit geval is de database `Blog` dus heb ik de DbContext genaamd `BlogDbContext`. De naam van de DbContext moet wel eindigen op `DbContext`.



# 3. Data modellen maken

In de map `Models` maak je de classes aan die je gaat gebruiken om data op te slaan in de database. In dit geval heb ik 2 classes gemaakt genaamd [`Blog`](TestVoorToets/Data/Models/Blog.cs) en [`Author`](TestVoorToets/Data/Models/Author.cs). 

```csharp
public class Blog
{
    public int Id { get; set; }
    
    [Required, MaxLength(100)]
    public string Title { get; set; }
    
    public string Content { get; set; }
    
    public string AuthorName { get; set; }
    
    // Navigation property zorgt voor de one kant van one to many relatie met Author op basis van AuthorName
    [ForeignKey("AuthorName")]
    public Author Author { get; set; }
}
```

```csharp
public class Author
{
    [Key, MinLength(3)]
    public string Name { get; set; }
    
    public string Email { get; set; }
    
    // Navigation property zorgt voor de many kant van one to many relatie met Blog
    public ICollection<Blog> Blogs { get; set; }
}
```

Deze classes zijn de data modellen. Deze classes zijn de classes die je gebruikt om data op te slaan in de database. De classes zijn ook de classes die je gebruikt om data uit de database te halen.

 > De classes hebben een one to many relatie. Een author kan meerdere blogs hebben. En een blog kan maar 1 author hebben.
Dit is in de classes te zien aan de property `Author` en `Blogs`. De property `Author` is een property van de class `Blog`. De property `Blogs` is een property van de class `Author`.
De many kant van de relatie is van het type `ICollection`. Dit is een interface die je gebruikt om een lijst van objecten op te slaan. In dit geval is de property `Blogs` van het type `ICollection<Blog>`. Dit betekent dat de property `Blogs` een lijst van blogs op kan slaan. En de property `Author` is van het type `Author`. Dit betekent dat de property `Author` 1 author op kan slaan.

De classes hebben een aantal attributen die gebruikt worden door Entity Framework om de database te maken. Deze attributen op 2 manieren worden aangegeven:

1. Met een attribuut in de class zelf (bijvoorbeeld `[Key]` en `[MaxLength(100)]`). Deze attributen worden Data Annotations genoemd.
2. In de `OnModelCreating` method van de DbContext (bijvoorbeeld `modelBuilder.Entity<Blog>().Property(b => b.Content).HasMaxLength(1000);`). Deze manier wordt de Fluent API genoemd.
        
Hieronder leg ik uit hoe de attributen werken:

> `Key`

Het `Key` attribuut zorgt ervoor dat de property die je aan het attribuut koppelt de primary key van de tabel wordt. In dit geval is de property `Name` de primary key van de tabel `Author`. (Dit attribuut is alleen nodig als de property niet `Id` heet)

> `ForeignKey`

Het `ForeignKey` attribuut zorgt ervoor dat de property die je aan het attribuut koppelt de foreign key van de tabel wordt. In dit geval is de property `AuthorName` de foreign key van de tabel `Blog`.
Dus als je een blog wilt opslaan in de database moet je de property `AuthorName` invullen met de naam van de author. Hierdoor kan Entity Framework de juiste author vinden in de database.

> `Required`

Het `Required` attribuut zorgt ervoor dat de property die je aan het attribuut koppelt verplicht is. In dit geval is de property `Title` verplicht.

> `MaxLength`

Het `MaxLength` attribuut zorgt ervoor dat de property die je aan het attribuut koppelt een maximum lengte heeft. In dit geval is de property `Title` maximaal 100 karakters lang.


# 4. DbContext opstellen en connectie met de database leggen

#### DbContext opstellen

---

De DbContext is de class die je gebruikt om te communiceren met 1 database. De DbContext is een class die je zelf moet maken. De DbContext is een subclass van de class `DbContext` die je vindt in de namespace `Microsoft.EntityFrameworkCore`.

De naam maakt niet uit, maar het is wel handig om de naam van de DbContext te koppelen aan de database die je gaat gebruiken. In dit geval is de database `Blog` dus heb ik de DbContext genaamd [`BlogDbContext`](TestVoorToets/Data/BlogDbContext.cs). De naam van de DbContext moet wel eindigen op `DbContext`.

```csharp
using Microsoft.EntityFrameworkCore;

public class BlogDbContext : DbContext
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
    {
    }
}
```

De DbContext heeft een constructor die een parameter `DbContextOptions<BlogDbContext>` verwacht. Deze parameter is nodig om de connectie met de database te leggen. De constructor van de DbContext roept de constructor van de superclass `DbContext` aan en geeft de parameter `options` door aan de superclass. 

 > De generieke parameter `BlogDbContext` is de huidige DbContext. Dit is nodig omdat de superclass `DbContext` een generieke class is. De superclass `DbContext` heeft een generieke parameter nodig om te weten welke DbContext het is. In dit geval is de huidige DbContext `BlogDbContext`. Stel dat je een andere DbContext maakt, dan moet je de generieke parameter aanpassen naar de naam van de nieuwe DbContext.

#### Connectie met de database leggen

---

Deze manier van de constructor opbouwen wordt in de **Buisness** `dependency injection` genoemd. Dit zorgt ervoor dat de DbContext niet zelf de opties voor de connectie met de database heeft, maar dat de connectie wordt gelegd op een andere plek. In dit geval wordt dit gedaan in de [`Program.cs`](TestVoorToets/Program.cs).

```csharp
var connectionString = builder.Configuration.GetConnectionString("BlogDatabase");

builder.Services.AddDbContext<BlogDbContext>(options => options.UseSqlite(connectionString));
```

Hier wordt de connectie met de database gemaakt. De connectie wordt gemaakt door de `AddDbContext` method aan te roepen op de `builder.Services`. Deze `builder.Services` is een `IServiceCollection` die je kan vinden in de namespace `Microsoft.Extensions.DependencyInjection`. De `AddDbContext` method verwacht een generieke parameter `BlogDbContext` en een parameter `options`. De generieke parameter `BlogDbContext` is de huidige DbContext. De parameter `options` is een anonieme functie die een parameter `DbContextOptionsBuilder` verwacht. Deze parameter `DbContextOptionsBuilder` is een class die je kan vinden in de namespace `Microsoft.EntityFrameworkCore`. De `DbContextOptionsBuilder` heeft een method `UseSqlite` die verwacht een parameter `connectionString`. 

 > Stel dat je gebruik maakt van een andere database dan SQLite, dan moet je de `UseSqlite` method aanpassen naar de method die je moet gebruiken voor de database die je gebruikt. De method die je moet gebruiken is dan `UseSqlServer` of `UseMySql` of `UseNpgsql` of `UseOracle` of `UseInMemoryDatabase`. De method die je moet gebruiken is afhankelijk van de database die je gebruikt. Deze method zit waarschijnlijk in een NuGet package die je moet installeren. 
> 
> ### Bij het gebruik van SQLServer moet je dus de `UseSqlite` method aanpassen naar `UseSqlServer`. 

 > De [`Program.cs`](TestVoorToets/Program.cs) haalt eerst de connection string op uit de [`appsettings.json`](TestVoorToets/appsettings.json) file. Deze connection string heeft alle informatie om de connectie met de database te leggen. Hier zit bijvoorbeeld de naam van de database, de gebruikersnaam en het wachtwoord. De connection string wordt opgeslagen in de variable `connectionString`.
 > 
 > Hoe je deze connectie string opstelt is [hier](#5-connection-string-opstellen) uitgelegd.


# 5. Connectie met de database leggen

De connection string is een string die alle informatie bevat om de connectie met de database te leggen. De connection string wordt opgeslagen in de [`appsettings.json`](TestVoorToets/appsettings.json) file. De connection string wordt opgeslagen in de `ConnectionStrings` sectie van de `appsettings.json` file. 

Omdat het opstellen van de connection string afhankelijk is van de database die je gebruikt, zijn hier een paar links om je ermee te helpen:
 * [Hier staan alle databases die Entity Framework Core ondersteund](https://docs.microsoft.com/en-us/ef/core/providers/?tabs=dotnet-core-cli)
 * [Hier is een lijst met connection strings voor verschillende databases](https://www.connectionstrings.com/)



De connection string die hier gebruikt wordt heeft de naam `BlogDatabase`. Omdat er gebruik wordt gemaakt van SQLite is de connection string `Data Source=blog.db`. De `Data Source` is de naam van de database. In dit geval is de database `blog.db`. De database wordt opgeslagen in de root van de project.

```json
{
    "ConnectionStrings": {
        "BlogDatabase": "Data Source=blog.db"
    }
}
```

Stel dat je gebruik maakt van een andere database, dan moet je de connection string aanpassen. Hieronder staan een aantal voorbeelden van connection strings voor andere databases:

> SQL Server localdb (standaard ingebouwde database bij Visual Studio)

```json
{
    "ConnectionStrings": {
        "BlogDatabase": "Server=(localdb)\\mssqllocaldb;Database=Blog;Trusted_Connection=True;"
    }
}
```

> SQL Server

```json
{
    "ConnectionStrings": {
        "BlogDatabase": "Server=localhost;Database=Blog;User Id=sa;Password=Password123;"
    }
}
```

> MySQL

```json
{
    "ConnectionStrings": {
        "BlogDatabase": "Server=localhost;Database=Blog;User Id=root;Password=Password123;"
    }
}
```

# 6. Database migreren

Om de database aan te maken moet je een database migratie maken. Dit verschilt voor Windows en Mac gebruikers. Dus hieronder staan de stappen voor Windows en Mac gebruikers.

### Windows gebruikers

---

1. Open de `Package Manager Console` in Visual Studio. Dit kan je doen door op `Tools` te klikken en dan op `NuGet Package Manager` en dan op `Package Manager Console` te klikken.
2. In de `Package Manager Console` moet je de `Default project` op de project zetten waar de DbContext in staat. Dit kan je doen door op `Set Default Project` te klikken en dan op de project te klikken waar de DbContext in staat.
3. Voer de commando `Add-Migration Initial` uit. Dit commando maakt een database migratie aan. De database migratie heet `Initial`. De database migratie wordt opgeslagen in de `Migrations` folder.
4. Voer de commando `Update-Database` uit. Dit commando voert de database migratie uit. De database migratie wordt uitgevoerd op de database die in de connection string staat.
5. Als je de database migratie hebt uitgevoerd, dan kan je de database openen met een database viewer. Of als je gebruik maakt van SQLite, dan kan je de database openen met een SQLite viewer.

### Mac gebruikers

---

1. Open de `Terminal` in Visual Studio. Dit kan je doen door op `View` te klikken en dan op `Terminal` te klikken.
2. Ga naar de folder waar de project staat. Dit kan je doen door de commando `cd` te gebruiken. Bijvoorbeeld: `cd TestVoorToets`.
3. Voer de commando `dotnet ef migrations add Initial` uit. Dit commando maakt een database migratie aan. De database migratie heet `Initial`. De database migratie wordt opgeslagen in de `Migrations` folder.
4. Voer de commando `dotnet ef database update` uit. Dit commando voert de database migratie uit. De database migratie wordt uitgevoerd op de database die in de connection string staat.
5. Als je de database migratie hebt uitgevoerd, dan kan je de database openen met een database viewer. Of als je gebruik maakt van SQLite, dan kan je de database openen met een SQLite viewer.

### Troubleshooting

---

 * Als het commando `dotnet ef migrations add Initial` niet werkt, dan moet je de `Microsoft.EntityFrameworkCore.Tools` NuGet package installeren.
 * Als het commando `dotnet ef migrations add Initial` de volgende error geeft: `Unable to create an object of type 'BlogContext'. For the different patterns supported at design time, see https://go.microsoft.com/fwlink/?linkid=851728`, dan moet je ervoor zorgen dat je in de [`Program.cs`](TestVoorToets/Program.cs) de dependency injection gebruikt wordt. En dit moet je doen boven de code die de builder.Build() method aanroept. Zoals te zien is in het [voorbeeld](TestVoorToets/Program.cs).
 * Als het commando `dotnet ef migrations add Inital` de volgende error geeft: `No project was found. Change the current working directory or use the --project option.`, dan moet je ervoor zorgen dat je in de `Terminal` in de folder staat waar de project staat. En als dit niet werkt kan je de extra parameter `--project` gebruiken met daarachter de naam van je project. Bijvoorbeeld: `dotnet ef migrations add Initial --project TestVoorToets`.

# 7. Session state bijhouden

Soms wil je op een pagina dingen bijhouden. Bijvoorbeeld als je een formulier hebt met meerdere stappen. Dan wil je op elke stap de ingevulde gegevens bijhouden. Dit kan je doen door de gegevens op te slaan in de `Session`. 

Om gebruik te kunnen maken van de `Session` moet je de `Session` service registreren in de [`Program.cs`](TestVoorToets/Program.cs) file. 

```csharp
// Voeg de sessie service toe
builder.Services.AddSession();

/// ...
var app = builder.Build();
/// ...

// Voeg de sessie middleware toe
app.UseSession();
```

Vervolgens kan je de `Session` gebruiken in de controller doormiddel van het `HttpContext` object. Hier is een voorbeeld van hoe je de `Session` kan gebruiken:

```csharp
public IActionResult Index()
{
    // Haal de waarde op van de key "test"
    var test = HttpContext.Session.GetString("test");

    // Zet de waarde van de key "test" op "Hello World"
    HttpContext.Session.SetString("test", "Hello World");

    return View();
}
```

Om meer dan alleen strings in de `Session` op te kunnen slaan, moet je de `Session` serialiseren. Dit kan je doen door de `System.Text.Json` package te installeren. En vervolgens kan je de `Session` serialiseren en deserialiseren met de `System.Text.Json` package. Een voorbeeld hiervan zijn de extension methods die in de [`SessionExtensions.cs`](TestVoorToets/Utils/SessionExtensions.cs) file staan. Deze extension methods kan je generiek gebruiken voor alle objecten. 

De extension methods zijn beschikbaar in het `HttpContext.Session` object. Hier is een voorbeeld van hoe je de extension methods kan gebruiken:

```csharp
public IActionResult Index()
{
    // Haal de waarde op van de key "test"
    var test = HttpContext.Session.Get<Test>("test");

    // Zet de waarde van de key "test" op een nieuwe Test object
    HttpContext.Session.Set("test", new Test());

    return View();
}

```

 > De credit voor de extension methods gaat naar [John Brouwers](https://github.com/JohnBrouwers) in zijn [Cijfer Registratie](https://github.com/JohnBrouwers/CijferRegistratie/blob/master/CijferRegistratie/Tools/ExtensionMethods.cs) project.
 > 
 > Hier is ook een voorbeeld van hoe je de extension methods kan gebruiken: [Cijfer Registratie](https://github.com/JohnBrouwers/CijferRegistratie/blob/a05939e667c2770574974463101d2519384313c5/CijferRegistratie/Controllers/HomeController.cs#L25)