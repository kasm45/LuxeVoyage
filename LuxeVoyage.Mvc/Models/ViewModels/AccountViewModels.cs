using System.ComponentModel.DataAnnotations;

namespace LuxeVoyage.Mvc.Models.ViewModels;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";

    public bool RememberMe { get; set; }

    /// <summary>When true, post-login requires Admin role and redirects to /admin.</summary>
    public bool AdminLogin { get; set; }

    public string? ReturnUrl { get; set; }
}

public class RegisterViewModel
{
    [Required]
    [MaxLength(120)]
    public string DisplayName { get; set; } = "";

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    [StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";

    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = "";
}
