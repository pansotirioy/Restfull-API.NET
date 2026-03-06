using Assignment.Models;
using Assignment.Repository;
using Assignment.Service;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using System.Text.Json.Serialization;

namespace Assignment
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            Log.Information("Starting up the application");

            var builder = WebApplication.CreateBuilder(args);

            // Use Serilog ONLY
            builder.Logging.ClearProviders();
            builder.Host.UseSerilog();

            // Add services to the container.
            builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
               options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")
            ));

            //for singleton services to use db context
            builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
            );


            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("http://localhost:5174", "http://localhost:5173") // Frontend URL
                          .AllowAnyMethod()  // GET, POST, etc.
                          .AllowAnyHeader(); // Custom headers
                });
            });

            MapsterConfig.RegisterMappings();

            builder.Services.AddSingleton(TypeAdapterConfig.GlobalSettings);
            builder.Services.AddScoped<IMapper, Mapper>();
            builder.Services.AddScoped<ICandidatesRepository, CandidatesRepository>();
            builder.Services.AddScoped<IAddressRepository, AddressRepository>();
            builder.Services.AddScoped<ICertificatesRepository, CertificatesRepository>();
            builder.Services.AddScoped<IPhotoIdRepository, PhotoIdRepository>();
            builder.Services.AddScoped<ICandidatesAnalyticsRepository, CandidatesAnalyticsRepository>();
            builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
            builder.Services.AddScoped<ISaleCertificatesRepository, SaleCertificatesRepository>();
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddSingleton<IAiRoutingService, AiRoutingService>();

            builder.Services.AddMemoryCache();

            builder.Services.AddControllers(options =>
            {
                // Add global model validation filter
                options.Filters.Add<ValidateModelAttribute>();
            })
             .AddJsonOptions(options =>
             {
                 // Avoid circular reference errors when serializing
                 options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
             });

            builder.Services.AddIdentityCore<Candidate>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var secret = builder.Configuration["JwtConfig:Secret"];
                var issuer = builder.Configuration["JwtConfig:ValidIssuer"];
                var audience = builder.Configuration["JwtConfig:ValidAudiences"];

                if (secret is null || issuer is null || audience is null)
                {
                    throw new ApplicationException("Jwt is not set in the configuration");
                }
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidIssuer = issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
                };

            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.FullName);
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(AppPolicies.RequireAdministratorRole, policy =>
                policy.RequireRole(AppRoles.Administrator));
                options.AddPolicy(AppPolicies.RequireUserRole, policy =>
                policy.RequireRole(AppRoles.User));
            });


            var app = builder.Build();

            app.UseGlobalExceptionHandling();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            using (var serviceScope = app.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                // Ensure database is created and apply migrations
                var dbContext = services.GetRequiredService<ApplicationDbContext>();

                dbContext.Database.EnsureCreated();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                if (!await roleManager.RoleExistsAsync(AppRoles.User))
                {
                    await roleManager.CreateAsync(new IdentityRole(AppRoles.User));
                }
                if (!await roleManager.RoleExistsAsync(AppRoles.Administrator))
                {
                    await roleManager.CreateAsync(new IdentityRole(AppRoles.Administrator));
                }
            }

            app.MapControllers();

            try
            {
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.Information("Application shutting down");
                Log.CloseAndFlush();
            }

        }
    }
}
