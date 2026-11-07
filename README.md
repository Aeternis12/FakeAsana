# FakeAsana

A project and task management app built with C#, ASP.NET Core, Blazor WebAssembly, and SQL Server. Users can create and manage projects, organize tasks, track completion, and view project progress.

## Features

- Create, view, update, and delete projects
- Create, view, update, and delete tasks within projects
- Track task priority, due date, and completion status
- Calculate project completion percentage from its tasks
- Use a Blazor web interface backed by a REST-style API

## Tech stack

- C# and .NET 10
- ASP.NET Core Web API
- Blazor WebAssembly
- SQL Server and Dapper
- SQL Server Database Project

## Architecture

- **FakeAsana_Blazor** — Web UI and API client services
- **FakeAsana.API** — HTTP API, controllers, and application services
- **Asana.Library** — Shared project and task models
- **Asana.Database** — SQL Server table definitions

The web client sends HTTP requests to the API. The API handles project and task operations and uses Dapper to access SQL Server.

## Requirements

- .NET 10 SDK
- Windows with SQL Server LocalDB
- Visual Studio with SQL Server Data Tools (SSDT), to deploy the database project

## Run locally

1. Deploy `Asana.Database` to a local SQL Server database named `AsanaDb`.
2. Start the API:

   ```bash
   dotnet run --project FakeAsana.API
