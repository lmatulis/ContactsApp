using ContactsApp.Client.Services.Interfaces;
using ContactsApp.Components;
using ContactsApp.Components.Account;
using ContactsApp.Data;
using ContactsApp.Services;
using ContactsApp.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

builder.Services.AddControllers();
builder.Services.AddOutputCache();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies(cookieOpts =>
    { // Make our API controllers return 401/403 instead of the HTML login page
        cookieOpts.ApplicationCookie!.Configure(cfg =>
        {
            cfg.Events.OnRedirectToLogin += (ctx) =>
            {
                if (ctx.Request.Path.StartsWithSegments("/api"))
                {
                    ctx.Response.StatusCode = 401;
                }

                return Task.CompletedTask;
            };

            cfg.Events.OnRedirectToAccessDenied += (ctx) =>
            {
                if (ctx.Request.Path.StartsWithSegments("/api"))
                {
                    ctx.Response.StatusCode = 403;
                }

                return Task.CompletedTask;
            };
        });
    });

var connectionString = DataUtility.GetConnectionString(builder.Configuration) ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddClaimsPrincipalFactory<CustomUserClaimsPrincipalFactory>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddHttpClient();


/*builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();*/

//builder.Services.AddSingleton<IEmailSender<ApplicationUser>, SendGridEmailService>();
//builder.Services.AddSingleton<IEmailSender, SendGridEmailService>();

//Tasker register
builder.Services.AddScoped<ITaskerItemService, TaskerItemService>();
builder.Services.AddScoped<ITaskerItemRepository, TaskerItemRepository>();

//Category register
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

//contacts register
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IContactService, ContactService>();

//Email Service config grab
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, GoogleEmailService>();
builder.Services.AddSingleton<IEmailSender, GoogleEmailService>();

var app = builder.Build();

var scope = app.Services.CreateScope();
await DataUtility.ManageDataAsync(scope.ServiceProvider);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ContactsApp.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.MapControllers();

app.Run();
