using Aplicacion;
using Arquitectura;
using Azure.Core;
using Azure.Identity;
using CourierMaxApi.Properties;

var builder = WebApplication.CreateBuilder(args);

var keyVaultUri = builder.Configuration["KeyVault:Uri"];
if (!string.IsNullOrWhiteSpace(keyVaultUri))
{
    var tenantId = builder.Configuration["KeyVault:TenantId"];
    var clientId = builder.Configuration["KeyVault:ClientId"];
    var clientSecret = builder.Configuration["KeyVault:ClientSecret"];

    TokenCredential credential =
        !string.IsNullOrWhiteSpace(tenantId)
        && !string.IsNullOrWhiteSpace(clientId)
        && !string.IsNullOrWhiteSpace(clientSecret)
            ? new ClientSecretCredential(tenantId, clientId, clientSecret)
            : new DefaultAzureCredential();

    builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUri), credential);
}

builder.Services.AddPresentation(builder.Configuration)
                    .AddArquitectura(builder.Configuration)
                    .AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler("/error");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
