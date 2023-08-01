using System.ComponentModel.DataAnnotations.Schema;

namespace softrobotics.auth.domain.Entity;

public class User : BaseEntity
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Mail { get; set; } = null!;

    [NotMapped]
    public ICollection<UserValidate> UserValidates { get; set; } = new List<UserValidate>();
}