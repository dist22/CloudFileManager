namespace Cloud.Interfaces;

public interface IFileSizeConverter
{
    public string FormatSize(long bytes);
}