using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using System.Text.Json;
using ProductApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IConnectionFactory>(new ConnectionFactory() { HostName = "localhost" });
builder.Services.AddSingleton<RabbitMQ.Client.IConnection>(sp => sp.GetRequiredService<IConnectionFactory>().CreateConnection());
builder.Services.AddSingleton<RabbitMQ.Client.IModel>(sp => sp.GetRequiredService<RabbitMQ.Client.IConnection>().CreateModel());
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.SetIsOriginAllowed(origin => true)
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});
var app = builder.Build();

app.UseRouting();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.UseHttpsRedirection();

app.MapControllers();
app.MapHub<NotificationHub>("/notifications");
app.Run();
