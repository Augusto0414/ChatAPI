using ChatAPI.Data;
using ChatAPI.Interfaces;
using ChatAPI.Repositorio;
using ChatAPI.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//configurar autenticacion JWT
// Inyección de dependencias para MongoDB y repositorios
builder.Services.AddSingleton<MongoContext>();
builder.Services.AddScoped<IMessagee, MessageRepository>();
builder.Services.AddScoped<IUsuario, UserReposotory>();
builder.Services.AddScoped<IAuth, AuthService>();

// Inyección de servicios adicionales
builder.Services.AddScoped<WebSocketService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});



builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseWebSockets();


app.Use(async (context, next) =>
{
    // Detecta si es una solicitud WebSocket
    if (context.WebSockets.IsWebSocketRequest)
    {
        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        var socketService = context.RequestServices.GetRequiredService<WebSocketService>();

        // Lógica para gestionar la conexión del cliente WebSocket
        await socketService.ConnectUser(context, webSocket);
    }
    else
    {
        await next();
    }
});


app.MapControllers();

app.Run();
