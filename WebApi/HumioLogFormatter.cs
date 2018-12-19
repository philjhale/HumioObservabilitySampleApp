using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Serilog.Events;
using Serilog.Formatting;

namespace ObservabilitySampleApp.WebApi
{
    /// <summary>
    ///Add #type:json to all logs so Humio parses the logs as JSON
    /// 
    /// https://docs.humio.com/parsers/
    ///
    /// "A client sending data to Humio must specify which repository to store the data in and which parser to use for ingesting the data.
    /// You do this either by setting the special #type field to the name of the parser to use or by assigning a specific parser to the Ingest API Token used to authenticate the client"
    /// </summary>
    public class HumioLogFormatter : ITextFormatter
    {
        private readonly JsonSerializerSettings _jsonSettings;

        public HumioLogFormatter()
        {
            _jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Include,
                Formatting = Formatting.None,
            };
            
            this._jsonSettings.Converters.Add(new StringEnumConverter());
        }
        
        public void Format(LogEvent logEvent, TextWriter output)
        {
            var json = JObject.FromObject(logEvent, JsonSerializer.Create(_jsonSettings));
            json.Add("#type", "json");
            output.WriteLine(json.ToString(Formatting.None));
        }
    }
}