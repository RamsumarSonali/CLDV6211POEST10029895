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

        public async Task<IActionResult> Index(string searchType, int? venueId, DateTime? startDate, DateTime? endDate)
        {
            var events = _context.Event
                .Include(e => e.Venue)
                .Include(e => e.EventType)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchType))
            {
                events = events.Where(e => e.EventType.Name == searchType);
            }
            if (venueId.HasValue)
            {
                events = events.Where(e => e.VenueId == venueId);
            }
            if(startDate.HasValue && endDate.HasValue)
            {
                events = events.Where(e => e.EventDate >= startDate && e.EventDate <= endDate);
            }

            //drop down filters
            ViewData["EventTypes"] = _context.EventType.ToList();
            ViewData["Venues"] = _context.Venue.ToList();

            return View(await events.ToListAsync());
        }
        public IActionResult Create()
        {
            ViewData["Venues"] = _context.Venue.ToList();
            ViewData["EventTypes"] = _context.EventType.ToList();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create([Bind("EventId,EventName,EventDate,VenueId,ImageUrl")] Event events)
        {
            if (ModelState.IsValid)
            {
                _context.Add(events);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Event created successfully.";
                return RedirectToAction(nameof(Index));
            }
            ViewData["Venues"] = _context.Venue.ToList();
            ViewData["EventTypes"] = _context.EventType.ToList();
            return View(events);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var events = await _context.Event
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (events == null) return NotFound();
            return View(events);
        }

        //1: cofirming the delete action (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var events = await _context.Event
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (events == null) return NotFound();
            return View(events);
        }

        //2: Performing the delete action (POST) -> added logic to check if the event has any bookings associated with it
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Check if the event exists
            var events = await _context.Event.FindAsync(id);
            if (events == null) return NotFound();

            var isBooked = await _context.Booking.AnyAsync(b => b.EventId == id);
            if (isBooked)
            {
                TempData["ErrorMessage"] = "Cannot delete event with existing bookings.";
                return RedirectToAction(nameof(Index));
            }
            _context.Event.Remove(events);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Event deleted successfully.";
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
            ViewData["Venues"] = _context.Venue.ToList();
            ViewData["EventTypes"] = _context.EventType.ToList();
            return View(events);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,EventName,EventDate,VenueId,ImageUrl")] Event events)
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