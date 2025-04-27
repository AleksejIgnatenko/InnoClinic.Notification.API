using InnoClinic.Notification.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureBuilder();

var app = builder
    .Build()
    .ConfigureApplicationAsync();

app.Run();