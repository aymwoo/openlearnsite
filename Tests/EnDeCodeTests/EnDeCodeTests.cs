using System;
using Xunit;
using LearnSite.Common;

namespace EnDeCodeTests
{
    public class EnDeCodeTests
    {
        [Fact]
        public void Encrypt_ValidInput_ReturnsEncryptedString()
        {
            // Arrange
            string pToEncrypt = "HelloWorld";
            string sKey = "12"; // Key has to be 8 bytes when combined with keyset (123456) => "12123456"

            // Act
            string encrypted = EnDeCode.Encrypt(pToEncrypt, sKey);

            // Assert
            Assert.False(string.IsNullOrEmpty(encrypted));
            Assert.NotEqual(pToEncrypt, encrypted);
        }

        [Fact]
        public void Decrypt_ValidInput_ReturnsDecryptedString()
        {
            // Arrange
            string originalText = "HelloWorld";
            string sKey = "12"; // 2 bytes + 6 bytes keyset = 8 bytes

            // Act
            string encrypted = EnDeCode.Encrypt(originalText, sKey);
            string decrypted = EnDeCode.Decrypt(encrypted, sKey);

            // Assert
            Assert.Equal(originalText, decrypted);
        }

        [Fact]
        public void Encrypt_EmptyString_ReturnsEncryptedString()
        {
            // Arrange
            string pToEncrypt = "";
            string sKey = "12";

            // Act
            string encrypted = EnDeCode.Encrypt(pToEncrypt, sKey);

            // Assert
            Assert.False(string.IsNullOrEmpty(encrypted));
            Assert.NotEqual(pToEncrypt, encrypted);

            string decrypted = EnDeCode.Decrypt(encrypted, sKey);
            Assert.Equal(pToEncrypt, decrypted);
        }

        [Fact]
        public void Encrypt_SpecialCharacters_ReturnsEncryptedString()
        {
            // Arrange
            string pToEncrypt = "!@#$%^&*()_+";
            string sKey = "ab";

            // Act
            string encrypted = EnDeCode.Encrypt(pToEncrypt, sKey);

            // Assert
            Assert.False(string.IsNullOrEmpty(encrypted));
            Assert.NotEqual(pToEncrypt, encrypted);

            string decrypted = EnDeCode.Decrypt(encrypted, sKey);
            Assert.Equal(pToEncrypt, decrypted);
        }

        [Fact]
        public void Encrypt_ChineseCharacters_ReturnsEncryptedString()
        {
            // Arrange
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance); // Register for GB2312 or Default encoding if it's running in modern .NET
            string pToEncrypt = "你好，世界！";
            string sKey = "34";

            // Act
            string encrypted = EnDeCode.Encrypt(pToEncrypt, sKey);

            // Assert
            Assert.False(string.IsNullOrEmpty(encrypted));
            Assert.NotEqual(pToEncrypt, encrypted);

            string decrypted = EnDeCode.Decrypt(encrypted, sKey);
            Assert.Equal(pToEncrypt, decrypted);
        }

        [Fact]
        public void Decrypt_InvalidEncryptedString_ThrowsException()
        {
            // Arrange
            string invalidEncryptedText = "NotHexadecimalString";
            string sKey = "12";

            // Act & Assert
            Assert.ThrowsAny<Exception>(() => EnDeCode.Decrypt(invalidEncryptedText, sKey));
        }

        [Fact]
        public void Encrypt_KeyTooLong_ThrowsException()
        {
            // Arrange
            string pToEncrypt = "HelloWorld";
            string sKey = "123456789"; // Too long

            // Act & Assert
            // The method might throw CryptographicException due to wrong key size
            Assert.ThrowsAny<Exception>(() => EnDeCode.Encrypt(pToEncrypt, sKey));
        }

        [Fact]
        public void Encrypt_KeyTooShort_ThrowsException()
        {
            // Arrange
            string pToEncrypt = "HelloWorld";
            string sKey = "1"; // Too short

            // Act & Assert
            Assert.ThrowsAny<Exception>(() => EnDeCode.Encrypt(pToEncrypt, sKey));
        }

        [Fact]
        public void Encrypt_SameInputTwice_WithSameKey_ReturnsSameCipherText()
        {
            string originalText = "RepeatableContent";
            string sKey = "56";

            string encrypted1 = EnDeCode.Encrypt(originalText, sKey);
            string encrypted2 = EnDeCode.Encrypt(originalText, sKey);

            Assert.Equal(encrypted1, encrypted2);
        }

        [Fact]
        public void Decrypt_LowercaseHexCipherText_ReturnsOriginalText()
        {
            string originalText = "CaseInsensitiveHex";
            string sKey = "78";

            string encrypted = EnDeCode.Encrypt(originalText, sKey).ToLowerInvariant();
            string decrypted = EnDeCode.Decrypt(encrypted, sKey);

            Assert.Equal(originalText, decrypted);
        }

        [Fact]
        public void Encrypt_WhitespaceContent_RoundTripsSuccessfully()
        {
            string originalText = " line1\r\n\tline2 ";
            string sKey = "90";

            string encrypted = EnDeCode.Encrypt(originalText, sKey);
            string decrypted = EnDeCode.Decrypt(encrypted, sKey);

            Assert.Equal(originalText, decrypted);
        }
    }
}
