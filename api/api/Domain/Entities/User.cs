using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Domain.Entities;

[Table("users")]
public class User
{
    public long Id { get; set; }

    [Column(TypeName = "varchar(50)")]
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public required string Name { get; set; }

    [Column(TypeName = "varchar(100)")]
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public required string LastName { get; set; }

    [EmailAddress]
    [Required]
    [MinLength(10)]
    [MaxLength(100)]
    public required string Email { get; set; }

    [Column(TypeName = "varchar(100)")]
    [Required]
    [MinLength(8)]
    [MaxLength(100)]
    public required string Password { get; set; }

    [Required] public required long RoleId { get; set; }
    public Role Role { get; set; }
    public ICollection<Rating> Ratings { get; set; }
    public ICollection<UserSongs> UserSongs { get; set; }
}