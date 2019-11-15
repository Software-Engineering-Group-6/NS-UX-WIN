using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSpotify
{
    public class Track
    {
        public List<String> artists { get; set; }
        public String name { get; set; }
        public String duration { get; set; }
        public String popularity { get; set; }
        public String preview_url { get; set; }
        public String external_url { get; set; }

    }
}
