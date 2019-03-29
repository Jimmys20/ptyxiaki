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
      services.AddMvc()
        .AddRazorPagesOptions(options =>
        {
          options.Conventions.AuthorizeFolder("/Administration", Globals.ADMINISTRATOR_POLICY);
        })
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

      services.AddAuthentication(options =>
      {
        options.DefaultScheme = Globals.APP_COOKIE_SCHEME;
        options.DefaultSignInScheme = Globals.EXTERNAL_COOKIE_SCHEME;
      })
      .AddCookie(Globals.APP_COOKIE_SCHEME)
      .AddCookie(Globals.EXTERNAL_COOKIE_SCHEME)
      .AddOAuth(Globals.O_AUTH_SCHEME, options =>
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
        options.ClaimActions.MapJsonKey(Claims.REGISTRATION_NUMBER, "am");
        options.ClaimActions.MapJsonKey(Claims.PHONE, "telephoneNumber");

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
            //var db = context.HttpContext.RequestServices.GetRequiredService<DepartmentContext>();
            //var prof = await db.professors.FindAsync(2);

            //context.Identity.AddClaim(new Claim("", ""));
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
        options.AddPolicy(Globals.USER_POLICY, policy => policy.RequireRole(Globals.STUDENT_ROLE, Globals.PROFESSOR_ROLE));
        options.AddPolicy(Globals.STUDENT_POLICY, policy => policy.RequireRole(Globals.STUDENT_ROLE));
        options.AddPolicy(Globals.PROFESSOR_POLICY, policy => policy.RequireRole(Globals.PROFESSOR_ROLE));
        options.AddPolicy(Globals.ADMINISTRATOR_POLICY, policy => policy.RequireRole(Globals.ADMINISTRATOR_ROLE));
      });
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
