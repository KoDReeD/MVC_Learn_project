using System.Configuration;
using System.Net;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.FileProviders;
using WebStore.Data;
using ConfigurationManager = Microsoft.Extensions.Configuration.ConfigurationManager;

namespace WebStore.Services.Implements;

public class CloudinaryPhotoHost : IPhotoHost
{
    private static ConfigurationManager _config = Helper.Config;
    private static IConfigurationSection _cloudinarySection = _config.GetSection("Cloudinary");

    private static Account account = new Account(
        _cloudinarySection["cloud"],
        _cloudinarySection["apiKey"],
        _cloudinarySection["apiSecret"]);

    private static Cloudinary cloudinary = new Cloudinary(account);

    public string UploadPhoto(IFormFile file)
    {
        // Загрузка фото
        if (file != null)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                PublicId = Guid.NewGuid().ToString()
            };

            // Загружаем изображение в Cloudinary
            var uploadResult = cloudinary.Upload(uploadParams);

            // Доступ к данным загруженного изображения
            if (uploadResult.Error != null)
            {
                var imageUrl = uploadResult.SecureUrl.ToString();
                return imageUrl;
            }
            else
            {
                return null;
            }
        }

        return null;
    }

    public Boolean DeletePhoto(string PhotoUrl)
    {
        string publicId = GetPhotoName(PhotoUrl);
        DeletionParams deletionParams = new DeletionParams(publicId);
        DeletionResult deletionResult = cloudinary.Destroy(deletionParams);

        if (deletionResult.StatusCode != HttpStatusCode.Redirect)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public string GetPhotoName(string photoUrl)
    {
        string photoName = Path.GetFileName(photoUrl);
        return photoName;
    }
}