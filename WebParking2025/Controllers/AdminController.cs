using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebParking2025.Data;
using WebParking2025.Models;

namespace WebParking2025.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ParkingContext _context;
        public AdminController(ParkingContext context)
        {
            _context = context;
        }

        public IActionResult Panel()
        {
            return View();
        }

        public async Task<IActionResult> ManageRoles() {

            Dictionary<User,string> userRoles = new Dictionary<User,string>();
            
            var roleEntries = await _context.UserRoles.ToListAsync();
            foreach (var entry in roleEntries)
            {   
               
                var user = _context.Users.Find(entry.UserId);
                var role = _context.Roles.Find(entry.RoleId);
                userRoles.Add(user, role.Name);
            }
          return View(userRoles);
           
        }
    }
}
