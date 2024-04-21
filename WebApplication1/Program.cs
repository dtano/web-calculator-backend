using WebApplication1.Repositories;
using WebApplication1.Services;

//var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
//{
//    builder.Services.AddCors(options =>
//    {
//        options.AddPolicy(name: MyAllowSpecificOrigins,
//                          policy =>
//                          {
//                              policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
//                          });
//    });
//    builder.Services.AddControllers();
//    builder.Services.AddScoped<IUserService, UserService>();
//}

//var app = builder.Build();
//{
//    app.UseHttpsRedirection();
//    //app.UseAuthorization();
//    app.UseCors(MyAllowSpecificOrigins);
//    app.MapControllers();
//    app.Run();
//}

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                                 .AllowAnyMethod()
                                 .AllowAnyHeader();
                      });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

//app.UseRouting();

app.UseCors(MyAllowSpecificOrigins); // Placed after UseRouting and before UseAuthorization

//app.UseAuthorization();

app.MapControllers();

app.Run();
