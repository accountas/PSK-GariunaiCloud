using System.Diagnostics;
using System.Drawing;
using System.Security.Cryptography;
using GariunaiCloud_ToolSharing.DataAccess;
using GariunaiCloud_ToolSharing.IServices;
using GariunaiCloud_ToolSharing.Models;
using Microsoft.EntityFrameworkCore;

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
        var md5 = MD5.Create();
        var hash = md5.ComputeHash(imageData);
        var hashString = BitConverter
            .ToString(hash)
            .Replace("-", "")
            .ToLower()[..10];
        
        var image = new DbImage
        {
            Name = hashString,
            ImageData = imageData
        };
        
        Console.WriteLine("Image uploaded");
        await _context.Images.AddAsync(image);
        await _context.SaveChangesAsync();
        Console.WriteLine("Image uploaded");
        return image;
    }

    public Task DeleteImageAsync(string imageName)
    {
        var image = _context.Images.FirstOrDefault(i => i.Name == imageName);
        if (image == null) return Task.CompletedTask;
        _context.Images.Remove(image);
        return _context.SaveChangesAsync();
    }
}