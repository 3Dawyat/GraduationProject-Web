using BL.BesnesLogic.IServices;
using UoN.ExpressiveAnnotations.NetCore.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
ConnectionString Connection = new ConnectionString(connectionString);

builder.Services.AddDbContext<PharmacyContext>(options =>
    options.UseSqlServer(connectionString),ServiceLifetime.Transient);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
}).AddEntityFrameworkStores<PharmacyContext>(); //required to permissions 

builder.Services.ConfigureApplicationCookie(option =>
{
    option.Cookie.Name = "Identity";
    option.Cookie.HttpOnly = true;
    option.ExpireTimeSpan = TimeSpan.FromDays(1);
    option.LoginPath = "/Authorization/Sign";
    option.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    option.SlidingExpiration = true;
});
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
builder.Services.Configure<AuthorizationOptions>(option =>
option.AddPolicy("DoctorsOnly", policy =>
{
    policy.RequireAuthenticatedUser();
    policy.RequireRole(AppRoles.Doctor);
}));
builder.Services.AddHangfire(x => x.UseSqlServerStorage(connectionString));
builder.Services.AddHangfireServer();

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();
builder.Services.AddScoped<IUserServices, ClsUserServices>();
builder.Services.AddScoped(typeof(IService<>), typeof(ClsService<>));
//builder.Services.AddDistributedMemoryCache();
builder.Services.AddExpressiveAnnotations();
//builder.Services.AddSession();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
//app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/BackgroundTasks",
    new DashboardOptions
    {
        DashboardTitle = "Saidaly Tech Background Tasks",
         //IsReadOnlyFunc = (DashboardContext context) => true,
        Authorization = new IDashboardAuthorizationFilter[]
        {
            new HangfireAuthorizationFilter("DoctorsOnly")
        }
    });


app.MapControllerRoute(
    name: "default",
    // pattern: "{controller=Account}/{action=Index}/{id?}");
    pattern: "{controller=Home}/{action=Dashboard}/{id?}");
//app.MapRazorPages();

app.Run();
