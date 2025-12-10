using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

[Authorize]
public class WelcomeModel : PageModel
{
    public void OnGet()
    {
    }
}
