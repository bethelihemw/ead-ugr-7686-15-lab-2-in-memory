using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using PizzaStore.Models; 
[cite_start]class PizzaDb : DbContext
{
    public PizzaDb(DbContextOptions options) : base(options) { }
    [cite_start]
    public DbSet<Pizza> Pizzas { get; set; } = null!;
}

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PizzaStore API (In-Memory)",
        Description = "Making the Pizzas you love",
        Version = "v1"
    });
[cite_start]}); [cite: 64]

[cite_start]
builder.Services.AddDbContext<PizzaDb>(
    options => options.UseInMemoryDatabase("pizzas")
);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PizzaStore API V1");
    });
[cite_start]} [cite: 65, 66]
[cite_start]
app.MapPost("/pizza", async (PizzaDb db, Pizza pizza) =>
{
    await db.Pizzas.AddAsync(pizza);
    await db.SaveChangesAsync();
    return Results.Created($"/pizza/{pizza.Id}", pizza);
});

[cite_start]
app.MapGet("/pizzas", async (PizzaDb db) => await db.Pizzas.ToListAsync());

[cite_start]
app.MapGet("/pizza/{id}", async (PizzaDb db, int id) =>
    await db.Pizzas.FindAsync(id) is Pizza pizza ? Results.Ok(pizza) : Results.NotFound());

[cite_start]
app.MapPut("/pizza/{id}", async (PizzaDb db, Pizza updatepizza, int id) =>
{
    var pizza = await db.Pizzas.FindAsync(id);

    if (pizza is null) return Results.NotFound();

    pizza.Name = updatepizza.Name;
    pizza.Description = updatepizza.Description;
    
    await db.SaveChangesAsync();
    return Results.NoContent();
});

[cite_start]
app.MapDelete("/pizza/{id}", async (PizzaDb db, int id) =>
{
    var pizza = await db.Pizzas.FindAsync(id);

    if (pizza is null)
    {
        return Results.NotFound();
    }

    db.Pizzas.Remove(pizza);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.MapGet("/", () => "Hello World!");
app.Run();