using Room8.API.ExceptionHandler;
using Room8.API.Extensions;
using Room8.Core.Implementations;
using Room8.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbServices(builder.Configuration);
builder.Services.AddServices(builder.Configuration);

builder.Services.AddTransient<SeedData>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();
//builder.Services.AddIdentityApiEndPoint<IdentityUser>().AddEntityFrameworkStores<AppDbContext>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var seedData = services.GetRequiredService<SeedData>();
    seedData.SeedAsync().Wait();
}


//automatically apply migration
Seed.EnsurePopulated(app);



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowAllOrigins");
}

app.UseMiddleware<GlobalErrorHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chat");


app.Run();