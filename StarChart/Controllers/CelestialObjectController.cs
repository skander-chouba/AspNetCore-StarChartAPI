using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var Name = _context.CelestialObjects.Where(x => x.Id == id).FirstOrDefault();
            if (Name == null) return NotFound();
            var satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == id).ToList();
            if (satellites != null && satellites.Any()) Name.Satellites = satellites;
            return Ok(Name);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var Name = _context.CelestialObjects.Where(x => x.Name == name).ToList();
            if (Name == null || !Name.Any()) return NotFound();
            foreach (var item in Name)
            {
                var satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == item.Id).ToList();
                if (satellites != null && satellites.Any()) item.Satellites = satellites;
            }
            return Ok(Name);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var Name = _context.CelestialObjects.ToList();
            if (Name == null || !Name.Any()) return NotFound();
            foreach (var item in Name)
            {
                var satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == item.Id).ToList();
                if (satellites != null && satellites.Any()) item.Satellites = satellites;
            }
            return Ok(Name);
        }
    }
}
