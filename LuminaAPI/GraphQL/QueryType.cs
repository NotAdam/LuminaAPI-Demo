using HotChocolate.Types;

namespace LuminaAPI.GraphQL
{
    public class QueryType : ObjectType< Query >
    {
        protected override void Configure( IObjectTypeDescriptor< Query > descriptor )
        {
            // descriptor.Directive( new LanguageDirective() );
        }
    }
}