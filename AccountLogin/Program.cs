using JwtTokenExample.DBContext;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7233") });

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("UserIdentityConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//                     .AddCookie(option =>
//                     {
//                       option.LoginPath = new PathString("/Account/SignIn");
//                     });


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = "275545007442-91j8alp2eleai9sfoi7v2vl5gnui9o7o.apps.googleusercontent.com";
    options.ClientSecret = "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";   
});

builder.Services.AddAuthentication().AddFacebook(options =>
{
    options.AppId = "401313952358484";
    options.AppSecret = "c845aa880403fd54cff3f637089313a9";
});

builder.Services.AddAuthentication().AddMicrosoftAccount(options =>
{
    options.ClientId = "3f9d7851-46c6-45a3-aa5e-46b565c8e6b7";
    options.ClientSecret = "IVr8Q~YAI0eOglDm24QmiNDN_.nuRot8kJHL0bNX";
});


builder.Services.AddHttpClient();
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


app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
