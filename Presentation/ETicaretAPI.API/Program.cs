using ETicaretAPI.Application.Validators.Products;
using ETicaretAPI.Infrastructure;
using ETicaretAPI.Infrastructure.Enums;
using ETicaretAPI.Infrastructure.Filters;
using ETicaretAPI.Infrastructure.Services.Storage.Azure;
using ETicaretAPI.Infrastructure.Services.Storage.Local;
using ETicaretAPI.Persistence;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();

builder.Services.AddStorage<AzureStorage>();
//builder.Services.AddStorage<LocalStorage>();
//builder.Services.AddStorage(StorageType.Local);

//Cors
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
{
    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200", "https://localhost:4200");
}));

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>(); //EF Core Validasyonlarýný devre dýþý býrakýp kendi filterýmýzý ekliyoruz
})
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>()) //Fluent Validationlarý ekler
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true); //EF Core Validasyonlarýný devre dýþý býrakýr

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();
