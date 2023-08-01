namespace softrobotics.auth.domain.Entity;

public class UserValidate : BaseEntity
{
    public int UserValidateId { get; set; }

    public int UserId { get; set; }

    public string HashUUID { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
