using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Lumina.Text;

namespace LuminaAPI
{
    public class SeStringConverter : JsonConverter< SeString >
    {
        public override SeString Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options ) => null;

        public override void Write(
            Utf8JsonWriter writer,
            SeString seString,
            JsonSerializerOptions options ) =>
            writer.WriteStringValue( seString.RawString );
    }
}