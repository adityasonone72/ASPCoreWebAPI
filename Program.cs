using ASPCoreWebAPI.Middleware;
using ASPCoreWebAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(); //registering the services required to generate Swagger Information (Before .Build)

//AddScoped will create one object per HTTP request, it doesn't create one object for all the requests. Suppose 5 people hit the URL seperately, then it will create 5 objects and treat them individually.
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // add swagger to HTTP request pipeline, that's why after .Build
    app.UseSwaggerUI(); // for browser UI
}
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
