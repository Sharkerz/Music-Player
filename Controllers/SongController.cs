using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Player.Models;
using Microsoft.Extensions.Logging;
using Player.Data;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Diagnostics;
using System.Security.Claims;

namespace Player.Controllers
{
    public class SongController : Controller
    {
        private readonly AuthDbContext _context;
        private readonly IWebHostEnvironment hostingEnvironment;

        public SongController(AuthDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            hostingEnvironment = environment;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Song song)
        {
            try
            {
                List<String> listExt = new List<String>() { "mp3", "wav", "m4a", "flac", "mp4", "wma", "aac" };
                var Files = Request.Form.Files;

                foreach (var file in Files)
                {
                    var songs = Path.Combine(hostingEnvironment.WebRootPath, "songs");
                    if (file.Length > 0)
                    {
                        string filename = Guid.NewGuid().ToString();
                        var old_filename = file.FileName.Split('.');
                        var extension = old_filename.Last().ToLower();

                        if (listExt.Contains(extension) == false)
                        {
                            ViewData["error_file_type"] = "You have send a non authorized type file";
                            return View();
                        }

                        song.PathFileSong = filename + "." + extension;
                        using (var fileStream = new FileStream(Path.Combine(songs, song.PathFileSong), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        _context.Add(song);
                        _context.SaveChanges();
                    }
                }

                return View();
            }
            catch
            {
                return View();
            }
         
        }

        public IActionResult Detail(int id)
        {
            var song = _context.Song.Find(id);

            ViewData["songTitle"] = song.Title;
            ViewData["songArtist"] = song.Artist;
            ViewData["songGenre"] = song.Genre;
            return View();
        }


        public IActionResult AddInPlaylist(int id)
        {
            var song = _context.Song.Find(id);
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Playlist> playlists = (from x in _context.Playlist where x.userID == currentUserID select x).ToList();

            ViewData["playlists"] = playlists;
            ViewData["songName"] = song.Title;
            ViewData["songId"] = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddInPlaylist(SongPlaylist songPlaylist)
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
            }
            catch
            {

            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Delete()
        {
            try
            {
                var songId = Request.Form["songId"];
                var song = _context.Song.Find(Int32.Parse(songId));
                _context.Song.Remove(song);
                _context.SaveChanges();
            }
            catch { }

            return RedirectToAction("Index", "Home");
        }
    }
}
