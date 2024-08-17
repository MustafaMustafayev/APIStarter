using ENTITIES.Entities.Generic;
using System.ComponentModel.DataAnnotations;

namespace ENTITIES.Entities;

public class User : Auditable, IEntity
{
    public Guid Id { get; set; }
    [Required]
    public required string Username { get; set; }
    [EmailAddress]
    public required string Email { get; set; }
    [Phone]
    public required string ContactNumber { get; set; }
    [Required]
    public required string Password { get; set; }
    [Required]
    public required string Salt { get; set; }
    public Role? Role { get; set; }
    public Guid? RoleId { get; set; }
    public string? Image { get; set; }
}