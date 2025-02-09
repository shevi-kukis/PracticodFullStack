using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);

// הוספת שירות CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


// var connectionString = Environment.GetEnvironmentVariable("ToDoDB");

// Console.WriteLine($"🔍 Connection String: {connectionString}");
builder.Services.AddDbContext<ToDoDbContext>(options =>
options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"),
new MySqlServerVersion(new Version(8, 0, 0))));

// builder.Services.AddDbContext<ToDoDbContext>(options =>
//     options.UseMySql(
//         connectionString,
//         ServerVersion.AutoDetect(connectionString),
//         mysqlOptions => mysqlOptions.EnableRetryOnFailure()
//     ));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// הוספת CORS לפני כל הקריאות ב-API
app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo API V1");
    c.RoutePrefix = "swagger"; // ניגשים דרך /swagger
});


app.MapGet("/items", async (ToDoDbContext db) =>
    await db.Items.ToListAsync());

app.MapPost("/items", async (ToDoDbContext db, Item newItem) =>
{
    db.Items.Add(newItem);
    await db.SaveChangesAsync();
    return Results.Created($"/items/{newItem.Id}", newItem);
});

app.MapPut("/items/{id}", async (ToDoDbContext db, int id, Item updatedItem) =>
{
    var item = await db.Items.FindAsync(id);
    if (item == null) return Results.NotFound();

    item.Name = updatedItem.Name;
    item.isComplete = updatedItem.isComplete;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/items/{id}", async (ToDoDbContext db, int id) =>
{
    var item = await db.Items.FindAsync(id);
    if (item == null) return Results.NotFound();

    db.Items.Remove(item);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapGet("/", () => "hello world");

app.Run();
