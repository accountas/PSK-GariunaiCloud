using System.Diagnostics;
using System.Security.Cryptography;
using GariunaiCloud_ToolSharing.DataAccess;
using GariunaiCloud_ToolSharing.IServices;
using GariunaiCloud_ToolSharing.Models;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace GariunaiCloud_ToolSharing.Services;

public class ImageService : IImageService
{
    private readonly GariunaiDbContext _context;

    public ImageService(GariunaiDbContext context)
    {
        _context = context;
    }

    public Task<DbImage?> GetImageAsync(string imageName)
    {
        return _context.Images.FirstOrDefaultAsync(i => i.Name == imageName);
    }

    public async Task<DbImage> UploadImageAsync(byte[] imageData)
    {
        var image = Image.Load<Rgba32>(imageData);
        image.Mutate(i => i.Resize(0, Math.Min(480, image.Height)));
        using var ms = new MemoryStream();
        await image.SaveAsync(ms, new JpegEncoder());

        var dbImage = new DbImage
        {
            Name = Guid.NewGuid().ToString(),
            ImageData = ms.ToArray()
        };

        Console.WriteLine("Image uploaded");
        await _context.Images.AddAsync(dbImage);
        await _context.SaveChangesAsync();
        Console.WriteLine("Image uploaded");
        return dbImage;
    }

    public Task DeleteImageAsync(string imageName)
    {
        var image = _context.Images.FirstOrDefault(i => i.Name == imageName);
        if (image == null) return Task.CompletedTask;
        _context.Images.Remove(image);
        return _context.SaveChangesAsync();
    }
}