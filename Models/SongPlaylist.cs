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
    public class SongPlaylist
    {
        public int SongId { get; set; }
        public Song Song { get; set; }

        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }
    }

}
