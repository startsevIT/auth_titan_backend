using auth_titan_backend;
using auth_titan_backend.Data;
using auth_titan_backend.Models;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.Globalization;

public class Startup
{
	public IConfiguration AppConfiguration { get; }
	public Startup(IConfiguration configuration) => 
		AppConfiguration = configuration;
	public void ConfigureServices(IServiceCollection services)
	{
		var connectingString = AppConfiguration.GetValue<string>("DbConnection");
		services.AddDbContext<AuthDbContext>(options =>
		{
			options.UseSqlite(connectingString);
		});

		services.AddIdentity<AppUser,IdentityRole>(config =>
		{
			config.Password.RequiredLength = 4;
			config.Password.RequireDigit= false;
			config.Password.RequireNonAlphanumeric= false;
			config.Password.RequireUppercase= false;
		})
			.AddEntityFrameworkStores<AuthDbContext>()
			.AddDefaultTokenProviders();

		services.AddIdentityServer()
			.AddAspNetIdentity<AppUser>()
			.AddInMemoryApiResources(Configuration.ApiResources)
			.AddInMemoryIdentityResources(Configuration.IdentityResources)
			.AddInMemoryApiScopes(Configuration.ApiScopes)
			.AddInMemoryClients(Configuration.Clients)
			.AddDeveloperSigningCredential();

		services.ConfigureApplicationCookie(config =>
		{
			config.Cookie.Name = "Titan.Identity.Coockie";
			config.LoginPath = "/Auth/Login";
			config.LogoutPath = "/Auth/Logout";
		});
		services.AddControllersWithViews();
	}
	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}
		app.UseStaticFiles(new StaticFileOptions
		{
			FileProvider = new PhysicalFileProvider(
				Path.Combine(env.ContentRootPath, "Styles")),
			RequestPath = "/Styles"
		});
		app.UseRouting();
		app.UseIdentityServer();

		app.UseEndpoints(endpoints =>
		{
			endpoints.MapDefaultControllerRoute();
		});
	}

}
public class Program
{
	private static void Main(string[] args)
	{
		var host = CreateHostBuilder(args).Build();
		using (var scope = host.Services.CreateScope())
		{
			var serviceProvider = scope.ServiceProvider;
			try
			{
				var context = serviceProvider.GetRequiredService<AuthDbContext>();
				DbInitializer.Initialize(context);
			}
			catch (Exception exception)
			{
				var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
				logger.LogError(exception, "An error occurred while app initialization");
			}
		}
		host.Run();
	}

	public static IHostBuilder CreateHostBuilder(string[] args) =>
		Host.CreateDefaultBuilder(args)
		.ConfigureWebHostDefaults(webBuilder =>
		{
			webBuilder.UseStartup<Startup>();
		});
}