using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Crud.Data;
using Crud.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IGenericRepos, GenericRepos>();
builder.Services.AddDbContext<CrudContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CrudContext") ?? throw new InvalidOperationException("Connection string 'CrudContext' not found.")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("CROSPolicy",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});




// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();



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

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Information}/{action=Index}/{id?}");

app.Run();
