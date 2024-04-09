using AdvisorManagement.Model;
using AdvisorManager.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("AdvisorsList"));

var app = builder.Build();


app.MapGet("/advisors", async (AppDbContext db) =>
    await db.Advisors.ToListAsync());


app.MapGet("/advisors/{id}", async (int id, AppDbContext db) =>
    await db.Advisors.FindAsync(id)
        is Advisor advisor
            ? Results.Ok(advisor)
            : Results.NotFound());

app.MapPost("/advisors", async (Advisor advisor, AppDbContext db) =>
{
    db.Advisors.Add(advisor);
    await db.SaveChangesAsync();

    return Results.Created($"/advisors/{advisor.Id}", advisor);
});

app.MapPut("/advisors/{id}", async (int id, Advisor inputadvisor, AppDbContext db) =>
{
    var advisor = await db.Advisors.FindAsync(id);

    if (advisor is null) return Results.NotFound();

    advisor.Name = inputadvisor.Name;
    advisor.Address = inputadvisor.Address;
    advisor.PhoneNumber= inputadvisor.PhoneNumber;
    advisor.HealthStatus = AdvisorService.GetColor();

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/advisors/{id}", async (int id, AppDbContext db) =>
{
    if (await db.Advisors.FindAsync(id) is Advisor advisor)
    {
        db.Advisors.Remove(advisor);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});
app.Run();
