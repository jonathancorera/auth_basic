using Microsoft.EntityFrameworkCore;
using DataAccess.Data;
using AuthTest.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using DataAccess.Entities;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddIdentity<ApplicationUser, ApplicationUserRole>(options =>
{

})
    .AddRoles<ApplicationUserRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<IAuthService, AuthService>();

builder.Services.AddScoped<ICoffeeShopService, CoffeeShopService>();
builder.Services.AddScoped<DbContext,ApplicationDbContext>();



builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience= true,
        ValidateIssuer= true,
        RequireExpirationTime= true,
        ValidateIssuerSigningKey= true,
        ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
        ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value))
};
})  ;



builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{

    //seed roles
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationUserRole>>();

    var roles = new[]
    {
        "Admin",
        "User"
    };

    foreach(var role in roles)
    {
        if(! await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new ApplicationUserRole(role));
        }
    }


    //seed admin
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    if(await userManager.FindByEmailAsync("admin@mexxar.com") == null)
    {
        var user = new ApplicationUser();
        user.Email = "admin@mexxar.com";
        user.UserName = "admin@mexxar.com";
        user.FirstName = "Mexxar";
        user.LastName = "Admin";

        await userManager.CreateAsync(user, "Thot4hm!");
        await userManager.AddToRoleAsync(user, "Admin");
    }


    //enable automatic db updates on startup
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
