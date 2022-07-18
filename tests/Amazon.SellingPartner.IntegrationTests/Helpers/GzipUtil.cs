using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Amazon.SellingPartner.IntegrationTests.Helpers
{
    public static class GzipUtil
    {
        public static async Task CompressAsync(string fileToCompress)
        {
            await CompressAsync(new FileInfo(fileToCompress));
        }

        public static async Task CompressAsync(FileInfo fileToCompress)
        {
            await using FileStream originalFileStream = fileToCompress.OpenRead();
            if ((File.GetAttributes(fileToCompress.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
            {
                await using FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz");
                await using GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress);
                await originalFileStream.CopyToAsync(compressionStream);
            }
        }

        public static async Task DecompressAsync(string fileToDecompress)
        {
            await DecompressAsync(new FileInfo(fileToDecompress));
        }

        public static async Task DecompressAsync(FileInfo fileToDecompress)
        {
            await using FileStream originalFileStream = fileToDecompress.OpenRead();
            var currentFileName = fileToDecompress.FullName;
            var newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

            await using FileStream decompressedFileStream = File.Create(newFileName);
            await using GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress);
            await decompressionStream.CopyToAsync(decompressedFileStream);
        }
    }
}