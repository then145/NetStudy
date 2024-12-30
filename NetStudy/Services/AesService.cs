using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NetStudy.Services
{
    public class AesService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;
       

        public AesService()
        {

        }

        public AesService(byte[] key, byte[] iv)
        {
            _key = key;
            _iv = iv;
        }
        public string EncryptMessage(string plainText, string key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = GetKeyBytes(key);
                aes.GenerateIV();

                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                    string encryptedText = Convert.ToBase64String(aes.IV) + ":" + Convert.ToBase64String(encryptedBytes);
                    return encryptedText;
                }
            }
        }
        public string DecryptMessage(string encryptedText, string key)
        {
            string[] parts = encryptedText.Split(':');
            if (parts.Length != 2)
            {
                throw new FormatException("Dữ liệu mã hóa không hợp lệ.");
            }

            byte[] iv = Convert.FromBase64String(parts[0]);
            byte[] encryptedBytes = Convert.FromBase64String(parts[1]);

            using (Aes aes = Aes.Create())
            {
                aes.Key = GetKeyBytes(key);
                aes.IV = iv;

                using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }
        private byte[] GetKeyBytes(string key)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
            }
        }

        public byte[] GetKey() => _key;
        public byte[] GetIV() => _iv;

        public string EncryptPrivateKey(string privateKey, string password, out string salt)
        {
            // Tạo salt ngẫu nhiên
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            salt = Convert.ToBase64String(saltBytes);

            // Tạo khóa AES từ mật khẩu và salt
            byte[] aesKey = DeriveKeyFromPassword(password, saltBytes, 32);

            using (Aes aes = Aes.Create())
            {
                aes.Key = aesKey;
                aes.GenerateIV();

                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    byte[] privateKeyBytes = Encoding.UTF8.GetBytes(privateKey);
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(privateKeyBytes, 0, privateKeyBytes.Length);

                    // Lưu IV cùng dữ liệu mã hóa
                    string encryptedPrivateKey = Convert.ToBase64String(aes.IV) + ":" + Convert.ToBase64String(encryptedBytes);
                    return encryptedPrivateKey;
                }
            }
        }

        public string DecryptPrivateKey(string encryptedPrivateKey, string password, string salt)
        {
            // Tách IV và dữ liệu mã hóa
            string[] parts = encryptedPrivateKey.Split(':');
            if (parts.Length != 2)
            {
                throw new FormatException("Dữ liệu mã hóa không hợp lệ.");
            }

            byte[] iv = Convert.FromBase64String(parts[0]);
            byte[] encryptedBytes = Convert.FromBase64String(parts[1]);

            // Chuyển salt từ Base64
            byte[] saltBytes = Convert.FromBase64String(salt);

            // Tạo khóa AES từ mật khẩu và salt
            byte[] aesKey = DeriveKeyFromPassword(password, saltBytes, 32);

            using (Aes aes = Aes.Create())
            {
                aes.Key = aesKey;
                aes.IV = iv;

                using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }

        private byte[] DeriveKeyFromPassword(string password, byte[] salt, int keySize)
        {
            using (var rfc2898 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                return rfc2898.GetBytes(keySize);
            }
        }

        public string GenerateAesKey()
        {

            using var aes = Aes.Create();
            aes.GenerateKey();
            return Convert.ToBase64String(aes.Key);

        }
    }
}
