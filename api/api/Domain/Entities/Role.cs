using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Domain.Entities;

[Table("roles")]
public class Role
{
    public long Id { get; set; }
    [MinLength(3)] public required string Name { get; set; }
}