namespace OpenPhotos.Core.Database.Entities;

public class Tag
{
    public Guid Id { get; set; }
    public string Value { get; set; }
    public int Confidence { get; set; }
    public int MyProperty { get; set; }
    public PhotoMetadata Photo { get; set; }
}