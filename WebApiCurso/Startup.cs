using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;
using WebApiCurso.Servicios;

namespace WebApiCurso
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().AddJsonOptions(x =>
                                                   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
                                                    .AddNewtonsoftJson();
   
                                                    

            services.AddDbContext<ApplicationDbContext>(options =>
                                                        options.UseSqlServer(Configuration
                                                        .GetConnectionString("defaultConnection")));

            // configurar interfaz como servicio o clase coomo servicio directamente
            //                    Interfaz      clase
            //services.AddTransient<IServicio, ServicioA>(); 
            //services.AddTransient<ServicioA>(); 

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opciones => opciones.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration["llaveJwt"])),
                    ClockSkew = TimeSpan.Zero
                });

            // addtransient : transitorio.siempre se da una nueva instancia . (Funciones simples sin mantener data, no utiliza un estado)
            // addscoped : el tiempo de vida aumenta. dentro del mismo contexto tiene la misma instancia (por ej appdbcontext. )
            // addsingleton: siempre se tiene la misma instancia, no importa el usuario q la llame.

            services.AddEndpointsApiExplorer();

            // CONFIGURACION DE SWAGGER PARA Q USE JWT
            services.AddSwaggerGen(
                c => {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web api autores", Version = "v1" });
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference=new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            new string[]{}
                        }
                    });
                });


            // CONFIGURAR AUTO MAPPER
            services.AddAutoMapper(typeof(Startup));


            // CONFIGURAR IDENTITY PARA CONTROL DE USUARIOS Y ROLES
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // autorizacion por claims

            services.AddAuthorization(opciones =>
            {
                opciones.AddPolicy("EsAdmin", politica => politica.RequireClaim("esAdmin"));
                // se pueden agregar varias politicas, roles.
                //opciones.AddPolicy("EsVendedor", politica => politica.RequireClaim("esVendedor"));

            });


            // el cors es solo para app web 

            services.AddCors(opciones =>
            {
                opciones.AddDefaultPolicy(buider =>
                {
                    // en with origins agregamos la ruta de la app web por ej. para permitir hacer llamadas 
                    // a los endpoints del web api
                    // borrar / final
                    buider.WithOrigins("https://www.apirequest.io").AllowAnyMethod().AllowAnyHeader();

                });
            });


            services.AddDataProtection();


            services.AddTransient<HashService>();

        }

        public void Configure(IApplicationBuilder app,IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }



    }


}
