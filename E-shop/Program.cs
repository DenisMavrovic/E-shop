using E_shop.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_shop
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			var services = builder.Services;
			var configuration = builder.Configuration;

			services.AddAuthentication().AddFacebook(facebookOptions =>
			{
				facebookOptions.AppId = configuration["Authentication:Facebook:AppId"];
				facebookOptions.AppSecret = configuration["Authentication:Facebook:AppSecret"];
			});

			services.AddAuthentication().AddGoogle(googleOptions =>
			{
				googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
				googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
			});

			services.AddAuthentication().AddTwitter(twitterOptions =>
			{
				twitterOptions.ConsumerKey = configuration["Authentication:Twitter:ConsumerAPIKey"];
				twitterOptions.ConsumerSecret = configuration["Authentication:Twitter:ConsumerSecret"];
			});

			// Add services to the container.
			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(connectionString));
			builder.Services.AddDatabaseDeveloperPageExceptionFilter();

			builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
				.AddEntityFrameworkStores<ApplicationDbContext>();
			builder.Services.AddControllersWithViews();

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
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");
			app.MapRazorPages();

			app.Run();
		}
	}
}