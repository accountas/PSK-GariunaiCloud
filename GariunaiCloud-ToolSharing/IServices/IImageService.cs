using GariunaiCloud_ToolSharing.Models;

namespace GariunaiCloud_ToolSharing.IServices;

public interface IImageService
{
    public Task<DbImage?> GetImageAsync(string imageName);
    public Task<DbImage> UploadImageAsync(byte[] imageData);
    public Task DeleteImageAsync(string imageName);
}