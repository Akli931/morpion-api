using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MorpionApi.Data;
using MorpionApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<GameStore>();

// Identity
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("MorpionDb"));

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapIdentityApi<IdentityUser>();

app.UseAuthorization();

app.MapControllers();
app.Run();