namespace softrobotics.auth.domain.Entity;

public class BaseEntity
{
    public bool IsActive { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
}