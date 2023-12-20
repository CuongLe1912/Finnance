using Finnance.CuongLe.Middlewares;
using Finnance.CuongLe.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;
using URF.Core.EF.Trackable.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<IISServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
    options.MaxRequestBodySize = long.MaxValue;
});
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
    options.Limits.MaxRequestBodySize = long.MaxValue;
    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(30);
});
builder.Services
    .AddSignalR(options =>
    {
        options.EnableDetailedErrors = true;
        options.MaximumReceiveMessageSize = 102400000;
    })
    .AddNewtonsoftJsonProtocol(options =>
    {
        options.PayloadSerializerSettings.ContractResolver = new DefaultContractResolver();
        options.PayloadSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });

builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.Configure<FormOptions>(o =>  // currently all set to max, configure it to your needs!
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = long.MaxValue; // <-- !!! long.MaxValue
    o.MultipartHeadersCountLimit = int.MaxValue;
    o.MultipartHeadersLengthLimit = int.MaxValue;
    o.MultipartBoundaryLengthLimit = int.MaxValue;
});

// appSettings
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
var appSettings = appSettingsSection.Get<AppSettings>();
builder.Services.Configure<AppSettings>(appSettingsSection);

//// identity
//builder.Services.AddIdentity<User, Role>(o =>
//{
//    o.Password.RequiredLength = 8;
//    o.User.RequireUniqueEmail = true;
//    o.Lockout.AllowedForNewUsers = true;
//    o.Lockout.MaxFailedAccessAttempts = 3;
//    o.Password.RequireNonAlphanumeric = false;
//    o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
//})
//.AddEntityFrameworkStores<MeeyAdminContext>()
//.AddDefaultTokenProviders();

// jwt authentication
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("xxx"));
builder.Services.AddAuthentication()
    .AddCookie(c => c.SlidingExpiration = true)
    .AddJwtBearer(c =>
    {
        c.SaveToken = true;
        c.RequireHttpsMetadata = false;
        c.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = key,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = Constant.Issuer,
            ValidateIssuerSigningKey = true,
            ValidAudience = Constant.Audience,
        };
    });

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
{
    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
})
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });
builder.Services.Configure<IISServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
    options.MaxRequestBodySize = long.MaxValue;
});

// swagger
string _apiVersion = "v1";

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(_apiVersion, new OpenApiInfo
    {
        Version = _apiVersion,
        Title = "Finnance API",
        Description = "Finnance",
        Contact = new OpenApiContact
        {
            Email = string.Empty,
            Name = "Finnance",
            Url = new Uri("https://finnance.com/"),
        },
        License = new OpenApiLicense
        {
            Name = "Finnance",
            Url = new Uri("https://finnance/page/intro"),
        }
    });
    options.DocInclusionPredicate((docName, description) => true);
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


CorsMiddlewareExtensions.UseCors(app, "CorsPolicy");
AuthAppBuilderExtensions.UseAuthentication(app);
//if (env.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//}
app.UseRouting();
app.UseStaticFiles();
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
//app.UseMiddleware<LoggerMiddleware>();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    //endpoints.MapHub<NotifyHub>("/notifyhub");
});
//app.UseStaticFiles(new StaticFileOptions()
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"resources")),
//    RequestPath = new PathString("/resources")
//});
//app.UseFileServer(new FileServerOptions
//{
//    FileProvider = new PhysicalFileProvider(
//       Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports")),
//    RequestPath = "/Reports",
//    EnableDefaultFiles = true
//});

app.Use(async (context, next) =>
{
    context.Features.Get<IHttpMaxRequestBodySizeFeature>().MaxRequestBodySize = null; // unlimited I guess
    await next.Invoke();
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Finnance API V1");
});

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

StaticFileExtensions.UseStaticFiles(app);
app.Run();
