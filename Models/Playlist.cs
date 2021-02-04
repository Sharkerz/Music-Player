using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using Player.Areas.Identity.Data;

namespace Player.Models
{
    public class Playlist
    {
        public int PlaylistId { get; set; }
        public String Name { get; set; }
        public String userID { get; set; }
        public IList<SongPlaylist> SongPlaylist { get; set; }
    }

}
