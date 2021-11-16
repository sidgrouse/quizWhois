using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Auth.OAuth2;
using JavaScriptEngineSwitcher.ChakraCore;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using QuizWhois.Api.Hubs;
using QuizWhois.Common;
using QuizWhois.Domain.Database;
using QuizWhois.Domain.Middleware;
using QuizWhois.Domain.Services.Implementations;
using QuizWhois.Domain.Services.Interfaces;
using React.AspNet;

namespace QuizWhois.Api
{
    public class Startup
    {
        private const string ClientSecretFilenameKey = "TEST_WEB_CLIENT_SECRET_FILENAME";

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

            var clientSecrets = GoogleClientSecrets.FromFile(Configuration[ClientSecretFilenameKey]).Secrets;           

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
            services.AddScoped<IQuizService, QuizService>();
            services.AddScoped<CustomExceptionFilter>();

            services
              .AddAuthentication(o =>
              {
                   // This forces challenge results to be handled by Google OpenID Handler, so there's no
                   // need to add an AccountController that emits challenges for Login.
                   o.DefaultChallengeScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;

                   // This forces forbid results to be handled by Google OpenID Handler, which checks if
                   // extra scopes are required and does automatic incremental auth.
                   o.DefaultForbidScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;

                   // Default scheme that will handle everything else.
                   // Once a user is authenticated, the OAuth2 token info is stored in cookies.
                   o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
              })
              .AddCookie()
              .AddGoogleOpenIdConnect(options =>
              {
                  options.ClientId = clientSecrets.ClientId;
                  options.ClientSecret = clientSecrets.ClientSecret;
              });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerAuthorized();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "QuizWhois.Api v1"));

                app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthentication();
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
