using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Domain.Entities;

[Table("roles")]
public class Role
{
    public long Id { get; set; }

    [Column(TypeName = "varchar(20)")]
    [MinLength(3)]
    [MaxLength(20)]
    public required string Name { get; set; }
}