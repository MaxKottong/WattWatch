using System.ComponentModel.DataAnnotations;

namespace WattWatch.Models;

public class RegisterModel {
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required, MinLength(6)]
    public string Password { get; set; }

    [Required, Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string PasswordConfirm { get; set; }
}