using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cloud.Models;

public class User
{
    [Key]
    [DatabaseGenerated((DatabaseGeneratedOption.Identity))]
    public int userId { get; set; }

    public string username { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;

    public string containerName { get; set; } = string.Empty;
    public string role { get; set; } = "Admin";
    
    public DateTime createAt { get; set; } = DateTime.Now;
    
    public DateTime updateAt { get; set; } = DateTime.Now;
    public List<FileRecord> files { get; set; } = new List<FileRecord>();
}