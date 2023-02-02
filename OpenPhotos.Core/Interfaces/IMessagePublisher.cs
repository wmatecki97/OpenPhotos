using OpenPhotos.Contracts;

namespace OpenPhotos.Core.Interfaces
{
    public interface IMessagePublisher
    {
        void PublishSaveFileMessage(FileSaveRequest data);
    }
}