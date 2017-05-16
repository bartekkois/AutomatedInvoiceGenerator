using Newtonsoft.Json.Converters;

namespace AutomatedInvoiceGenerator.Models
{
    class JsonDateConverter : IsoDateTimeConverter
    {
        public JsonDateConverter()
        {
            DateTimeFormat = "yyyy-MM-dd";
        }
    }
}
