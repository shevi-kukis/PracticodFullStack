// using Microsoft.EntityFrameworkCore;
// using TodoApi;

// var builder = WebApplication.CreateBuilder(args);

// // הוספת שירות CORS
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAll", policy =>
//     {
//         policy.AllowAnyOrigin()
//               .AllowAnyMethod()
//               .AllowAnyHeader();
//     });
// });

// // קבלת ה-Connection String ממשתנה סביבה בלבד
// var connectionString = Environment.GetEnvironmentVariable("ToDoDB");

// if (string.IsNullOrEmpty(connectionString))
// {
//     throw new Exception("Missing database connection string in environment variables");
// }

// builder.Services.AddDbContext<ToDoDbContext>(options =>
//     options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// var app = builder.Build();

// // הוספת CORS לפני כל הקריאות ב-API
// app.UseCors("AllowAll");

// app.UseSwagger();
// app.UseSwaggerUI(c =>
// {
//     c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo API V1");
//     c.RoutePrefix = "swagger"; // ניגשים דרך /swagger
// });


// app.MapGet("/items", async (ToDoDbContext db) =>
//     await db.Items.ToListAsync());

// app.MapPost("/items", async (ToDoDbContext db, Item newItem) =>
// {
//     db.Items.Add(newItem);
//     await db.SaveChangesAsync();
//     return Results.Created($"/items/{newItem.Id}", newItem);
// });

// app.MapPut("/items/{id}", async (ToDoDbContext db, int id, Item updatedItem) =>
// {
//     var item = await db.Items.FindAsync(id);
//     if (item == null) return Results.NotFound();

//     item.Name = updatedItem.Name;
//     item.IsComplete = updatedItem.IsComplete;

//     await db.SaveChangesAsync();
//     return Results.NoContent();
// });

// app.MapDelete("/items/{id}", async (ToDoDbContext db, int id) =>
// {
//     var item = await db.Items.FindAsync(id);
//     if (item == null) return Results.NotFound();

//     db.Items.Remove(item);
//     await db.SaveChangesAsync();
//     return Results.NoContent();
// });

// app.MapGet("/", () => "hello world");

// app.Run();
using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);


///swagger ////////////
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Injecting the DbContext into services
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql("Server=biuywqdsqp7r0cnswezm-mysql;Database=ToDoDB;User=uxgzm0r1st7zgwdo;Password=IWJAebiO2m4Nv0wq8Ecp;",
    Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.41-mysql")));

//// cors /////////////
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin() 
                   .AllowAnyMethod() 
                   .AllowAnyHeader(); 
        });
});




var app = builder.Build();

app.UseCors("AllowAllOrigins"); 


if (app.Environment.IsDevelopment())
{
    app . UseSwagger ();
    app . UseSwaggerUI ( c  =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

///controller ////////
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
    item.IsComplete = updatedItem.IsComplete;

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
