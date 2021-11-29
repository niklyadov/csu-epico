using Epico.Entity;
using Epico.Entity.DAL;
using Epico.Entity.DAL.Repository;
using Epico.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Epico
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region DbConnection

            var dbConnection = Configuration.GetConnectionString("DefaultConnection");
            var dbVersion = Configuration.GetValue<string>("ConnectionMysqlMariaDbVersion");
            services.AddDbContext<ApplicationContext>(options => 
                options.UseMySql(dbConnection, 
                    new MariaDbServerVersion(dbVersion)));

            #endregion
            
            services.AddScoped<FeatureRepository>();
            services.AddScoped<MetricRepository>();
            services.AddScoped<ProductRepository>();
            services.AddScoped<SprintRepository>();
            services.AddScoped<TaskRepository>();
            services.AddScoped<UserRepository>();
            
            #region Authentification

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = false;

                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

            }).AddEntityFrameworkStores<ApplicationContext>();

            services.AddScoped<AccountService>();
            
            #endregion
            
            services.AddControllersWithViews();

            services.AddScoped<ProductService>();
            services.AddScoped<MetricService>();
            services.AddScoped<UserService>();
            services.AddScoped<FeatureService>();
            services.AddScoped<TaskService>();
            services.AddScoped<SprintService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });
        }
    }
}