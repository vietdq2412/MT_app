using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using MT_app.business.Services;
using MT_app.Infrastructure.Data;
using MT_app.Infrastructure.Data.AuthenModels;
using MT_app.Infrastructure.Repository;
using MT_Project.Data;
using System.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using MT_app.core.Models;

namespace MT_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorPages();

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                                   throw new InvalidOperationException(
                                       "Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
                {
                    options.LoginPath = "/Identity/LoginRegister"; // Specify the login page
                    options.AccessDeniedPath = "/Identity/AccessDenied"; // Specify the access denied page
                });

            builder.Services.AddControllersWithViews();

            builder.Services.Configure<EmailSenderOptions>(builder.Configuration.GetSection("EmailSenderOptions"));
            builder.Services.AddScoped<IEmailSender, GmailSender>();

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();

            builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
            builder.Services.AddScoped<ISupplierService, SupplierService>();

            builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();

            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            
            builder.Services.AddScoped<IIdentityService, IdentityService>();

            builder.Services.AddSession();
            builder.Services.AddDistributedMemoryCache();

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
                pattern: "{controller=Products}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}