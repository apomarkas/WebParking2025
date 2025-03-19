using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using WebParking2025.Data;
using WebParking2025.Models;

namespace WebParking2025.Controllers
{
    public class ParkingsInfoController : Controller
    {
        private readonly ParkingContext _context;
        private readonly SignInManager<User> _signInManager;
        

        public ParkingsInfoController(ParkingContext context, SignInManager<User> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
            
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

        public async Task<IActionResult> Reservation(int? id)
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

            if (_signInManager.IsSignedIn(User))
            {

                return View(parking);
            }
            else
            {
                return Redirect("/Login");
            }
        }



    }
}
