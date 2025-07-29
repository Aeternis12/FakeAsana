using FakeAsana_Blazor;
using FakeAsana_Blazor.Services;
using Asana.Library.Util;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<WebRequestHandler>();
builder.Services.AddScoped<IProjectService, ProjectServiceProxy>();
builder.Services.AddScoped<IToDoService, ToDoServiceProxy>();


await builder.Build().RunAsync();
