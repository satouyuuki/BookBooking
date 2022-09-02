using BookBooking.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = new PathString("/Account/Login");
        options.Cookie.Name = "BookBookingAppCooki";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = new PathString("/Account/Denied");
    });

var serverVersion = new MySqlServerVersion(new Version(8, 0, 27));
builder.Services.AddDbContext<BookContext>(opt =>
    opt.UseMySql(configuration.GetConnectionString("DefaultConnection"), serverVersion)
    );

builder.Services.AddMvc(options =>
{
    options.EnableEndpointRouting = false;
});

//builder.Services.AddSwaggerGen();

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller=Books}/{action=Index}/{id?}");
app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Books}/{action=Index}/{id?}");

app.Run();

