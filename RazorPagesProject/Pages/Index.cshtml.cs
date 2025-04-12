using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesProject.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RazorPagesProject.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        // Kalıcı liste (test amaçlı)
        public static List<ClassInformationModel> AllClasses { get; set; } = new();

        public List<ClassInformationTable> ClassesForDisplay { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Filter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }

        [BindProperty]
        public ClassInformationModel NewClass { get; set; } = new();

        [BindProperty]
        public ClassInformationModel EditClass { get; set; } = new();

        public void OnGet()
        {
            var filtered = AllClasses.AsQueryable();

            if (!string.IsNullOrWhiteSpace(Filter))
            {
                filtered = filtered.Where(c => c.ClassName.Contains(Filter, StringComparison.OrdinalIgnoreCase));
            }

            int totalItems = filtered.Count();
            TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            filtered = filtered.Skip((PageNumber - 1) * PageSize).Take(PageSize);

            ClassesForDisplay = filtered.Select(c => new ClassInformationTable
            {
                Id = c.Id,
                ClassName = c.ClassName,
                StudentCount = c.StudentCount,
                Description = c.Description
            }).ToList();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                NewClass.Id = AllClasses.Any() ? AllClasses.Max(c => c.Id) + 1 : 1;
                AllClasses.Add(NewClass);
                return RedirectToPage("/Index");
            }

            OnGet();
            return Page();
        }

        public IActionResult OnPostEdit()
        {
            var existing = AllClasses.FirstOrDefault(c => c.Id == EditClass.Id);
            if (existing != null)
            {
                existing.ClassName = EditClass.ClassName;
                existing.StudentCount = EditClass.StudentCount;
                existing.Description = EditClass.Description;
            }

            return RedirectToPage("/Index");
        }

        public IActionResult OnGetDelete(int id)
        {
            var classToRemove = AllClasses.FirstOrDefault(c => c.Id == id);
            if (classToRemove != null)
            {
                AllClasses.Remove(classToRemove);
            }

            return RedirectToPage("/Index");
        }
    }
}
