using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace ETA.InvoiceServices.Services
{
    public class JsonServices
    {
        /// <summary>
        /// This Function for convert object to JSON string,
        /// Support Unicode and 5 Digits Decimal
        /// </summary>
        /// <param name="obj">Object to be Converted</param>
        public static string GetJson(object obj)
        {
            var serializeOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All, UnicodeRanges.All),
                Converters = { new DecimalFormatConverter() }
            };

            return JsonSerializer.Serialize(obj, serializeOptions);
        }

        /// <summary>
        /// This Function for serialize object using ETA Serialization Algorithm to be Signed,
        /// Support ISO DateTime and 5 Digits Decimal
        /// </summary>
        /// <param name="obj">Object to be serialized</param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            string serializedString = "";

            foreach (PropertyInfo pi in obj.GetType().GetProperties())
            {

                serializedString += string.Format("\"{0}\"", pi.Name.ToUpper());

                if (pi.PropertyType.IsPrimitive || pi.PropertyType.Equals(typeof(string)) || pi.PropertyType.Equals(typeof(DateTime)))
                {
                    if (pi.PropertyType.Equals(typeof(double)))
                        serializedString += string.Format("\"{0:0.00000}\"", pi.GetValue(obj));
                    else if (pi.PropertyType.Equals(typeof(DateTime)))
                        serializedString += string.Format("\"{0}\"", ((DateTime)pi.GetValue(obj)).ToUniversalTime()
                         .ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"));
                    else
                        serializedString += string.Format("\"{0}\"", pi.GetValue(obj));
                }
                else if (pi.PropertyType.IsArray)
                {
                    foreach (object item in (Array)pi.GetValue(obj))
                    {
                        serializedString += string.Format("\"{0}\"", pi.Name.ToUpper());
                        serializedString += Serialize(item);
                    }
                }
                else if (pi.PropertyType.IsClass)
                {
                    serializedString += Serialize(pi.GetValue(obj));
                }
            }

            return serializedString;
        }

        /// <summary>
        /// This Asynchronous Function for convert object to JSON string,
        /// Support Unicode and 5 Digits Decimal
        /// </summary>
        /// <param name="obj">Object to be Converted</param>
        public async static Task<string> GetJsonAsync(object obj)
        {
            return await Task.Run(() =>
            {
                var serializeOptions = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All, UnicodeRanges.All),
                    Converters = { new DecimalFormatConverter() }
                };

                return JsonSerializer.Serialize(obj, serializeOptions);
            });
        }

        /// <summary>
        /// This Asynchronous Function for serialize object using ETA Serialization Algorithm to be Signed,
        /// Support ISO DateTime and 5 Digits Decimal
        /// </summary>
        /// <param name="obj">Object to be serialized</param>
        /// <returns></returns>
        public async static Task<string> SerializeAsync(object obj)
        {
            return await Task.Run(async () =>
            {
                string serializedString = "";

                foreach (PropertyInfo pi in obj.GetType().GetProperties())
                {

                    serializedString += string.Format("\"{0}\"", pi.Name.ToUpper());

                    if (pi.PropertyType.IsPrimitive || pi.PropertyType.Equals(typeof(string)) || pi.PropertyType.Equals(typeof(DateTime)))
                    {
                        if (pi.PropertyType.Equals(typeof(double)))
                            serializedString += string.Format("\"{0:0.00000}\"", pi.GetValue(obj));
                        else if (pi.PropertyType.Equals(typeof(DateTime)))
                            serializedString += string.Format("\"{0}\"", ((DateTime)pi.GetValue(obj)).ToUniversalTime()
                             .ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"));
                        else
                            serializedString += string.Format("\"{0}\"", pi.GetValue(obj));
                    }
                    else if (pi.PropertyType.IsArray)
                    {
                        foreach (object item in (Array)pi.GetValue(obj))
                        {
                            serializedString += string.Format("\"{0}\"", pi.Name.ToUpper());
                            serializedString += await SerializeAsync(item);
                        }
                    }
                    else if (pi.PropertyType.IsClass)
                    {
                        serializedString += await SerializeAsync(pi.GetValue(obj));
                    }
                }

                return serializedString;
            });
        }
    }
}
