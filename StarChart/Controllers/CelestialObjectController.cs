using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

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

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject obj)
        {
            //obj.Id = _context.CelestialObjects.Count() + 1;
            _context.CelestialObjects.Add(obj);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = obj.Id }, obj);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var existingObject = _context.CelestialObjects.Find(id);
            if (existingObject == null)
                return NotFound();
            existingObject.Name = celestialObject.Name;
            existingObject.OrbitalPeriod = celestialObject.OrbitalPeriod;
            existingObject.OrbitedObjectId = celestialObject.OrbitedObjectId;
            _context.CelestialObjects.Update(existingObject);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var existingObject = _context.CelestialObjects.Find(id);
            if (existingObject == null)
                return NotFound();
            existingObject.Name = name;
            _context.CelestialObjects.Update(existingObject);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var celestialObjects = _context.CelestialObjects.Where(e => e.Id == id || e.OrbitedObjectId == id);
            if (!celestialObjects.Any())
                return NotFound();
            _context.CelestialObjects.RemoveRange(celestialObjects);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
