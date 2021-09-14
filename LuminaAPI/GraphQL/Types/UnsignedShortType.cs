using HotChocolate;
using HotChocolate.Language;
using HotChocolate.Types;

namespace LuminaAPI.GraphQL.Types
{
    public class UnsignedShortType : IntegerTypeBase< ushort >
    {
        public UnsignedShortType() : base( "UnsignedShortType", ushort.MinValue, ushort.MaxValue, BindingBehavior.Explicit )
        {
        }

        protected override bool IsInstanceOfType( ushort runtimeValue ) => runtimeValue >= MinValue;

        protected override bool IsInstanceOfType( IntValueNode valueSyntax ) => valueSyntax.ToUInt16() >= MinValue;

        protected override ushort ParseLiteral( IntValueNode valueSyntax )
        {
            return valueSyntax.ToUInt16();
        }

        protected override IntValueNode ParseValue( ushort runtimeValue )
        {
            return new(runtimeValue);
        }
    }
}