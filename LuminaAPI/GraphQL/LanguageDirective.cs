using System.Threading.Tasks;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Lumina.Data;

namespace LuminaAPI.GraphQL
{
    public class LanguageDirectiveType : DirectiveType< LanguageDirective >
    {
        protected override void Configure( IDirectiveTypeDescriptor< LanguageDirective > descriptor )
        {
            descriptor.Name( "language" );
            descriptor.Location( DirectiveLocation.Object );
            descriptor.Location( DirectiveLocation.Query );
            descriptor.Argument( x => x.Name );

            descriptor.Use< LanguageDirectiveMiddleware >();
        }
    }

    public class LanguageDirective
    {
        public Language Name { get; set; } = Language.English;
    }

    public class LanguageDirectiveMiddleware
    {
        private readonly FieldDelegate _next;

        public LanguageDirectiveMiddleware( FieldDelegate next )
        {
            _next = next;
        }

        public async ValueTask InvokeAsync( IDirectiveContext context )
        {
            // context.Result = context.ArgumentValue< Language >( nameof( LanguageDirective.Name ) );
            context.ContextData[ nameof( LanguageDirective.Name ) ] = context.ArgumentValue< Language >( nameof( LanguageDirective.Name ) );

            await _next( context );
        }
    }
}