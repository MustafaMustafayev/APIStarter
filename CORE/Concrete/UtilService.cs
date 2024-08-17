using CORE.Abstract;
using CORE.Enums;
using Microsoft.AspNetCore.Hosting;

namespace CORE.Concrete;

public class UtilService : IUtilService
{
    private readonly IWebHostEnvironment _environment;
    public UtilService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public string CreateGuid()
    {
        return Guid.NewGuid().ToString();
    }

    public string GetFolderName(EFileType type)
    {
        return type switch
        {
            EFileType.UserImages => @"files\images\user_profile",
            _ => "files/error"
        };
    }

    public string GetEnvFolderPath(string folderName)
    {
        return Path.Combine(_environment.WebRootPath, folderName);
    }
}