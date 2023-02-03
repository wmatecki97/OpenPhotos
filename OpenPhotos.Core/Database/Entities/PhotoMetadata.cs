namespace OpenPhotos.Core.Database.Entities;

public class PhotoMetadata
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Size { get; set; }
    public DateTime DateTaken { get; set; }
    public string Iso { get; set; }
    public string FStop { get; set; }
    public string FocalLength { get; set; }
    public int Longitude { get; set; }
    public int Attitude { get; set; }
    public List<Tag> Tags { get; set; }
}