namespace Cloud.DTOs;

public class UserDTOs
{
    public int userId { get; set; }
    public string username { get; set; }
    public string email { get; set; }
    public string role { get; set; }
    public DateTime createAt { get; set; } = DateTime.Now;
}