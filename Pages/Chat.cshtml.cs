using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

[Authorize]
public class ChatModel : PageModel
{
    public void OnGet() { }
}
