var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "recipeDelete",
    pattern: "{controller=Home}/{action=Delete}/{id}");

app.MapControllerRoute(
    name: "recipeEdit",
    pattern: "{controller=Admin}/{action=Edit}/{id?}");

app.MapControllerRoute(
    name: "UserEdit",
    pattern: "{controller=Admin}/{action=EditUser}/{id?}");

app.Run();
