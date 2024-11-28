using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Domain.Entities;

[Table("albums")]
public class Album
{
    public long Id { get; set; }
    [MaxLength(100)] [MinLength(1)] public required string Title { get; set; }
    public required byte[] Cover { get; set; }
}