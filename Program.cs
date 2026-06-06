using HrPortal.Api.Configuration;
using HrPortal.Api.Infrastructure;
using HrPortal.Api.Repositories;
using HrPortal.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection(DatabaseSettings.SectionName));
builder.Services.Configure<CorsSettings>(builder.Configuration.GetSection(CorsSettings.SectionName));

var databaseSettings = builder.Configuration.GetSection(DatabaseSettings.SectionName).Get<DatabaseSettings>() ?? new DatabaseSettings();
var corsSettings = builder.Configuration.GetSection(CorsSettings.SectionName).Get<CorsSettings>() ?? new CorsSettings();

builder.Services.AddSingleton(databaseSettings);
builder.Services.AddSingleton(corsSettings);

builder.Services.AddScoped<IRequestRepository>(_ => databaseSettings.Provider switch
{
    DatabaseProvider.Hana => new HanaRequestRepository(databaseSettings.HanaConnectionString, databaseSettings.HanaProviderInvariantName),
    _ => new SqlServerRequestRepository(databaseSettings.SqlServerConnectionString)
});

builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<ICurrentEmployeeContext, CurrentEmployeeContext>();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        if (corsSettings.AllowedOrigins.Length == 0)
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            return;
        }

        policy.WithOrigins(corsSettings.AllowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddProblemDetails();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors("FrontendPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
