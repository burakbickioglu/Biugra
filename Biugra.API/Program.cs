
using BaseProject.Domain.Models;
using BaseProject.Domain.Models.Context;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

//var key = builder.Configuration["JwtSettings:JwtSecret"];
// Add services to the container.
builder.Services.Configure<JwtSettings>(builder.Configuration);

var jwtSettings = builder.Configuration.GetSection(typeof(JwtSettings).Name)
                                             .Get<JwtSettings>();

builder.Services.AddSingleton(jwtSettings);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Biugra.Api"
    });
    // To Enable authorization using Swagger (JWT)  
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                   }
                });
});
var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:JwtSecret"]);
var issuer = builder.Configuration["JwtSettings:Issuer"];

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("https://app-Biugra.vercel.app", "http://localhost:3000", "https://localhost:7013")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod();
        });
});

builder.Services.AddAuthorization()
    .AddAuthentication(x =>
                        {
                            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        })
                        .AddJwtBearer("Bearer", x =>
                        {
                            x.RequireHttpsMetadata = false;
                            x.SaveToken = true;
                            x.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateAudience = true,
                                ValidAudience = Audiences.Public.ToString(),
                                ValidateIssuer = true,
                                ValidIssuer = issuer,
                                ValidateIssuerSigningKey = true,
                                ValidateLifetime = true,
                                IssuerSigningKey = new SymmetricSecurityKey(key),
                            };
                        })

                        .AddCookie();

//builder.Services.AddScoped<IAuthService, AuthService>();
//builder.Services.AddScoped<ITokenHandler, JwtHelper>();
builder.Services.AddDependencies(builder.Configuration);


var profiles = ProfileHelper.GetProfiles();
var configuration = new MapperConfiguration(opt => { opt.AddProfiles(profiles); });
builder.Services.AddSingleton(configuration.CreateMapper());


builder.Services.AddIdentity<AppUser, AppRole>(options =>
{


    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abc�defg�h�ijklmno�pqrs�tu�vwxyzABC�DEFG�HI�JKLMNO�PQRS�TU�VWXYZ0123456789-._@+";
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseStaticFiles();
app.UseMiddleware<ApiAuthMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
