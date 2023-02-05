using System.Globalization;
using OpenPhotos.Core.Database.Entities;
using OpenPhotos.Core.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;

namespace OpenPhotos.Core.FileProcessing;

internal class FileMetadataReader : IFileMetadataReader
{
    public PhotoMetadata GetFileMetadata(byte[] imageData)
    {
        using var image = Image.Load(imageData);
        // Get the Exif profile from the image
        var exifProfile = image.Metadata.ExifProfile;

        var dateTakenString = exifProfile.GetValue(ExifTag.DateTimeDigitized);
        var isoString = exifProfile.GetValue(ExifTag.ISOSpeed);
        var fStopString = exifProfile.GetValue(ExifTag.ApertureValue);
        var dateTaken = DateTime.ParseExact(dateTakenString.ToString(), "yyyy:MM:dd HH:mm:ss",
            CultureInfo.InvariantCulture);
        var exposureTimeString = exifProfile.GetValue(ExifTag.ExposureTime);
        var gps = exifProfile.GetValue(ExifTag.GPSTrack);

        //todo rest properties and null handling
        var result = new PhotoMetadata
        {
            DateTaken = dateTaken,
            FStop = fStopString?.Value.ToString(),
            Iso = isoString?.Value.ToString()
        };

        return result;
    }
}