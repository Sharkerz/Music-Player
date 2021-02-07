using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Player.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Player.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Data.AuthDbContext _context;

        public HomeController(ILogger<HomeController> logger, Data.AuthDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            List<Song> songs = _context.Song.ToList();
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Playlist> playlists = (from x in _context.Playlist where x.userID == currentUserID select x).ToList();

            ViewData["playlists"] = playlists;
            ViewData["songs"] = songs;
            return View();
        }

        [Authorize]
        public IActionResult AddSong()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddSongPlaylist(SongPlaylist songPlaylist)
        {
            try
            {
                //SongPlaylist songPlaylist = new SongPlaylist();

                var selected_playlist = Request.Form["selected_playlist"];
                Playlist playlist = _context.Playlist.Find(Int32.Parse(selected_playlist));

                var selected_song = Request.Form["songId"];
                Song song = _context.Song.Find(Int32.Parse(selected_song));

                songPlaylist.PlaylistId = playlist.PlaylistId;
                songPlaylist.SongId = song.SongId;

                _context.Add(songPlaylist);
                _context.SaveChanges();
            } catch {

            }

            Index();
            return View("Index");
        }
    }
}
