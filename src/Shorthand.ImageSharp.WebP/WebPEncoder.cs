using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;

namespace Shorthand.ImageSharp.WebP
{
    public class WebPEncoder : IImageEncoder {
        public static WebPEncoder Instance { get; } = new WebPEncoder();

        //
        // Summary:
        //     Gets or sets the quality, that will be used to encode the image. Quality index
        //     must be between 0 and 100 (compression from max to min). Defaults to null (lossless).
        public Int32? Quality { get; set; }

        public void Encode<TPixel>(Image<TPixel> image, Stream stream) where TPixel : unmanaged, IPixel<TPixel> {
            var processArguments = "-o - -- -";

            if(Quality.HasValue) {
                processArguments = $"-q {Quality.Value.ToString(CultureInfo.InvariantCulture)} -o - -- -";
            }

            var psi = new ProcessStartInfo {
                FileName = Native.CWebP,
                Arguments = processArguments,
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

        public async Task EncodeAsync<TPixel>(Image<TPixel> image, Stream stream, CancellationToken cancellationToken) where TPixel : unmanaged, IPixel<TPixel> {
            var processArguments = "-o - -- -";

            if(Quality.HasValue) {
                processArguments = $"-q {Quality.Value.ToString(CultureInfo.InvariantCulture)} -o - -- -";
            }

            var psi = new ProcessStartInfo {
                FileName = Native.CWebP,
                Arguments = processArguments,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            };

            var tmpStream = new MemoryStream();
            try {
                image.SaveAsPng(tmpStream);
                tmpStream.Seek(0, SeekOrigin.Begin);

                var process = Process.Start(psi);

                await tmpStream.CopyToAsync(process.StandardInput.BaseStream).ConfigureAwait(false);
                process.StandardInput.Close();

                var outputStream = process.StandardOutput.BaseStream;
                await outputStream.CopyToAsync(stream).ConfigureAwait(false);
            } finally {
                tmpStream.Dispose();
            }
        }
    }
}
