using Microsoft.EntityFrameworkCore;
using UsersAPI;
using UsersAPI.Model;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<UserDb>(opt => opt.UseInMemoryDatabase("UserList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/users", async (UserDb db) => await db.Users.ToListAsync());

app.MapGet("/users/active", async (UserDb db) => await db.Users.Where(t => t.IsActive).ToListAsync());

app.MapGet("/users/{id}", async (int id, UserDb db) =>
    await db.Users.FindAsync(id)
        is User user
            ? Results.Ok(user)
            : Results.NotFound());

app.MapPost("/users", async (User user, UserDb db) =>
{
    db.Users.Add(user);
    await db.SaveChangesAsync();

    return Results.Created($"/users/{user.Id}", user);
});

app.MapPut("/users/{id}", async (int id, User inputUser, UserDb db) =>
{
    var user = await db.Users.FindAsync(id);

    if (user is null) return Results.NotFound();

    user.Name = inputUser.Name;
    user.IsActive = inputUser.IsActive;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/users/{id}", async (int id, UserDb db) =>
{
    if (await db.Users.FindAsync(id) is User user)
    {
        db.Users.Remove(user);
        await db.SaveChangesAsync();
        return Results.Ok(user);
    }

    return Results.NotFound();
});

app.Run();