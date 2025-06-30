using Cloud.Domain.Interfaces.FileSizeConverter;

namespace Cloud.Infrastructure.FileSizeConverter;

public class FileSizeConverter : IFileSizeConverter
{
    public string FormatSize(long bytes)
    {
        string[] sizeUnits = ["B", "KB", "MB", "GB", "TB"];
        double size = bytes;
        int unitIndex = 0;

        while (size >= 1024 && unitIndex < sizeUnits.Length - 1)
        {
            size /= 1024;
            unitIndex++;
        }

        if (unitIndex == 0)
            return $"{(long)size} {sizeUnits[unitIndex]}";

        return $"{size:F2} {sizeUnits[unitIndex]}";
    }
}