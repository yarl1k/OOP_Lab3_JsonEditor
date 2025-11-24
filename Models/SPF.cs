using System.Collections.Generic;

namespace JsonEditor.Models
{
    public class SPF_Data
    {
        public List<SPF_Event> Events { get; set; } = new List<SPF_Event>();
        public string Faculty { get; set; } = "ФІТ";
    }
}