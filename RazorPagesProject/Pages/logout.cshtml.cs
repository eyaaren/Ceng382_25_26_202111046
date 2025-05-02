using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class LogoutModel : PageModel
{
    public IActionResult OnGet()
    {
        // Session'ı temizle
        HttpContext.Session.Clear();

        // Tüm cookie'leri sil
        foreach (var cookie in Request.Cookies.Keys)
        {
            Response.Cookies.Delete(cookie);
        }

        // Kullanıcıyı Login sayfasına yönlendir
        return Redirect("/Login");
    }
}
