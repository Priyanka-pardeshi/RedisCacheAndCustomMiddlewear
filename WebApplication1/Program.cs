using Microsoft.Extensions.Configuration;
using WebApplication1.Middlewear;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = Configuration.GetSection("RedisConnection").GetValue<string>("ResdisConnectionString");
    options.InstanceName = Configuration.GetSection("RedisConnection").GetValue<string>("InstanceName");
   
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())   
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
//app.UseMiddleware<TimingMiddlewear>();
app.UseTiming();
///Added Custom middlewear
//app.Use(async (ctx, next) => {
//    var start = DateTime.UtcNow;
//    await next.Invoke(ctx);
//    app.Logger.LogInformation($"Request {ctx.Request.Path}:{(DateTime.UtcNow-start).TotalMilliseconds}ms");

//}); 


app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
