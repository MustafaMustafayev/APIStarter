using System.ComponentModel.DataAnnotations;

namespace DTO.Auth;

public record LoginRequestDto()
{
    [EmailAddress]
    public required string Email { get; set; }
    [Required]
    public required string Password { get; set; }
}