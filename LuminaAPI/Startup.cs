using System.Text.Json.Serialization;
using LuminaAPI.Converters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LuminaAPI
{
    public class Startup
    {
        public Startup( IConfiguration configuration )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services )
        {
            services
                .AddControllers()
                .AddJsonOptions(
                opt =>
                {
                    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                    opt.JsonSerializerOptions.IncludeFields = true;
                    opt.JsonSerializerOptions.Converters.Add( new SeStringConverter() );
                    opt.JsonSerializerOptions.PropertyNamingPolicy = null;
                    // opt.JsonSerializerOptions.Converters.Add( new LazyRowConverter() );
                } );
            services.AddSingleton( new Lumina.GameData( Configuration.GetValue<string>( "DataPath" ) ) );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IWebHostEnvironment env )
        {
            if( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints( endpoints => { endpoints.MapControllers(); } );
        }
    }
}
