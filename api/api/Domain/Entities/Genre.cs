using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Domain.Entities;

[Table("genres")]
public class Genre
{
    public long Id { get; set; }

    [Column(TypeName = "varchar(20)")]
    [Required]
    [MinLength(3)]
    [MaxLength(20)]
    public required string Name { get; set; }
}