namespace Cloud.Models;

public class User
{
    public int userId { get; set; }
    public string auth0Id { get; set; } = string.Empty;
    public string email { get; set; } = String.Empty;

    public List<FileRecord> files { get; set; } = new List<FileRecord>();
}