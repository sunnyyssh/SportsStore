using System.ComponentModel.DataAnnotations;

namespace SportsStore.Models.ViewModel;

public sealed class LoginModel
{
    [Required(ErrorMessage = "Please enter Name")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Please enter password")]
    public required string Password { get; set; }

    public string ReturnUrl { get; set; } = "/";
}