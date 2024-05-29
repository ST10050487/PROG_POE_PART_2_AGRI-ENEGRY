using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using PROG_POE_PART_2_AGRI_ENEGRY.Areas.Data;
using PROG_POE_PART_2_AGRI_ENEGRY.Data;
using PROG_POE_PART_2_AGRI_ENEGRY.Models;
using Microsoft.AspNetCore.Authorization;

namespace PROG_POE_PART_2_AGRI_ENEGRY.Controllers
{
    [Authorize(Roles = "Farmer,Employee")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<PlatformUsers> _userManager;
        private string userID;
        public ProductsController(ApplicationDbContext context, UserManager<PlatformUsers> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Authorize(Roles = "Farmer")]
        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "CategoryName");
            return View();
        }
        [Authorize(Roles = "Farmer")]
        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,ProductionDate,CategoryId")] Product product, IFormFile PictureData)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            product.UserId = user.Id;
            product.User = user;
            userID = user.Id;
            if (PictureData != null && PictureData.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await PictureData.CopyToAsync(memoryStream);
                    product.PictureData = memoryStream.ToArray();
                }
            }
            else
            {
                // Set default image data if no image is uploaded
                product.PictureData = GetDefaultImageData();
            }

            product.CategoryId = product.CategoryId;
            product.Category = _context.Categories.Find(product.CategoryId);

            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "CategoryName", product.CategoryId);
            return View(product);
        }
        [Authorize(Roles = "Farmer")]
        // GET: Products/Edit/1
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Get the current logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var category = _context.Categories.Find(product.CategoryId);
            // Assuming you want to populate dropdown lists for UserId and CategoryId
            // Set the UserId directly to the user's Id
            product.UserId = user.Id;
            product.CategoryId = category.Id;
            ViewBag.UserId = new SelectList(_context.Users.Where(u => u.Id != null), "Id", "UserName", product.UserId);
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "CategoryName", product.CategoryId);

            return View(product);
        }
        [Authorize(Roles = "Farmer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,ProductionDate,CategoryId,UserId")] Product product, IFormFile PictureData)
        {
            var user = await _userManager.GetUserAsync(User); 
            if (id != product.Id)
            {
                return NotFound();
            }
            product.UserId = user.Id;
            product.User = user;
            userID = user.Id;

            if (PictureData != null && PictureData.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await PictureData.CopyToAsync(memoryStream);
                    product.PictureData = memoryStream.ToArray();
                }
            }
            else
            {
                // Set default image data if no image is uploaded
                product.PictureData = GetDefaultImageData();
            }

            product.CategoryId = product.CategoryId;
                product.Category = _context.Categories.Find(product.CategoryId);

                _context.Update(product);
                    await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));


            // Repopulating dropdown lists if ModelState is not valid
            ViewBag.UserId = new SelectList(_context.Users, "Id", "UserName", product.UserId);
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "CategoryName", product.CategoryId);

            return View(product);
        }
        [Authorize(Roles = "Farmer")]
        // GET: Products/Details/1
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.User)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        [Authorize(Roles ="Farmer,Employee")]
        // GET: Products/GetImage/1
        public async Task<IActionResult> GetImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null || product.PictureData == null)
            {
                return NotFound();
            }

            return File(product.PictureData, "image/jpeg"); 
        }
        [Authorize(Roles = "Farmer")]
        // GET: Products/Delete/1
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.User)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        [Authorize(Roles = "Farmer")]
        // POST: Products/Delete/1
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Filter(ProductFilterViewModel filter)
        {
            var query = _context.Product.Include(p => p.Category).Include(p => p.User).AsQueryable();

            if (filter.StartDate.HasValue)
            {
                query = query.Where(p => p.ProductionDate >= filter.StartDate.Value);
            }

            if (filter.EndDate.HasValue)
            {
                query = query.Where(p => p.ProductionDate <= filter.EndDate.Value);
            }

            if (filter.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == filter.CategoryId.Value);
            }

            // If the user selects a specific farmer
            if (!string.IsNullOrEmpty(filter.FarmerId))
            {
                query = query.Where(p => p.UserId == filter.FarmerId);
            }

            // Populate the Farmer dropdown with options for filtering
            var farmers = await _userManager.GetUsersInRoleAsync("Farmer");
            filter.Farmers = new SelectList(farmers.Select(f => new SelectListItem
            {
                Text = $"{f.Name} {f.Surname}", // Concatenate name and surname
                Value = f.Id // Use user ID as the value
            }), "Value", "Text");

            filter.Products = await query.ToListAsync();
            filter.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "CategoryName");

            return View(filter);
        }





        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Index()
        {
            // Get the current logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Filter products by the logged-in user's ID
            var products = await _context.Product
                .Include(p => p.Category)
                .Include(p => p.User)
                .Where(p => p.UserId == user.Id)
                .ToListAsync();

            return View(products);
        }


        private byte[] GetDefaultImageData()
        {
            throw new NotImplementedException();
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}

