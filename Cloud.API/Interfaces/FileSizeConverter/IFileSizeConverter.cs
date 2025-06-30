namespace Cloud.Interfaces.FileSizeConverter;

public interface IFileSizeConverter
{
    public string FormatSize(long bytes);
}