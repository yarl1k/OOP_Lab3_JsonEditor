using System;
using System.Text.Json.Serialization;

namespace JsonEditor.Models
{
    public class SPF_Event
    {
        public int Event_id { get; set; }
        public string Event_name { get; set; }
        public string Department { get; set; }
        public string Event_date { get; set; }
        public string Event_time { get; set; }
        public string Event_location { get; set; }

        [JsonIgnore]
        public string FullDateTime => $"{Event_date} о {Event_time}";
    }
}