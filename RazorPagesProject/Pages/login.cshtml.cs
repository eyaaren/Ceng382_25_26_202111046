using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;
using System.IO;

public class LoginModel : PageModel
{
    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    public string Password { get; set; }

    public string ErrorMessage { get; set; }

    public LoginModel()
    {
        Username = string.Empty;
        Password = string.Empty;
        ErrorMessage = string.Empty;
    }

    public IActionResult OnPost()
    {
        var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/users.json");
        var json = System.IO.File.ReadAllText(jsonPath);
        var users = JsonSerializer.Deserialize<List<User>>(json);

        if (users == null)
        {
            ErrorMessage = "Kullanıcı verileri yüklenemedi.";
            return Page();
        }

        // Kullanıcı adı ve şifreyi kontrol et
        var user = users.FirstOrDefault(u => u.Username.ToLower() == Username.ToLower());

        if (user != null)
        {
            if (user.Password == Password && user.IsActive)
            {
                var token = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("username", Username);
                HttpContext.Session.SetString("token", token);
                HttpContext.Session.SetString("role", user.Role);
                HttpContext.Session.SetString("session_id", HttpContext.Session.Id);

                var options = new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                };
                Response.Cookies.Append("auth_token", token, options);

                return LocalRedirect("~/Index");
            }
            else
            {
                ErrorMessage = "Kullanıcı adı, şifre yanlış veya kullanıcı aktif değil!";
            }
        }
        else
        {
            ErrorMessage = "Kullanıcı adı veya şifre yanlış!";
        }

        return Page();
    }
}
