using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cloud.Domain.Models;

public class FileRecord
{
    [Key]
    [DatabaseGenerated((DatabaseGeneratedOption.Identity))]
    public int fileId { get; set; }
    public string fileName { get; set; } = string.Empty;
    public string filePath { get; set; } = string.Empty;
    public string fileSize { get; set; } = string.Empty;
    public string fileType { get; set; } = string.Empty;
    
    public DateTime uploadedAt { get; set; } = DateTime.Now;

    public int userId { get; set; }
    public User user { get; set; }

}