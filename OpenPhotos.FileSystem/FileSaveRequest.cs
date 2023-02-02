namespace OpenPhotos.Contracts
{
    internal class FileSaveRequest
    {
        public string Name { get; set; }
        public byte[] Content { get; set; }
    }
}