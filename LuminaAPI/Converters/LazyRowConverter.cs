using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Lumina.Excel;

namespace LuminaAPI.Converters
{
    public class LazyRowConverter : JsonConverter< ILazyRow >
    {
        public override bool CanConvert( Type type )
        {
            return typeof( ILazyRow ).IsAssignableFrom( type );
        }

        public override ILazyRow Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options ) => null;

        public override void Write(
            Utf8JsonWriter writer,
            ILazyRow lazyRow,
            JsonSerializerOptions options )
        {
            JsonSerializer.Serialize( writer, (object)lazyRow.RawRow );
        }
    }
}