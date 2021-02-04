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
        public ViewResult Edit(int id)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            Playlist playlist = _context.Playlist.Find(id);

            if(playlist != null)
            {
                if (playlist.userID == currentUserID)
                {
                    List<SongPlaylist> songplaylist = (from x in _context.SongPlaylist where x.PlaylistId == id select x).Include(q => q.Song).ToList();
                    //songplaylist.Select(o => new
                    //{
                    //    o.PlaylistId,
                    //    Songs = o.SongPlaylist.Select(ot => ot.Song).ToList()
                    //});
                    ViewData["songs"] = songplaylist;
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

    }
}
