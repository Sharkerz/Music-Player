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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Player.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly AuthDbContext _context;

        public PlaylistController(AuthDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<Playlist> playlists = (from x in _context.Playlist where x.userID == currentUserID select x).ToList();
            ViewData["playlists"] = playlists;
            return View();
        }
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            var playlist = _context.Playlist.Find(id);

            ViewData["playlistId"] = playlist.PlaylistId;
            ViewData["playlistName"] = playlist.Name;
            return View(playlist);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Playlist playlist)
        {
            try
            {
                ClaimsPrincipal currentUser = this.User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                var renamedPlaylist = _context.Playlist.Find(playlist.PlaylistId);
                renamedPlaylist.Name = Request.Form["playlistName"];

                if (renamedPlaylist.userID == currentUserID)
                {
                    _context.Entry(renamedPlaylist).State = EntityState.Modified;
                    _context.SaveChanges();
                }
            } catch
            {

            }

            Index();
            return View("Index");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Playlist playlist)
        {

            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            playlist.userID = currentUserID;
            _context.Add(playlist);
            _context.SaveChanges();

            Index();
            return View("Index");
        }

        [Authorize]
        public ViewResult Detail(int id)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            Playlist playlist = _context.Playlist.Find(id);

            if(playlist != null)
            {
                if (playlist.userID == currentUserID)
                {
                    List<SongPlaylist> songplaylist = (from x in _context.SongPlaylist where x.PlaylistId == id select x).Include(q => q.Song).ToList();

                    ViewData["songs"] = songplaylist;
                    ViewData["playlistName"] = playlist.Name;
                    ViewData["playlistId"] = playlist.PlaylistId;
                    return View(playlist);
                }
                else
                {
                    Index();
                    return View("Index");
                }
            }
            else
            {
                Index();
                return View("Index");
            }
        }


        [HttpPost]
        public IActionResult RemoveSong()
        {
            try
            {
                var playlistId = Int32.Parse(Request.Form["playlistId"]);
                var songId = Int32.Parse(Request.Form["songId"]);
                var song = _context.Set<SongPlaylist>().Where(x => x.PlaylistId == playlistId && x.SongId == songId).FirstOrDefault();

                if (song != null)
                {
                    _context.Set<SongPlaylist>().Remove(song);
                    _context.SaveChanges();
                }
                return RedirectToAction("Edit", new { id = playlistId });
            } 
            catch 
            {
                Index();
                return View("Index");
            }
        }

        [HttpPost]
        public IActionResult Remove()
        {
            try
            {
                var playlistId = Int32.Parse(Request.Form["playlistId"]);
                var playlist = _context.Playlist.Find(playlistId);
                _context.Playlist.Remove(playlist);
                _context.SaveChanges();
            }catch { }

            Index();
            return View("Index");
        }

    }
}
