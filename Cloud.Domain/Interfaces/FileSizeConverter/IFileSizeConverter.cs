namespace Cloud.Domain.Interfaces.FileSizeConverter;

public interface IFileSizeConverter
{
    public string FormatSize(long bytes);
}