using System;
using System.Diagnostics;
using HotChocolate.Types;
using HotChocolate.Types.Pagination;
using Lumina.Text;
using LuminaAPI.Data;
using LuminaAPI.GraphQL;
using LuminaAPI.GraphQL.Types;
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
                .AddControllers();

            var luminaInstance = new Lumina.GameData( Configuration.GetValue< string >( "DataPath" ) );

            Service< Lumina.GameData >.Set( luminaInstance );
            Service< SheetTypeCache >.Set( new SheetTypeCache( luminaInstance ) );

            services.AddSingleton( luminaInstance );

            services
                .AddGraphQLServer()
                .BindRuntimeType< UInt32, UnsignedIntType >()
                .BindRuntimeType< UInt64, UnsignedLongType >()
                .BindRuntimeType< UInt64, UnsignedLongType >()
                .BindRuntimeType< SeString, StringType >()
                .AddTypeConverter< SeString, string >( x =>
                {
                    return x.RawString;
                } )
                // .AddTypeConverter< ILazyRow, object >( x =>
                // {
                //     return x.RawRow;
                // } )
                .SetPagingOptions( new PagingOptions
                {
                    DefaultPageSize = 100,
                    MaxPageSize = 250,
                    IncludeTotalCount = true
                } )
#if DEBUG
                .OnSchemaError( ( err, ex ) => { Debugger.Break(); } )
#endif
                // .AddDirectiveType< LanguageDirectiveType >()
                .BindRuntimeType< UInt16, UnsignedShortType >()
                // todo: fix this shit because it's not actually valid
                .BindRuntimeType< sbyte, IntType >()
                .AddType< SheetsQueryType >()
                .AddQueryType< QueryType >();
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

            app.UseEndpoints( endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGraphQL();
            } );
        }
    }
}