
namespace Varghese_Demo.API.Cryptography
{
    /// <summary>
    /// To encrypt and decrypt a string
    /// </summary>
    public interface ICryptography
    {
        /// <summary>
        /// Encryption
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        string Encrypt(string item);

        /// <summary>
        /// Decryption
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        string Decrypt(string item);
    }
}
