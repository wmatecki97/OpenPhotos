namespace OpenPhotos.Contracts;

public class FileSaveRequest
{
    public string Name { get; set; }
    public byte[] Content { get; set; }
}