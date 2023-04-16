using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;

namespace Shorthand.ImageSharp.WebP;

public class WebPNativeEncoderUnsafe : IImageEncoder {
    public static WebPNativeEncoder Instance { get; } = new WebPNativeEncoder();

    //
    // Summary:
    //     Gets or sets the quality, that will be used to encode the image. Quality index
    //     must be between 0 and 100 (compression from max to min). Defaults to null (lossless).
    public Int32? Quality { get; set; }

    public void Encode<TPixel>(Image<TPixel> image, Stream stream) where TPixel : unmanaged, IPixel<TPixel> {
        var memoryAllocator = Configuration.Default.MemoryAllocator;

        image.DangerousTryGetSinglePixelMemory(out var pixelData);
        unsafe {
            using var pinHandle = pixelData.Pin();
            var pointer = new IntPtr(pinHandle.Pointer);

            var resultPointer = IntPtr.Zero;
            Int32 resultSize;

            try {
                if(Quality.HasValue) {
                    var quality = Convert.ToSingle(Quality.Value);
                    if(image.PixelType.BitsPerPixel == 32) {
                        resultSize = NativeLibrary.WebPEncodeRGBA(pointer, image.Width, image.Height, image.Width * 4, quality, out resultPointer);
                    } else if(image.PixelType.BitsPerPixel == 24) {
                        resultSize = NativeLibrary.WebPEncodeRGB(pointer, image.Width, image.Height, image.Width * 3, quality, out resultPointer);
                    } else {
                        throw new InvalidOperationException("Invalid bits per pixel for webp. Use Rgba32 or Rgb24.");
                    }
                } else {
                    if(image.PixelType.BitsPerPixel == 32) {
                        resultSize = NativeLibrary.WebPEncodeLosslessRGBA(pointer, image.Width, image.Height, image.Width * 4, out resultPointer);
                    } else if(image.PixelType.BitsPerPixel == 24) {
                        resultSize = NativeLibrary.WebPEncodeLosslessRGB(pointer, image.Width, image.Height, image.Width * 3, out resultPointer);
                    } else {
                        throw new InvalidOperationException("Invalid bits per pixel for webp. Use Rgba32 or Rgb24.");
                    }
                }

                using var managedBuffer = memoryAllocator.Allocate<byte>(resultSize);
                var resultBuffer = managedBuffer.Memory.ToArray();
                Marshal.Copy(resultPointer, resultBuffer, 0, resultSize);

                using var ms = new MemoryStream(resultBuffer, 0, resultSize);
                ms.CopyTo(stream);
            } finally {
                if(resultPointer != IntPtr.Zero)
                    NativeLibrary.WebPFree(resultPointer);
            }
        }
    }

    public Task EncodeAsync<TPixel>(Image<TPixel> image, Stream stream, CancellationToken cancellationToken) where TPixel : unmanaged, IPixel<TPixel> {
        Encode(image, stream);
        return Task.CompletedTask;
    }
}
