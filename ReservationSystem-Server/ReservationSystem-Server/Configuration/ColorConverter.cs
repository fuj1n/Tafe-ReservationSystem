using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReservationSystem_Server.Configuration;

public class ColorConverter : JsonConverter<Color>
{
    public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        byte a = 255, r = 255, g = 0, b = 255;

        while (reader.Read())
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.EndObject:
                    return Color.FromArgb(a, r, g, b);
                case JsonTokenType.PropertyName:
                {
                    string? propertyName = reader.GetString()?.ToUpper();
                    reader.Read();
                    switch (propertyName)
                    {
                        case "A":
                            a = reader.GetByte();
                            break;
                        case "R":
                            r = reader.GetByte();
                            break;
                        case "G":
                            g = reader.GetByte();
                            break;
                        case "B":
                            b = reader.GetByte();
                            break;
                    }

                    break;
                }
            }
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        
        writer.WritePropertyName(options.PropertyNamingPolicy?.ConvertName(nameof(Color.A)) ?? nameof(Color.A));
        writer.WriteNumberValue(value.A);
        
        writer.WritePropertyName(options.PropertyNamingPolicy?.ConvertName(nameof(Color.R)) ?? nameof(Color.R));
        writer.WriteNumberValue(value.R);
        
        writer.WritePropertyName(options.PropertyNamingPolicy?.ConvertName(nameof(Color.G)) ?? nameof(Color.G));
        writer.WriteNumberValue(value.G);
        
        writer.WritePropertyName(options.PropertyNamingPolicy?.ConvertName(nameof(Color.B)) ?? nameof(Color.B));
        writer.WriteNumberValue(value.B);
        
        writer.WriteEndObject();
    }
}