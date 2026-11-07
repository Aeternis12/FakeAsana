# FakeAsana

FakeAsana is a project and task management application built with C# and .NET. It includes a Blazor web client, an ASP.NET Core Web API, shared models, and a SQL Server database project.

> This is an independent project and not connected at all with Asana

## Features

- Create, view, update, and delete projects
- Create, view, update, and delete tasks within projects
- Track task priority, due date, and completion status
- View project completion percentages based on completed tasks
- Browse projects and tasks in a Blazor web interface
- Access project and task data through HTTP API endpoints

## Technology

- C# and .NET 10
- ASP.NET Core Web API
- Blazor WebAssembly and Razor components
- SQL Server and Dapper
- SQL Server Database Project

## Project structure

| Project | Purpose |
|---|---|
| `FakeAsana_Blazor` | Blazor WebAssembly client and its API service proxies |
| `FakeAsana.API` | ASP.NET Core API controllers, application services, and SQL data access |
| `Asana.Library` | Shared project and task models, plus HTTP client utilities |
| `Asana.Database` | SQL Server table definitions for projects and tasks |
| `Asana.CLI` | Command-line client project |
| `Asana.Maui` | .NET MAUI client project |

The web client calls the API over HTTP. The API handles project and task operations and uses Dapper to communicate with SQL Server.

## Requirements

- Windows
- .NET 10 SDK
- SQL Server LocalDB
- Visual Studio with SQL Server Data Tools (SSDT) to publish the database project

## Run the web application locally

### 1. Set up the database

Publish `Asana.Database/Asana.Database.sqlproj` to a local SQL Server database named `AsanaDb`. You can do this from Visual Studio using **Build > Publish** on the database project.

The API currently uses a LocalDB connection string defined in `FakeAsana.API/Database/DatabaseService.cs`. The connection string is hardcoded, so update it there if your SQL Server instance or database name differs.

### 2. Start the API

From the repository root, run:

```bash
dotnet run --project FakeAsana.API --launch-profile https
```

The API's HTTPS profile is configured for https://localhost:7127. Swagger is available at: ```https://localhost:7127/swagger```



### 3. Start the Blazor client

In a separate terminal, run:
```bash
dotnet run --project FakeAsana_Blazor --launch-profile https
```

Open the client URL printed in the terminal. The HTTPS launch profile is configured for  ```https://localhost:7200```


The API URL is currently hardcoded in Asana.Library/Util/WebRequestHandler.cs as https://localhost:7127. If you change the API port, update that URL as well.
API routes

| Method | Route Description |
|---|---|
| `GET /Project` |List projects
| `GET /Project/Expand`|	List projects with their tasks |
| `GET /Project/{id}` | Get a project by ID |
| `POST /Project` | Create or update a project |
| `DELETE /Project/{id}` | Delete a project |
| `GET /ToDo` | List tasks |
| `GET /ToDo/{id}` | Get a task by ID |
| `GET /ToDo/Project/{id}` | List tasks for a project |
| `POST /ToDo` | Create or update a task |
| `DELETE /ToDo/{id}` | Delete a task |


## Data model

A project has a name, description, and calculated completion percentage. Tasks belong to a project and can have a name, description, priority, due date, and completion status. The SQL schema defines a foreign key from tasks to projects and cascades task deletion when a project is deleted.
Current limitations

    The database connection string and API base URL are hardcoded for local development.
    The API currently uses SQL Server LocalDB; it is not configured for cloud deployment.
    No automated test project is currently included.
    The repository does not currently include CI/CD configuration.

## Potential next steps

    Move database and API URLs into environment-specific configuration.
    Add automated tests for API endpoints and business logic.
    Add request validation and user-facing error handling.
    Review accessibility and keyboard navigation in the web interface.
    Document and configure a deployment environment.

