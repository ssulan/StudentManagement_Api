using StudentInfoSystem.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//---------------Connection String-------------------
builder.Services.AddDbContext<AppDbContext>(options =>
{
    string? connectionString = builder.Configuration.GetConnectionString("Default");

    if (connectionString is null)
    {
        throw new InvalidOperationException("Connection string is not found");
    }

    options.UseSqlServer(connectionString);
});
//----------------------------------

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowAll"); // pipeline'da UseCors

//-------------------SCOPE----------
using (var scope = app.Services.CreateScope())
{
    using (var dbConext = scope.ServiceProvider.GetRequiredService<AppDbContext>())
    {
        await dbConext.Database.EnsureDeletedAsync();
        await dbConext.Database.EnsureCreatedAsync();

        await DbSeed.SeedAsync(dbConext);
    }
}


    app.Run();

