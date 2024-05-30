using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROG_POE_PART_2_AGRI_ENEGRY.Data;
using PROG_POE_PART_2_AGRI_ENEGRY.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Security.Claims;
using PROG_POE_PART_2_AGRI_ENEGRY.Areas.Data;
using PROG_POE_PART_2_AGRI_ENEGRY.Data.Migrations;

namespace PROG_POE_PART_2_AGRI_ENEGRY.Controllers
{
    [Authorize(Roles = "Employee")]
    public class FarmersController : Controller
    {
        private readonly UserManager<PlatformUsers> _userManager;
        private readonly ApplicationDbContext _context;

        public FarmersController(ApplicationDbContext context,
                                UserManager<PlatformUsers> userManager,
                                ILogger<FarmersController> logger,
                                IEmailSender emailSender,
                                RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        private readonly ILogger<FarmersController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;

        public async Task<IActionResult> Index()
        {
            var farmerUsers = await _userManager.GetUsersInRoleAsync("Farmer");
            var applicationUsers = farmerUsers.Select(u => new ApplicationUser
            {
                UserName = u.UserName,
                Email = u.Email,
                Name = u.Name,
                Surname = u.Surname,
                PhoneNumber = u.PhoneNumber,
                Address = u.Address
            }).ToList();

            return View(applicationUsers);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmer = await _context.Farmer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (farmer == null)
            {
                return NotFound();
            }

            return View(farmer);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FarmerName,FarmerSurname,Age,Email,Password,PhoneNumber,Address")] Farmer farmer)
        {
            if (ModelState.IsValid)
            {
                var user = new PlatformUsers
                {
                    UserName = farmer.Email,
                    Email = farmer.Email,
                    Name = farmer.FarmerName,
                    Surname = farmer.FarmerSurname,
                    CellPhoneNumber = farmer.PhoneNumber,
                    PhoneNumber = farmer.PhoneNumber,
                    Cellphone = farmer.PhoneNumber,
                    Address = farmer.Address
                };

                var result = await _userManager.CreateAsync(user, farmer.Password);

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync("Farmer"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Farmer"));
                    }
                    await _userManager.AddToRoleAsync(user, "Farmer");

                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { area = "Identity", userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    await _emailSender.SendEmailAsync(farmer.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(farmer);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmer = await _context.Farmer.FindAsync(id);
            if (farmer == null)
            {
                return NotFound();
            }
            return View(farmer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FarmerName,FarmerSurname,Age,Email,Password,PhoneNumber,Address")] Farmer farmer)
        {
            if (id != farmer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(farmer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FarmerExists(farmer.Id))
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
            return View(farmer);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmer = await _context.Farmer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (farmer == null)
            {
                return NotFound();
            }

            return View(farmer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var farmer = await _context.Farmer.FindAsync(id);
            if (farmer != null)
            {
                _context.Farmer.Remove(farmer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FarmerExists(int id)
        {
            return _context.Farmer.Any(e => e.Id == id);
        }
    }
}
