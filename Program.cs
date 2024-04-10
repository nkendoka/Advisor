using AdvisorManagement.Infrastructure.Persistence;
using AdvisorManagement.Model;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("AdvisorsList"));

builder.Services.AddSingleton<MRUCache<int, Advisor>>(new MRUCache<int, Advisor>(capacity: 5));

builder.Services.AddScoped<IAdvisorRepository, AdvisorRepository>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapPost("/api/advisors", (Advisor advisor, IAdvisorRepository repo) =>
{
    repo.AddAsync(advisor);
    return Results.Created($"/advisors/{advisor.Id}", advisor);
});

//app.MapPut("/advisors/{id}", async (int id, Advisor inputadvisor, AppDbContext db) =>
//{
//    var advisor = await db.Advisors.FindAsync(id);

//    if (advisor is null) return Results.NotFound();

//    advisor.Name = inputadvisor.Name;
//    advisor.Address = inputadvisor.Address;
//    advisor.PhoneNumber= inputadvisor.PhoneNumber;

//    await db.SaveChangesAsync();

//    return Results.NoContent();
//});

//app.MapDelete("/advisors/{id}", async (int id, AppDbContext db) =>
//{
//    if (await db.Advisors.FindAsync(id) is Advisor advisor)
//    {
//        db.Advisors.Remove(advisor);
//        await db.SaveChangesAsync();
//        return Results.NoContent();
//    }

//    return Results.NotFound();
//});

app.UseHttpsRedirection();
app.MapControllers();

app.UseStaticFiles(); 
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapFallbackToFile("index.html");
});

app.Run();
