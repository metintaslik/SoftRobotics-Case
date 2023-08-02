namespace softrobotics.shared.Models;

public class UserCreatedEventModel
{
    public UserCreatedEventModel(
            int userId,
            string uuid,
            string mail
        )
    {
        UserID = userId;
        UUID = uuid;
        Mail = mail;
    }

    public int UserID { get; set; }
    public string UUID { get; set; }
    public string Mail { get; set; }
}