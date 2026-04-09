using System;
using System.IO;
using System.Text;
using Xunit;
using LearnSite.Store;

namespace SharpZipTests
{
    public class SharpZipTests : IDisposable
    {
        private string _tempDir;
        private string _zipFile;
        private string _extractDir;

        public SharpZipTests()
        {
#if NETCOREAPP || NET
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
            _tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(_tempDir);
            _zipFile = Path.Combine(_tempDir, "test.zip");
            _extractDir = Path.Combine(_tempDir, "extract") + "/";
        }

        public void Dispose()
        {
            if (Directory.Exists(_tempDir))
            {
                Directory.Delete(_tempDir, true);
            }
        }

        [Fact]
        public void UnpackFiles_ShouldExtractValidFiles()
        {
            // Arrange
            string sourceDir = Path.Combine(_tempDir, "source");
            Directory.CreateDirectory(sourceDir);
            File.WriteAllText(Path.Combine(sourceDir, "test.txt"), "Hello World");
            File.WriteAllText(Path.Combine(sourceDir, "Course.xml"), "XML Content"); // Should be skipped according to code
            File.WriteAllText(Path.Combine(sourceDir, "test.aspx"), "ASPX Content"); // Should be skipped according to code
            Directory.CreateDirectory(Path.Combine(sourceDir, "subfolder"));
            File.WriteAllText(Path.Combine(sourceDir, "subfolder", "sub.txt"), "Sub folder");

            SharpZip.PackFiles(_zipFile, sourceDir);

            // Act
            bool result = SharpZip.UnpackFiles(_zipFile, _extractDir);

            // Assert
            Assert.True(result);
            Assert.True(File.Exists(Path.Combine(_extractDir, "test.txt")));
            Assert.False(File.Exists(Path.Combine(_extractDir, "Course.xml")));
            Assert.False(File.Exists(Path.Combine(_extractDir, "test.aspx")));
            Assert.True(Directory.Exists(Path.Combine(_extractDir, "subfolder")));
            Assert.True(File.Exists(Path.Combine(_extractDir, "subfolder", "sub.txt")));
        }

        [Fact]
        public void UnpackFiles_ShouldHandleEmptyZip()
        {
            // Arrange
            string sourceDir = Path.Combine(_tempDir, "empty_source");
            Directory.CreateDirectory(sourceDir);

            SharpZip.PackFiles(_zipFile, sourceDir);

            // Act
            bool result = SharpZip.UnpackFiles(_zipFile, _extractDir);

            // Assert
            Assert.True(result);
            Assert.True(Directory.Exists(_extractDir));
        }

        [Fact]
        public void UnpackFiles_ShouldCreateDirectoryIfItDoesNotExist()
        {
            // Arrange
            string sourceDir = Path.Combine(_tempDir, "source2");
            Directory.CreateDirectory(sourceDir);
            File.WriteAllText(Path.Combine(sourceDir, "test.txt"), "Hello");
            SharpZip.PackFiles(_zipFile, sourceDir);

            string newExtractDir = Path.Combine(_tempDir, "new_extract") + "/";

            // Act
            bool result = SharpZip.UnpackFiles(_zipFile, newExtractDir);

            // Assert
            Assert.True(result);
            Assert.True(Directory.Exists(newExtractDir));
            Assert.True(File.Exists(Path.Combine(newExtractDir, "test.txt")));
        }

        [Fact]
        public void UnpackQuizFiles_ShouldSkipDatabaseFiles()
        {
            string sourceDir = Path.Combine(_tempDir, "quiz_source");
            Directory.CreateDirectory(sourceDir);
            File.WriteAllText(Path.Combine(sourceDir, "quiz.txt"), "Quiz");
            File.WriteAllText(Path.Combine(sourceDir, "quiz.db"), "Should skip");

            SharpZip.PackFiles(_zipFile, sourceDir);

            bool result = SharpZip.UnpackQuizFiles(_zipFile, _extractDir);

            Assert.True(result);
            Assert.True(File.Exists(Path.Combine(_extractDir, "quiz.txt")));
            Assert.False(File.Exists(Path.Combine(_extractDir, "quiz.db")));
        }

        [Fact]
        public void UnpackFilesXml_ShouldExtractOnlyCourseXml()
        {
            string sourceDir = Path.Combine(_tempDir, "xml_source");
            Directory.CreateDirectory(sourceDir);
            File.WriteAllText(Path.Combine(sourceDir, "Course.xml"), "<course />");
            File.WriteAllText(Path.Combine(sourceDir, "other.txt"), "Other");

            SharpZip.PackFiles(_zipFile, sourceDir);

            bool result = SharpZip.UnpackFilesXml(_zipFile, _extractDir);

            Assert.True(result);
            Assert.True(File.Exists(Path.Combine(_extractDir, "Course.xml")));
            Assert.False(File.Exists(Path.Combine(_extractDir, "other.txt")));
        }

        [Fact]
        public void UnpackFilesXml_WhenCourseXmlDoesNotExist_ReturnsFalse()
        {
            string sourceDir = Path.Combine(_tempDir, "no_xml_source");
            Directory.CreateDirectory(sourceDir);
            File.WriteAllText(Path.Combine(sourceDir, "other.txt"), "Other");

            SharpZip.PackFiles(_zipFile, sourceDir);

            bool result = SharpZip.UnpackFilesXml(_zipFile, _extractDir);

            Assert.False(result);
        }
    }
}
