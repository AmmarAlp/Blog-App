
using System.Text;
using BlogAPI.Data;
using BlogAPI.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

public class Program
{
    public static void Main(string[] args)
    {
        BlogAPIContext _context;
        RoleManager<IdentityRole> _roleManager;
        UserManager<BlogUser> _userManager;
        IdentityRole identityRole;
        BlogUser blogUser;

        var builder = WebApplication.CreateBuilder(args);



        // Configure DbContext with Sql Server

        builder.Services.AddDbContext<BlogAPIContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("BlogAPIContext")
                    ?? throw new InvalidOperationException("Connection string 'BlogAPIContext' not found.")));

        //Configure Identity
        builder.Services.AddIdentity<BlogUser, IdentityRole>()
            .AddEntityFrameworkStores<BlogAPIContext>()
            .AddDefaultTokenProviders();

        //Configure JWT Authentication
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                ValidAudience = builder.Configuration["JWT:ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
            };


        });

        builder.Services.AddAuthorization();

        builder.Services.AddControllers();
        // Configure Swagger/OpenAPI
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"

            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                new string[] { }
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

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        //Admin User Config.
        IServiceProvider serviceProvider = app.Services.CreateScope().ServiceProvider;
        _context = serviceProvider.GetRequiredService<BlogAPIContext>();
        _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        _userManager = serviceProvider.GetRequiredService<UserManager<BlogUser>>();



        if (_roleManager.FindByNameAsync("Admin").Result == null)
        {
            identityRole = new IdentityRole("Admin");
            _roleManager.CreateAsync(identityRole).Wait();
        }
        if (_userManager.FindByNameAsync("Admin").Result == null)
        {
            blogUser = new BlogUser();
            blogUser.UserName = "Admin";
            _userManager.CreateAsync(blogUser, "Admin123!").Wait();
            _userManager.AddToRoleAsync(blogUser, "Admin").Wait();
        }

        app.Run();
    }
}