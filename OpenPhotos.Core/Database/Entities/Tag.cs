namespace OpenPhotos.Core.Database.Entities;

public class Tag
{
    public Guid Id { get; set; }
    public string Value { get; set; }
    public double Confidence { get; set; }
    public PhotoMetadata Photo { get; set; }
}