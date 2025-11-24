using System.Collections.Generic;
using System.Linq;
using JsonEditor.Models;

namespace JsonEditor.Utilities
{
    public static class LINQ_Search
    {
        public static List<SPF_Event> do_LINQ_search(List<SPF_Event> source, string text, string criterion)
        {
            if (source == null) return new List<SPF_Event>();

            if (string.IsNullOrWhiteSpace(text)) return source;

            text = text.ToLower();
            var query = source.AsEnumerable(); 

            switch (criterion)
            {
                case "Назва":
                    query = query.Where(x => x.Event_name != null && x.Event_name.ToLower().Contains(text));
                    break;
                case "Департамент":
                    query = query.Where(x => x.Department != null && x.Department.ToLower().Contains(text));
                    break;
                case "Локація":
                    query = query.Where(x => x.Event_location != null && x.Event_location.ToLower().Contains(text));
                    break;
                default:
                    query = query.Where(x => (x.Event_name != null && x.Event_name.ToLower().Contains(text)) ||
                                             (x.Department != null && x.Department.ToLower().Contains(text)) ||
                                             (x.Event_location != null && x.Event_location.ToLower().Contains(text)));
                    break;
            }

            return query.ToList();
        }
    }
}