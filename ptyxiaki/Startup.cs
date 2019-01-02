using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json.Linq;
using ptyxiaki.Authorization;
using ptyxiaki.Common;
using ptyxiaki.Data;

namespace ptyxiaki
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.Configure<CookiePolicyOptions>(options =>
      {
        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
        options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = SameSiteMode.None;
      });

      services.AddEntityFrameworkNpgsql()
        .AddDbContext<DepartmentContext>(options =>
          options.UseNpgsql(Configuration.GetConnectionString("DepartmentContext")))
        .BuildServiceProvider();

      services.AddAutoMapper();
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

      services.AddAuthentication(options =>
      {
        options.DefaultScheme = Globals.AppCookieScheme;
        options.DefaultSignInScheme = Globals.ExternalCookieScheme;
      })
      .AddCookie(Globals.AppCookieScheme)
      .AddCookie(Globals.ExternalCookieScheme)
      .AddOAuth(Globals.OAuthScheme, options =>
      {
        options.ClientId = Configuration["OAuth:ClientId"];
        options.ClientSecret = Configuration["OAuth:ClientSecret"];
        options.CallbackPath = new PathString("/callback");

        options.AuthorizationEndpoint = "https://login.it.teithe.gr/authorization";
        options.TokenEndpoint = "https://login.it.teithe.gr/token";
        options.UserInformationEndpoint = "https://api.it.teithe.gr/profile";

        options.Scope.Add("profile");

        options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
        options.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "givenName;lang-el");
        options.ClaimActions.MapJsonKey(ClaimTypes.Surname, "sn;lang-el");
        options.ClaimActions.MapJsonKey(ClaimTypes.Email, "mail");
        options.ClaimActions.MapJsonKey(ClaimTypes.Role, "eduPersonAffiliation");
        options.ClaimActions.MapJsonKey(Claims.RegistrationNumber, "am");
        options.ClaimActions.MapJsonKey(Claims.Phone, "telephoneNumber");

        options.Events = new OAuthEvents
        {
          OnCreatingTicket = async context =>
          {
            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

            request.Headers.Add("x-access-token", context.AccessToken);

            var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
            response.EnsureSuccessStatusCode();

            var user = JObject.Parse(await response.Content.ReadAsStringAsync());

            context.RunClaimActions(user);
            var db = context.HttpContext.RequestServices.GetRequiredService<DepartmentContext>();
            var prof = await db.professors.FindAsync(2);

            context.Identity.AddClaim(new Claim("", ""));
          },
          OnRemoteFailure = context =>
          {
            context.HandleResponse();
            context.Response.Redirect("/Home/Error?message=" + context.Failure.Message);
            return Task.FromResult(0);
          }
        };
      });

      services.AddAuthorization(options =>
      {
        options.AddPolicy(Globals.UserPolicy, policy => policy.RequireClaim(ClaimTypes.Role, Globals.StudentRole, Globals.ProfessorRole));
        options.AddPolicy(Globals.StudentPolicy, policy => policy.RequireClaim(ClaimTypes.Role, Globals.StudentRole));
        options.AddPolicy(Globals.ProfessorPolicy, policy => policy.RequireClaim(ClaimTypes.Role, Globals.ProfessorRole));
        //options.AddPolicy("Administrator", policy => policy.RequireClaim("", "").RequireClaim("", ""));
      });

      services.AddSingleton<IAuthorizationHandler, IsOwnerAuthorizationHander>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseCookiePolicy();

      app.UseAuthentication();

      app.UseMvc();
    }
  }
}
