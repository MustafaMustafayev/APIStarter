using CORE.Abstract;
using CORE.Config;
using System.Security.Cryptography;
using System.Text;

namespace CORE.Concrete;
public class EncryptionService : IEncryptionService
{
    private readonly ConfigSettings _config;

    public EncryptionService(ConfigSettings config)
    {
        _config = config;
    }

    public string Encrypt(string value)
    {
        var key = _config.CryptographySettings.KeyBase64;
        var privatekey = _config.CryptographySettings.VBase64;
        var privatekeyByte = Encoding.UTF8.GetBytes(privatekey);
        var keybyte = Encoding.UTF8.GetBytes(key);
        SymmetricAlgorithm algorithm = Aes.Create();
        var transform = algorithm.CreateEncryptor(keybyte, privatekeyByte);
        var inputbuffer = Encoding.Unicode.GetBytes(value);
        var outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
        return Convert.ToBase64String(outputBuffer);
    }

    public string Decrypt(string value)
    {
        var key = _config.CryptographySettings.KeyBase64;
        var privatekey = _config.CryptographySettings.VBase64;
        var privatekeyByte = Encoding.UTF8.GetBytes(privatekey);
        var keybyte = Encoding.UTF8.GetBytes(key);
        SymmetricAlgorithm algorithm = Aes.Create();
        var transform = algorithm.CreateDecryptor(keybyte, privatekeyByte);
        var inputbuffer = Convert.FromBase64String(value);
        var outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
        return Encoding.Unicode.GetString(outputBuffer);
    }
}