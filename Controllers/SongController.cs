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
           
            var Files = Request.Form.Files;
            
            foreach(var file in Files)
            {
                var songs = Path.Combine(hostingEnvironment.WebRootPath, "songs");
                if (file.Length > 0)
                {
                    string filename = Guid.NewGuid().ToString();
                    var old_filename = file.FileName.Split('.');
                    var extension = old_filename.Last();

                    song.PathFileSong = filename + "." + extension;
                    using (var fileStream = new FileStream(Path.Combine(songs, song.PathFileSong), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }
            }
            _context.Add(song);
            _context.SaveChanges();

            return View();
         
        }

    }
}
