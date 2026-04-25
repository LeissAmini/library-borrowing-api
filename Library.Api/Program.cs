using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Library.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<Library.Api.Repositories.IBorrowRepository, Library.Api.Repositories.BorrowRepository>();
builder.Services.AddScoped<Library.Api.Services.IBorrowService, Library.Api.Services.BorrowService>();

var app = builder.Build();

// TEMP DIAGNOSTIC
// TEMP DIAGNOSTIC + SEED
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Library.Api.Data.ApplicationDbContext>();
    Console.WriteLine($"[DIAG] DB: {db.Database.GetConnectionString()}");
    Console.WriteLine($"[DIAG] Members: {db.Members.Count()}, Books: {db.Books.Count()}");

    if (!db.Members.Any())
    {
        db.Members.Add(new Library.Api.Models.Member
        {
            Id = Guid.Parse("5ec6b71f-ac62-4ce4-b957-1854211812d3"),
            Name = "Alice",
            Email = "alice@example.com",
            IsActive = true
        });
    }

    if (!db.Books.Any())
    {
        db.Books.Add(new Library.Api.Models.Book
        {
            Id = Guid.Parse("61105c46-e3ea-4826-a406-112d2a379e24"),
            Title = "Clean Code",
            AvailableCopies = 5
        });
    }

    db.SaveChanges();
    Console.WriteLine($"[DIAG] After seed — Members: {db.Members.Count()}, Books: {db.Books.Count()}");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(errApp => errApp.Run(async context =>
{
    var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;

    var (status, detail) = ex switch
    {
        KeyNotFoundException => (StatusCodes.Status404NotFound, ex.Message),
        InvalidOperationException => (StatusCodes.Status409Conflict, ex.Message),
        DbUpdateConcurrencyException => (StatusCodes.Status409Conflict, "This book is currently unavailable."),
        _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
    };

    context.Response.StatusCode = status;
    context.Response.ContentType = "application/problem+json";
    await context.Response.WriteAsJsonAsync(new { status, detail });
}));

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

public partial class Program { }