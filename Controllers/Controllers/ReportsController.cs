using BiartBiPortal.Data;
using BiartBiPortal.Models;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BiartBiPortal.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reports
        public async Task<IActionResult> Index(int? categoryId, string? search)
        {
            // Only show published reports
            var query = _context.Reports
                .Include(r => r.ReportCategories)
                .ThenInclude(rc => rc.Category)
                .Include(r => r.CreatedByUser)
                .Where(r => r.IsPublished)
                .AsQueryable();

            // Filter by category if specified
            if (categoryId.HasValue)
            {
                query = query.Where(r => r.ReportCategories.Any(rc => rc.CategoryId == categoryId.Value));
            }

            // Search in title and summary if specified
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(r => 
                    r.Title.Contains(search) || 
                    (r.Summary != null && r.Summary.Contains(search)));
            }

            var reports = await query
                .OrderByDescending(r => r.PublishedDate)
                .ToListAsync();

            // Load categories for filter dropdown
            ViewBag.Categories = await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.SearchTerm = search;

            return View(reports);
        }

        // GET: Reports/Details/5
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
                .FirstOrDefaultAsync(m => m.Id == id && m.IsPublished);

            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // GET: Reports/ExportToExcel
        public async Task<IActionResult> ExportToExcel(int? categoryId, string? search)
        {
            // Apply same filters as Index action
            var query = _context.Reports
                .Include(r => r.ReportCategories)
                .ThenInclude(rc => rc.Category)
                .Include(r => r.CreatedByUser)
                .Where(r => r.IsPublished)
                .AsQueryable();

            if (categoryId.HasValue)
            {
                query = query.Where(r => r.ReportCategories.Any(rc => rc.CategoryId == categoryId.Value));
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(r => 
                    r.Title.Contains(search) || 
                    (r.Summary != null && r.Summary.Contains(search)));
            }

            var reports = await query
                .OrderByDescending(r => r.PublishedDate)
                .ToListAsync();

            // Create Excel workbook
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Raporlar");

            // Add headers
            worksheet.Cell(1, 1).Value = "Başlık";
            worksheet.Cell(1, 2).Value = "Özet";
            worksheet.Cell(1, 3).Value = "Kategoriler";
            worksheet.Cell(1, 4).Value = "Yayın Tarihi";
            worksheet.Cell(1, 5).Value = "Oluşturan";

            // Style headers
            var headerRange = worksheet.Range(1, 1, 1, 5);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

            // Add data
            int row = 2;
            foreach (var report in reports)
            {
                worksheet.Cell(row, 1).Value = report.Title;
                worksheet.Cell(row, 2).Value = report.Summary ?? "";
                
                var categories = string.Join(", ", report.ReportCategories.Select(rc => rc.Category.Name));
                worksheet.Cell(row, 3).Value = categories;
                
                worksheet.Cell(row, 4).Value = report.PublishedDate?.ToLocalTime().ToString("dd.MM.yyyy HH:mm") ?? "";
                worksheet.Cell(row, 5).Value = report.CreatedByUser?.UserName ?? "";
                
                row++;
            }

            // Auto-fit columns
            worksheet.Columns().AdjustToContents();

            // Generate file
            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            var fileName = $"Reports_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
