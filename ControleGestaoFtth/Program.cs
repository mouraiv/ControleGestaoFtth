using ControleGestaoFtth.ComponentModel;
using ControleGestaoFtth.Context;
using ControleGestaoFtth.Repository;
using ControleGestaoFtth.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddSessionStateTempDataProvider();

builder.Services.Configure<CookieTempDataProviderOptions>(options => {
    options.Cookie.IsEssential = true;
});

builder.Services.AddSession();

string conn = builder.Configuration.GetConnectionString("AppDbContext");

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseMySql(conn, new MySqlServerVersion(new Version(8, 0, 31))));

builder.Services.AddScoped<IConstrutoraRepository, ConstrutoraRepository>();
builder.Services.AddScoped<IEstacoeRepository, EstacoeRepository>();
builder.Services.AddScoped<ITipoObraRepository, TipoObraRepository>();
builder.Services.AddScoped<INetwinRepository, NetwinRepository>();
builder.Services.AddScoped<IEstadoCampoRepository, EstadoCampoRepository>();
builder.Services.AddScoped<ITesteOpticoRepository, TesteOpticoRepository>();
builder.Services.AddScoped<IEnderecoTotaisRepository, EnderecoTotaisRepository>();
builder.Services.AddSingleton<ConversionViewModel>();
builder.Services.AddSingleton<ProgressBar>();
builder.Services.AddSingleton<ArquivoModel>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "Upload")),
   
    RequestPath = "/Upload"
});

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
