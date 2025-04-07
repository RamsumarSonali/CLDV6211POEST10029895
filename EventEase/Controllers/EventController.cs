using EventEase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EventEase.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;
        public EventController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _context.Event.ToListAsync();
            return View(events);
        }
        public IActionResult Create()
        {
            ViewBag.VenueId = new SelectList(_context.Venue, "VenueId", "Location");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Event events)
        {
            if (ModelState.IsValid)
            {
                _context.Add(events);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(events);
        }

        public async Task<IActionResult> Details(int? id)
        {
            var events = await _context.Booking.FirstOrDefaultAsync(m => m.EventId == id);
            if (events == null)
            {
                return NotFound();
            }
            return View(events);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var events = await _context.Event.FirstOrDefaultAsync(m => m.EventId == id);
            if (events == null)
            {
                return NotFound();
            }
            return View(events);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var events = await _context.Event.FindAsync(id);
            _context.Event.Remove(events);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.EventId == id);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
         
            var events = await _context.Event.FindAsync(id);
            if (events == null)
            {
                return NotFound();
            }
            ViewBag.VenueId = new SelectList(_context.Venue, "VenueId", "Location", events.VenueId);
            return View(events);
        }

        [HttpPost]
       
        public async Task<IActionResult> Edit(int id, Event events)
        {
            
            if (id != events.EventId)
            {
                return NotFound();
            }
        
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(events);
                    await _context.SaveChangesAsync();
                }
                //handle any exceptions
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(events.EventId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(events);
        }
    }
}



