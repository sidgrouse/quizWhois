using JavaScriptEngineSwitcher.ChakraCore;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using QuizWhois.Api.Hubs;
using QuizWhois.Common;
using QuizWhois.Domain.Database;
using QuizWhois.Domain.Services.Implementations;
using QuizWhois.Domain.Services.Interfaces;
using React.AspNet;

namespace QuizWhois.Api
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
            services.AddCors();

            services.AddSignalR();
            services.AddMemoryCache();
            services.AddHttpContextAccessor();
            services.AddReact();
            services.AddJsEngineSwitcher(options => options.DefaultEngineName = ChakraCoreJsEngine.EngineName).AddChakraCore();

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(CustomExceptionFilter));
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "QuizWhois.Api", Version = "v1" });
            });

            string dbConnectionString = Configuration.GetConnectionString("QuizWhoisDb");
            services.AddDbContext<ApplicationContext>(options =>
                options.UseMySql(
                dbConnectionString,
                new MySqlServerVersion(new System.Version(8, 0, 27))));

            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IUserAnswerService, UserAnswerService>();
            services.AddScoped<IQuestionRatingService, QuestionRatingService>();
            services.AddScoped<IPackService, PackService>();
            services.AddScoped<CustomExceptionFilter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "QuizWhois.Api v1"));

                app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseReact(config => { });
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<QuestionHub>("/hub");
            });
        }
    }
}
