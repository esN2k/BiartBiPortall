using BiartBiPortal.Data;
using BiartBiPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BiartBiPortal.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ReportsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin/Reports
        public async Task<IActionResult> Index()
        {
            var reports = await _context.Reports
                .Include(r => r.CreatedByUser)
                .Include(r => r.ReportCategories)
                .ThenInclude(rc => rc.Category)
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();
            return View(reports);
        }

        // GET: Admin/Reports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports
                .Include(r => r.CreatedByUser)
                .Include(r => r.ReportCategories)
                .ThenInclude(rc => rc.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // GET: Admin/Reports/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();
            return View();
        }

        // POST: Admin/Reports/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content,Summary,IsPublished")] Report report, int[] selectedCategories)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                report.CreatedByUserId = user?.Id;
                report.CreatedDate = DateTime.UtcNow;
                
                if (report.IsPublished)
                {
                    report.PublishedDate = DateTime.UtcNow;
                }

                _context.Add(report);
                await _context.SaveChangesAsync();

                // Add category relationships
                if (selectedCategories != null && selectedCategories.Length > 0)
                {
                    foreach (var categoryId in selectedCategories)
                    {
                        _context.ReportCategories.Add(new ReportCategory
                        {
                            ReportId = report.Id,
                            CategoryId = categoryId
                        });
                    }
                    await _context.SaveChangesAsync();
                }

                TempData["SuccessMessage"] = "Rapor başarıyla oluşturuldu.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            return View(report);
        }

        // GET: Admin/Reports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports
                .Include(r => r.ReportCategories)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (report == null)
            {
                return NotFound();
            }

            ViewBag.Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            ViewBag.SelectedCategories = report.ReportCategories.Select(rc => rc.CategoryId).ToArray();
            return View(report);
        }

        // POST: Admin/Reports/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,Summary,IsPublished,CreatedDate,CreatedByUserId")] Report report, int[] selectedCategories)
        {
            if (id != report.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    report.UpdatedDate = DateTime.UtcNow;
                    
                    if (report.IsPublished && report.PublishedDate == null)
                    {
                        report.PublishedDate = DateTime.UtcNow;
                    }
                    else if (!report.IsPublished)
                    {
                        report.PublishedDate = null;
                    }

                    _context.Update(report);

                    // Remove existing category relationships
                    var existingRelations = await _context.ReportCategories
                        .Where(rc => rc.ReportId == id)
                        .ToListAsync();
                    _context.ReportCategories.RemoveRange(existingRelations);

                    // Add new category relationships
                    if (selectedCategories != null && selectedCategories.Length > 0)
                    {
                        foreach (var categoryId in selectedCategories)
                        {
                            _context.ReportCategories.Add(new ReportCategory
                            {
                                ReportId = report.Id,
                                CategoryId = categoryId
                            });
                        }
                    }

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Rapor başarıyla güncellendi.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportExists(report.Id))
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

            ViewBag.Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            ViewBag.SelectedCategories = selectedCategories ?? Array.Empty<int>();
            return View(report);
        }

        // GET: Admin/Reports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports
                .Include(r => r.CreatedByUser)
                .Include(r => r.ReportCategories)
                .ThenInclude(rc => rc.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // POST: Admin/Reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report != null)
            {
                _context.Reports.Remove(report);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Rapor başarıyla silindi.";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ReportExists(int id)
        {
            return _context.Reports.Any(e => e.Id == id);
        }
    }
}
