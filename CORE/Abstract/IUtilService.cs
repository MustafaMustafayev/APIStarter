using CORE.Enums;

namespace CORE.Abstract;

public interface IUtilService
{
    public string CreateGuid();
    public string GetFolderName(EFileType type);
    string GetEnvFolderPath(string folderName);
}