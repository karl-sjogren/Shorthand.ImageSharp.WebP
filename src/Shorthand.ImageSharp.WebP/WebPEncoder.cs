using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;

namespace Shorthand.ImageSharp.WebP
{
    public class WebPEncoder : IImageEncoder {
        public static WebPEncoder Instance { get; } = new WebPEncoder();

        public void Encode<TPixel>(Image<TPixel> image, Stream stream) where TPixel : unmanaged, IPixel<TPixel> {
            var psi = new ProcessStartInfo {
                FileName = Native.CWebP,
                Arguments = "-o - -- -",
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            };

            var tmpStream = new MemoryStream();
            try {
                image.SaveAsPng(tmpStream);
                tmpStream.Seek(0, SeekOrigin.Begin);

                var process = Process.Start(psi);

                tmpStream.CopyTo(process.StandardInput.BaseStream);
                process.StandardInput.Close();

                var outputStream = process.StandardOutput.BaseStream;
                outputStream.CopyTo(stream);
            } finally {
                tmpStream.Dispose();
            }
        }

        public Task EncodeAsync<TPixel>(Image<TPixel> image, Stream stream) where TPixel : unmanaged, IPixel<TPixel> {
            Encode(image, stream);
            return Task.CompletedTask;
        }
    }
}
