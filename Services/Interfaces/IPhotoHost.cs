using Microsoft.Extensions.FileProviders;

namespace WebStore.Data;

public interface IPhotoHost
{
    public string UploadPhoto(IFormFile file);
    public Boolean DeletePhoto(string photoUrl);
    public string GetPhotoName(string photoUrl);
}