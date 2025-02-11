using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebParking2025.Data;

namespace WebParking2025.Controllers
{
    public class ParkingsInfoController : Controller
    {
        private readonly ParkingContext _context;

        public ParkingsInfoController(ParkingContext context)
        {
            _context = context;
        }
        // GET: ParkingsInfo/Details/5

        public async Task<IActionResult> Info(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parking = await _context.Parkings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parking == null)
            {
                return NotFound();
            }

            return View(parking);
        }

    }
}
