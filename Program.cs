using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using tcc_mypet_back.Data.Context;
using tcc_mypet_back.Data.Interfaces;
using tcc_mypet_back.Data.Repositories;
using tcc_mypet_back.Data.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Base")));

builder.Services.AddControllers((options => {
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    options.EnableEndpointRouting = false;
})).AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMvcCore().AddRazorViewEngine();

#region HangFire

builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("Hangfire")));
builder.Services.AddHangfireServer();

#endregion

#region Cors

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());
});

#endregion
#region Interfaces e serviços
builder.Services.AddScoped<IAdministratorRepository, AdministratorRepository>();
builder.Services.AddScoped<ISizeRepository, SizeRepository>();

#endregion

#region Swagger

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My Pet TCC", Version = "v1" });
    c.CustomSchemaIds(x => x.FullName);
});

builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 7257;
});

#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.Urls.Clear();
    app.Urls.Add("https://localhost:7257");
    app.Urls.Add("http://*:5031");
    app.Urls.Add("https://*:3434");
}

if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();   
}

app.UseCors("CorsPolicy");
app.UseRouting();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tcc My Pet v1");
    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    c.ShowExtensions();
    c.EnableFilter();
    c.EnableDeepLinking();
    c.EnableValidator();
});

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseHangfireServer();
app.UseHangfireDashboard();

app.Run();
