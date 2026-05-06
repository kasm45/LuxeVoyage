using Microsoft.AspNetCore.Identity;

namespace LuxeVoyage.Mvc.Models;

public class ApplicationUser : IdentityUser
{
    public string DisplayName { get; set; } = "";
}
