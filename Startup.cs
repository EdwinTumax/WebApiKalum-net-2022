using Microsoft.EntityFrameworkCore;
using WebApiKalum.Services;
using WebApiKalum.Utilities;

namespace WebApiKalum
{
    public class Startup
    {
        private readonly string OriginKalum = "OriginKalum";
        public IConfiguration Configuration {get;}
        public Startup(IConfiguration _Configuration)
        {
            this.Configuration = _Configuration;
        }
        public void ConfigureServices(IServiceCollection _services)
        {
            _services.AddCors(options => {
                options.AddPolicy(name: OriginKalum, builder => {
                    builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
                });
            });
            _services.AddTransient<IUtilsService, UtilsService>();
            _services.AddTransient<ActionFilter>();
            _services.AddControllers(options => options.Filters.Add(typeof(ErrorFilterException)));
            _services.AddAutoMapper(typeof(Startup));
            _services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = 
                Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            _services.AddDbContext<KalumDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));
            _services.AddEndpointsApiExplorer();
            _services.AddSwaggerGen();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors(OriginKalum);
            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}