namespace Cloud.DTOs.File;

public class FileDTOs
{
    public int fileId { get; set; }
    public string fileName { get; set; }
    public string fileSize { get; set; }
    public string fileType { get; set; }
    public DateTime uploadedAt { get; set; }
    public int userId { get; set; }
}