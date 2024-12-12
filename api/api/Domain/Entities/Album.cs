using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Domain.Entities;

[Table("albums")]
public class Album
{
    public long Id { get; set; }

    [Column(TypeName = "varchar(100)")]
    [Required]
    [MaxLength(100)]
    [MinLength(1)]
    public required string Title { get; set; }

    [Required] public required byte[] Cover { get; set; }
}