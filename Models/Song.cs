using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Player.Models
{
    public class Song
    {
        public int SongId { get; set; }
        public String Title { get; set; }
        public String Artist { get; set; }
        public String Genre { get; set; }
        public String PathFileSong { get; set; }
        public IList<SongPlaylist> SongPlaylist { get; set; }
    }

}
