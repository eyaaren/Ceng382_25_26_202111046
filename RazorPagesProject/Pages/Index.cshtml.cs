using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesProject.Models;
using RazorPagesProject.Helpers;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System; // Console için eklendi

namespace RazorPagesProject.Pages
{
    public class IndexModel : PageModel
    {
        public string WelcomeMessage { get; set; }
        private readonly ILogger<IndexModel> _logger;
        private static readonly Random _random = new Random(); // Random nesnesi eklendi

        // Static constructor ekliyoruz
        static IndexModel()
        {
            // Sadece liste boşsa fake veri ekle
            if (AllClasses.Count == 0)
            {
                GenerateFakeData(100);
            }
        }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        // Fake veri oluşturma metodu
        private static void GenerateFakeData(int count)
        {
            var classNames = new[] {"Mathematics", "Physics", "Chemistry", "Biology", "History",
                "Literature", "Geography", "Philosophy", "English", "Computer"};
            var descriptions = new[]{ "Basic", "Advanced", "Practical", "Theoretical", "Laboratory",
                "Online", "Face to face", "Mixed", "Project", "Seminar" };

            for (int i = 1; i <= count; i++)
            {
                AllClasses.Add(new ClassInformationModel
                {
                    Id = i,
                    ClassName = $"{classNames[_random.Next(classNames.Length)]} {_random.Next(1, 5)}0{i % 10}",
                    StudentCount = _random.Next(15, 45),
                    Description = $"{descriptions[_random.Next(descriptions.Length)]} ders - Grup {_random.Next(1, 10)}"
                });
            }
        }

        // Tüm sınıflar burada tutuluyor
        public static List<ClassInformationModel> AllClasses { get; set; } = new();

        // Sayfada gösterilecek sınıflar
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

        public class ExportRequest
        {
            public List<string> SelectedColumns { get; set; } = new();
            public string? Filter { get; set; }
            public int PageNumber { get; set; }
        }

        // JSON Export işlemi
        public IActionResult OnPostExportJson([FromBody] ExportRequest request)
        {
            var data = AllClasses.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                data = data.Where(x => x.ClassName.Contains(request.Filter, StringComparison.OrdinalIgnoreCase));
            }

            data = data.Skip((request.PageNumber - 1) * PageSize).Take(PageSize);

            var result = data.Select(x => new ClassInformationTable
            {
                Id = x.Id,
                ClassName = x.ClassName,
                StudentCount = x.StudentCount,
                Description = x.Description
            }).ToList();

            var json = Utils.Instance.ExportToJson(result, request.SelectedColumns);
            return File(Encoding.UTF8.GetBytes(json), "application/json", "exported_data.json");
        }

        // Sayfa yüklendiğinde verileri getir
      public void OnGet()
{
    // 🍪 Eğer kullanıcı çerezleri kabul ettiyse cookie yaz
    var acceptCookies = Request.Query["acceptCookies"];
    if (acceptCookies == "true")
    {
        Response.Cookies.Append("cookieConsent", "true", new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddYears(1),
            IsEssential = true // GDPR uyumu için
        });

        // Kullanıcı linke tıkladıktan sonra URL'deki ?acceptCookies=true kısmını temizlemek için sayfayı yenile
        Response.Redirect(Request.Path);
        return;
    }

    // 🔐 Session'dan kullanıcı bilgilerini al
    var username = HttpContext.Session.GetString("username");
    var role = HttpContext.Session.GetString("role");
    var message = HttpContext.Session.GetString("welcomeMessage");

    // Giriş yapılmamışsa login sayfasına yönlendir
    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(role))  // İF EKLE 
    {
        Response.Redirect("/Login");
        return;
    }

    // Hoş geldin mesajı
    WelcomeMessage = message ?? "";

    // 🔍 Filtreleme ve sayfalama
    var query = AllClasses.AsQueryable();

    if (!string.IsNullOrWhiteSpace(Filter))
    {
        query = query.Where(c => c.ClassName.Contains(Filter, StringComparison.OrdinalIgnoreCase));
    }

    TotalPages = (int)Math.Ceiling(query.Count() / (double)PageSize);

    ClassesForDisplay = query
        .Skip((PageNumber - 1) * PageSize)
        .Take(PageSize)
        .Select(c => new ClassInformationTable
        {
            Id = c.Id,
            ClassName = c.ClassName,
            StudentCount = c.StudentCount,
            Description = c.Description
        })
        .ToList();
}



        // Yeni sınıf ekleme
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

        // Sınıf düzenleme
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

        // Sınıf silme
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
