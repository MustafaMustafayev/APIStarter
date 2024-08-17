using ENTITIES.Entities.Generic;
using System.ComponentModel.DataAnnotations;

namespace ENTITIES.Entities;

public class Permission : Auditable, IEntity
{
    public Guid Id { get; set; }
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string Key { get; set; }
    public List<Role>? Roles { get; set; }
}