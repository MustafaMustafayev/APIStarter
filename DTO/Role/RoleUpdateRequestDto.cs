using System.ComponentModel.DataAnnotations;

namespace DTO.Role;

public record RoleUpdateRequestDto()
{
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string Key { get; set; }
    public List<Guid>? PermissionIds { get; set; }
}