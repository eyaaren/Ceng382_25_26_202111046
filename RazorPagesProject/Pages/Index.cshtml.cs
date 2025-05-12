using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesProject.Models;
using RazorPagesProject.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using RazorPagesProject.Data;

namespace RazorPagesProject.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SchoolDbContext _context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(SchoolDbContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;

            // CS8618 hatalarını önlemek için başlatmalar
            WelcomeMessage = string.Empty;
            ClassList = new List<Class>();
            NewClass = new Class();
            EditClass = new Class();
        }

        public string WelcomeMessage { get; set; }

        // Sayfada gösterilecek sınıflar
        public IList<Class> ClassList { get; set; }

        [BindProperty]
        public Class NewClass { get; set; }  // ✅ Yeni sınıf eklemek için

        [BindProperty]
        public Class EditClass { get; set; } // ✅ Mevcut sınıfı düzenlemek için

        [BindProperty(SupportsGet = true)]
        public string? Filter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }

        // Sayfa yüklendiğinde verileri getir
        public async Task OnGetAsync()
{
    // Kullanıcının oturum açıp açmadığını kontrol et
    if (string.IsNullOrEmpty(HttpContext.Session.GetString("username")))
    {
        // Eğer giriş yapılmamışsa, Login sayfasına yönlendir
        Response.Redirect("/Login");
        return; // Yönlendirme yapıldıktan sonra fonksiyonu sonlandır
    }

    var query = _context.Classes.AsQueryable();

    // Filtreleme işlemi
    if (!string.IsNullOrWhiteSpace(Filter))
    {
        query = query.Where(c => c.Name.Contains(Filter, StringComparison.OrdinalIgnoreCase));
    }

    // Sayfa sayısını hesapla
    TotalPages = (int)Math.Ceiling(await query.CountAsync() / (double)PageSize);

    // Sınıf listesini sayfalandırarak al
    ClassList = await query
        .Skip((PageNumber - 1) * PageSize)
        .Take(PageSize)
        .ToListAsync();

    // Hoş geldiniz mesajı
    WelcomeMessage = "Hoş geldiniz, " + HttpContext.Session.GetString("username");
}

        // Yeni sınıf ekleme
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                _context.Classes.Add(NewClass);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Index");
            }

            return Page();
        }

        // Sınıf düzenleme
        public async Task<IActionResult> OnPostEditAsync()
        {
            var existingClass = await _context.Classes.FindAsync(EditClass.Id);
            if (existingClass != null)
            {
                existingClass.Name = EditClass.Name;
                existingClass.PersonCount = EditClass.PersonCount;
                existingClass.Description = EditClass.Description;
                existingClass.IsActive = EditClass.IsActive;

                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Index");
        }

        // Sınıf silme
        public async Task<IActionResult> OnGetDeleteAsync(int id)
        {
            var classToRemove = await _context.Classes.FindAsync(id);
            if (classToRemove != null)
            {
                _context.Classes.Remove(classToRemove);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Index");
        }
    }
}
