using GenerativeAI.Microsoft;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using MovieGallery.Agents;
using MovieGallery.Data;
using MovieGallery.Services;
using MovieGallery.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<MovieGalleryDBContext>(option =>
    option.UseSqlServer(connectionString));

var apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
var model = "gemini-3.6-flash"; //gemini-3.8-flash or gemini-3.6-flash

builder.Services.AddChatClient(sp =>
    new GenerativeAIChatClient(apiKey, model));

builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient("MoviesApi", (sp, client) =>
{
    var httpRequest = sp.GetRequiredService<IHttpContextAccessor>()
                        .HttpContext!
                        .Request;

    client.BaseAddress = new Uri(UriHelper.BuildAbsolute(
        httpRequest.Scheme,
        httpRequest.Host,
        httpRequest.PathBase));
});

builder.Services.AddScoped<MovieGalleryHelper>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MovieGalleryDBContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IMovieDBService, MovieDBService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IReviewDBService, ReviewDBService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserDBService, UserDBService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Movies}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
