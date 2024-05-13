using System.Text.Json.Serialization;

namespace ReadyTechCoffee.Business.BusinessObjects
{
    public class CoffeeItem
    {
        public string Message { get; set; }

        public DateTime Prepared { get; set; }

        [JsonIgnore]
        public int RepeatOrderNumber { get; set; }

        [JsonIgnore]
        public CoffeeStatus CoffeeStatus { get; set; }
    }
}
