using Microsoft.Extensions.Options;

using MiWebApp.Interfaces;
using MiWebApp.Repositorios;
using MiWebApp.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSession(options=>{
    options.IdleTimeout=TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly=true;
    options.Cookie.IsEssential=true;

});
//que hace esto?
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserRepository, UsuarioRepository>();
builder.Services.AddScoped<IAutentificarService,AutentificarService >();
builder.Services.AddScoped<IProductoRepository, ProductoRepositorio>();
builder.Services.AddScoped<IPresupuestoRepository,PresupuestosRepository>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseSession();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();



