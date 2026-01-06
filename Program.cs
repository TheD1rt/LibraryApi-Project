using LibraryApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<LibraryContext>(opt => opt.UseSqlite("Data Source=library.db"));
builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LibraryContext>();
    db.Database.EnsureCreated(); 
}

app.UseDefaultFiles();
app.UseStaticFiles(); 
app.MapControllers();
app.Run();