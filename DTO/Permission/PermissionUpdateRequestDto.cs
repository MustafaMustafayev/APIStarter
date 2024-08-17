using System.ComponentModel.DataAnnotations;

namespace DTO.Permission;

public record PermissionUpdateRequestDto()
{
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string Key { get; set; }
}