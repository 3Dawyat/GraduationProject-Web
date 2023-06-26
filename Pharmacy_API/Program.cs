using BL.BesnesLogic.IServices;
using Swashbuckle.AspNetCore.SwaggerUI;
using UoN.ExpressiveAnnotations.NetCore.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
ConnectionString Connection = new ConnectionString(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddSwaggerGen(options =>
{ 
    options.EnableAnnotations();
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Saidaly Tech API",
        Version = "v1",
        Description = "An API for Saidaly Tech Mopile App",
    });
 
});
builder.Services.AddCors();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddDbContext<PharmacyContext>(options =>
             options.UseSqlServer(ConnectionString.GetConnectionString()));
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 1;
    options.User.RequireUniqueEmail = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
    options.User.AllowedUserNameCharacters = null;
}).AddEntityFrameworkStores<PharmacyContext>().AddDefaultTokenProviders();
builder.Services.Configure<JWTModel>(builder.Configuration.GetSection("JWT"));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddHangfire(x => x.UseSqlServerStorage(ConnectionString.GetConnectionString()));
builder.Services.AddHangfireServer();
builder.Services.Configure<AuthorizationOptions>(option =>
option.AddPolicy("DoctorsOnly", policy =>
{
    policy.RequireAuthenticatedUser();
    policy.RequireRole(AppRoles.Doctor);
}));
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<IUserServices, ClsUserServices>();
builder.Services.AddScoped<IService<TbSalesInvoiceItems>, ClsService<TbSalesInvoiceItems>>();
builder.Services.AddScoped<IService<TbOrders>, ClsService<TbOrders>>();
builder.Services.AddScoped<IService<TbCustomers>, ClsService<TbCustomers>>();
builder.Services.AddScoped<IService<TbCategories>, ClsService<TbCategories>>();
builder.Services.AddScoped<IService<VwItemsWithUnits>, ClsService<VwItemsWithUnits>>();
builder.Services.AddScoped<IService<VwOrderHead>, ClsService<VwOrderHead>>();
builder.Services.AddScoped<IService<VwOrderItem>, ClsService<VwOrderItem>>();

builder.Services.AddExpressiveAnnotations();

var app = builder.Build();
app.UseSwagger();
if (app.Environment.IsDevelopment())
    app.UseSwaggerUI();
else
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors(a => a.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/BackgroundTasks",
    new DashboardOptions
    {
        DashboardTitle = "Saidaly Tech Background Tasks",
        IsReadOnlyFunc = (DashboardContext context) => false,
        //Authorization = new IDashboardAuthorizationFilter[]
        //{
        //    new HangfireAuthorizationFilter("DoctorsOnly")
        //}
    });
app.MapControllers();
app.Run();
