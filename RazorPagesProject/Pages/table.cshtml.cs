using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class TableModel : PageModel
{
    public IActionResult OnGet()
    {
        var sessionToken = HttpContext.Session.GetString("token");
        var cookieToken = Request.Cookies["auth_token"];
        var username = HttpContext.Session.GetString("username");

        if (string.IsNullOrEmpty(sessionToken) || sessionToken != cookieToken || string.IsNullOrEmpty(username))
        {
            return RedirectToPage("/Login");
        }

        return Page();
    }
}
