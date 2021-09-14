using System;
using HotChocolate;
using HotChocolate.Language;
using HotChocolate.Types;

namespace LuminaAPI.GraphQL.Types
{
    public class SignedByteType : IntegerTypeBase< sbyte >
    {
        public SignedByteType( NameString name, BindingBehavior bind = BindingBehavior.Explicit ) : base( name, sbyte.MinValue, SByte.MaxValue, bind )
        {
        }
        
        // fuck
        
        protected override bool IsInstanceOfType(sbyte runtimeValue) => runtimeValue >= MinValue && runtimeValue <= MaxValue;

        protected override bool IsInstanceOfType(IntValueNode valueSyntax) => valueSyntax.ToUInt32() >= MinValue;

        protected override sbyte ParseLiteral( IntValueNode valueSyntax )
        {
            throw new System.NotImplementedException();
        }

        protected override IntValueNode ParseValue( sbyte runtimeValue )
        {
            throw new System.NotImplementedException();
        }
    }
}