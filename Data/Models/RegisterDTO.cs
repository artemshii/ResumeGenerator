using System.ComponentModel.DataAnnotations;

public class RegisterDto
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}