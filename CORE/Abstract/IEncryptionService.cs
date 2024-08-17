namespace CORE.Abstract;
public interface IEncryptionService
{
    public string Encrypt(string value);
    public string Decrypt(string value);
}
