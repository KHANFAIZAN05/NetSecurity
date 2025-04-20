using CustomAuth.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("MyCookieAuth")
             .AddCookie("MyCookieAuth", options =>
             {
                 options.Cookie.Name = "MyAuthCookie";
                 options.LoginPath = "/api/auth/unauthorized";
                 options.AccessDeniedPath = "/api/auth/denied"; 
                 options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                 options.SlidingExpiration = true;
             });

builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.AddSingleton<UserStore>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
