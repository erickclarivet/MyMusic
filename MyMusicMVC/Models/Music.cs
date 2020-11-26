using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMusicMVC.Models
{
    public class Music
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int IdArtist { get; set; }
        public Artist Artist { get; set; }
    }
}
