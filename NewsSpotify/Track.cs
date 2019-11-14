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

        //    public Track(String[] _artists, String _name, String _duration, String _popularity, String _preview_url, String _external_url)
        //    {
        //        artists = new List<String>(_artists);
        //        name = _name;
        //        duration = _duration;
        //        popularity = _popularity;
        //        preview_url = _preview_url;
        //        external_url = _external_url;
        //    }
        //}
    }
}
