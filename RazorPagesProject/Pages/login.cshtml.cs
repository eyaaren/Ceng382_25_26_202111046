using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;

public class LoginModel : PageModel
{
    [BindProperty]
    public string Username { get; set; }
    
    [BindProperty]
    public string Password { get; set; }

    public string ErrorMessage { get; set; }

    public IActionResult OnPost()
{
    var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/users.json");
    var json = System.IO.File.ReadAllText(jsonPath);
    var users = JsonSerializer.Deserialize<List<User>>(json);

    // Kullanıcı adı, şifre ve aktif kullanıcıyı kontrol et
    var user = users.FirstOrDefault(u =>
        u.Username.ToLower() == Username.ToLower() &&
        u.Password == Password &&
        u.IsActive);

    if (user == null)
    {
        // Kullanıcı bulunamazsa hata mesajı
        ErrorMessage = "Kullanıcı adı veya şifre yanlış!";
        return Page(); // Formu tekrar göster
    }

    // Token üretimi ve session/cookie işlemleri
    var token = Guid.NewGuid().ToString();
    HttpContext.Session.SetString("username", Username);
    HttpContext.Session.SetString("token", token);
    HttpContext.Session.SetString("role", user.Role);  // Rol bilgisi de ekleniyor
    HttpContext.Session.SetString("session_id", HttpContext.Session.Id);

    var options = new CookieOptions
    {
        Expires = DateTime.UtcNow.AddMinutes(30),
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict
    };
    Response.Cookies.Append("auth_token", token, options);

    // Kullanıcı rolüne göre yönlendirme yap
    // Her iki kullanıcıyı da Index'e yönlendiriyoruz, ancak mesaj farklı olacak
    /*
    if (user.Role == "admin")
    {
        // Admin'e özel mesaj
        HttpContext.Session.SetString("welcomeMessage", "Hoş geldiniz Admin!");
    }
    else
    {
        // User'a özel mesaj
        HttpContext.Session.SetString("welcomeMessage", "Hoş geldiniz User!");
    }
    */

    return LocalRedirect("~/Index");  // İkisinde de Index sayfasına yönlendir
}


}
