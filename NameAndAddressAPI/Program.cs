using NameAndAddressAPI.Services;
using NameAndAddressAPI.Services.DataManagment;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Could replace with an IOC using the services directory
builder.Services.AddSingleton<IAddressManager, AddressManager>();
builder.Services.AddSingleton<IDocumentStoreHolder, DocumentStoreHolder>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("docker"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
    Console.WriteLine("Swagger enabled: URL if running in docker localhost:8000/swagger/index.html");
} 

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
