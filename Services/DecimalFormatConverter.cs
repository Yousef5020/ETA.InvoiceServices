using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ETA.InvoiceServices.Services
{
    public class DecimalFormatConverter : JsonConverter<double>
    {
        public override double Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) =>
                double.Parse(reader.GetString(),
                    NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);

        public override void Write(
            Utf8JsonWriter writer,
            double value,
            JsonSerializerOptions options) =>
                writer.WriteNumberValue(0.00000m + (decimal)value);
    }
}
