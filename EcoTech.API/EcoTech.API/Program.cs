global using System;
global using System.Collections.Generic;
global using System.Linq;
global using FastEndpoints;
global using EcoTech.Domain.Entities;
global using EcoTech.Application.AppSetup;
global using Mediator;
global using EcoTech.Domain.AppSetup;
global using EcoTech.API.StartupExtentions;
global using EcoTech.Infrastructure.AppSetup;
global using EcoTech.API.Filters;
global using EcoTech.Shared.Constants;
global using FastEndpoints.Security;
global using System.Reflection;
global using EcoTech.Shared.Extensions;
global using System.Security.Cryptography;
global using System.Text;
global using EcoTech.Domain.FeatureEntities;
global using EcoTech.Application.BackGroundServices;
global using EcoTech.Domain.UtilityEntities;
global using Utf8Json; 


var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureInfrastructure(builder.Configuration, builder.Configuration)
    .ConfigureApplication(builder.Configuration, builder.Configuration).
    ConfigureAPIApplication(builder.Configuration,builder.Configuration);


var app = builder.Build();

app.UseHttpsRedirection();
app.ConfigureRequestPath();
app.Run();
