using auth_titan_backend;
using auth_titan_backend.Data;
using auth_titan_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

//AddServices
var provider = builder.Services.BuildServiceProvider();
var configuration = provider.GetRequiredService<IConfiguration>();
builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlite(
		configuration.GetConnectionString("DbConnection")));
builder.Services.AddIdentity<AppUser, IdentityRole>(config =>
{
	config.Password.RequiredLength = 4;
	config.Password.RequireDigit = false;
	config.Password.RequireUppercase = false;
	config.Password.RequireNonAlphanumeric = false;
})
	.AddEntityFrameworkStores<AuthDbContext>()
	.AddDefaultTokenProviders();

builder.Services.AddIdentityServer()
	.AddAspNetIdentity<AppUser>()
	.AddInMemoryApiResources(Configuration.ApiResources)
	.AddInMemoryIdentityResources(Configuration.IdentityResources)
	.AddInMemoryApiScopes(Configuration.ApiScopes)
	.AddInMemoryClients(Configuration.Clients)
	.AddDeveloperSigningCredential();

builder.Services.ConfigureApplicationCookie(config =>
{
	config.Cookie.Name = "Titan.Identity.Cookie";
	config.LoginPath = "/Auth/Login";
	config.LogoutPath = "/Auth/Logout";
});

builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();
var app = builder.Build();

//UseServices

app.UseSwagger();
app.UseSwaggerUI();


using(var scope = app.Services.CreateScope())
{ 
	var serviceProvider = scope.ServiceProvider;
	try
	{
		var context = serviceProvider.GetRequiredService<AuthDbContext>();
		DbInitializer.Initialize(context);
	}
	catch (Exception exeption)
	{ 
		var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
		logger.LogError(exeption, "An error ocurred while app initialization");
	}
}
app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(
		Path.Combine(builder.Environment.ContentRootPath, "Styles")), //<== Maybe Error
	RequestPath = "/Styles"
});
app.UseRouting();
app.UseIdentityServer();
app.UseEndpoints(endpoints =>
{
	endpoints.MapDefaultControllerRoute();
});
app.Run();
