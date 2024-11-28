using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Domain.Entities;

[Table("genres")]
public class Genre
{
    public long Id { get; set; }
    [MinLength(3)] public required string Name { get; set; }
 
}