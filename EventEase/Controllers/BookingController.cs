using EventEase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
   using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EventEase.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var booking = await _context.Booking.Include(b => b.Venue).Include(b => b.Event).ToListAsync();
            return View(booking);
        }
        public IActionResult Create()
        {
            ViewBag.VenueId = new SelectList(_context.Venue, "VenueId", "Location");
            ViewBag.EventId = new SelectList(_context.Event, "EventId", "EventName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(booking);
        }

        public async Task<IActionResult> Details(int? id)
        {
            var booking = await _context.Booking.FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var booking = await _context.Booking.FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            _context.Booking.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.BookingId == id);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //look for the booking in the database
            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewBag.VenueId = new SelectList(_context.Venue, "VenueId", "Location", booking.VenueId);
            ViewBag.EventId = new SelectList(_context.Event, "EventId", "EventName", booking.EventId);
            //pass booking for editing
            return View(booking);
        }

        [HttpPost]
        //retreive booking data for editing
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            //ensure correct booking is being edited
            if (id != booking.BookingId)
            {
                return NotFound();
            }
            //if booking is valid -> updates information
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                //handle any exceptions
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
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
            ViewBag.VenueId = new SelectList(_context.Venue, "VenueId", "Location", booking.VenueId);
            ViewBag.EventId = new SelectList(_context.Event, "EventId", "EventName", booking.EventId);
            return View(booking);
        }
    }
}



