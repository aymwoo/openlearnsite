using System;
using System.IO;
using Xunit;
using LearnSite.Common;

namespace ImageCheckTests
{
    public class ImageCheckTests : IDisposable
    {
        private string _tempDir;

        public ImageCheckTests()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), "ImageCheckTests_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempDir);
        }

        public void Dispose()
        {
            if (Directory.Exists(_tempDir))
            {
                Directory.Delete(_tempDir, true);
            }
        }

        [Fact]
        public void CheckImageType_ValidPng_ReturnsTrue()
        {
            string filePath = Path.Combine(_tempDir, "valid.png");
            byte[] pngHeader = new byte[] { 0x89, 0x50, 0x4E, 0x47 };
            File.WriteAllBytes(filePath, pngHeader);

            bool result = ImageCheck.CheckImageType(filePath);

            Assert.True(result);
        }

        [Fact]
        public void CheckImageType_ValidJpg_ReturnsTrue()
        {
            string filePath = Path.Combine(_tempDir, "valid.jpg");
            byte[] jpgHeader = new byte[] { 0xFF, 0xD8 };
            File.WriteAllBytes(filePath, jpgHeader);

            bool result = ImageCheck.CheckImageType(filePath);

            Assert.True(result);
        }

        [Fact]
        public void CheckImageType_InvalidImage_ReturnsFalse()
        {
            string filePath = Path.Combine(_tempDir, "invalid.txt");
            byte[] invalidHeader = new byte[] { 0x61, 0x62, 0x63, 0x64 }; // "abcd"
            File.WriteAllBytes(filePath, invalidHeader);

            bool result = ImageCheck.CheckImageType(filePath);

            Assert.False(result);
        }

        [Fact]
        public void CheckImageType_EmptyFile_ReturnsFalse()
        {
            string filePath = Path.Combine(_tempDir, "empty.bin");
            File.WriteAllBytes(filePath, Array.Empty<byte>());

            bool result = ImageCheck.CheckImageType(filePath);

            Assert.False(result);
        }

        [Fact]
        public void CheckImageType_OneByteFile_ReturnsFalse()
        {
            string filePath = Path.Combine(_tempDir, "onebyte.bin");
            byte[] oneByte = new byte[] { 0xFF };
            File.WriteAllBytes(filePath, oneByte);

            bool result = ImageCheck.CheckImageType(filePath);

            Assert.False(result);
        }

        [Fact]
        public void CheckImageType_NonExistentFile_ReturnsFalse()
        {
            string filePath = Path.Combine(_tempDir, "nonexistent.png");

            bool result = ImageCheck.CheckImageType(filePath);

            Assert.False(result);
        }

        [Fact]
        public void CheckImageType_ByteArrayNull_ReturnsFalse()
        {
            Assert.False(ImageCheck.CheckImageType((byte[])null));
        }

        [Fact]
        public void CheckImageType_ByteArrayPngHeader_ReturnsTrue()
        {
            byte[] pngHeader = new byte[] { 0x89, 0x50 };

            bool result = ImageCheck.CheckImageType(pngHeader);

            Assert.True(result);
        }

        [Fact]
        public void CheckImageType_ByteArrayUnknownHeader_ReturnsFalse()
        {
            byte[] header = new byte[] { 0x01, 0x02 };

            bool result = ImageCheck.CheckImageType(header);

            Assert.False(result);
        }

    }
}
