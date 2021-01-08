using System;
using System.Buffers;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Memory;
using SixLabors.ImageSharp.PixelFormats;

namespace Shorthand.ImageSharp.WebP {
    public class WebPEncoder : IImageEncoder {
        public static WebPEncoder Instance { get; } = new WebPEncoder();

        //
        // Summary:
        //     Gets or sets the quality, that will be used to encode the image. Quality index
        //     must be between 0 and 100 (compression from max to min). Defaults to null (lossless).
        public Int32? Quality { get; set; }

        public void Encode<TPixel>(Image<TPixel> image, Stream stream) where TPixel : unmanaged, IPixel<TPixel> {
            var memoryAllocator = SixLabors.ImageSharp.Configuration.Default.MemoryAllocator;

            image.TryGetSinglePixelSpan(out var pixelData);
            var buffer = MemoryMarshal.AsBytes(pixelData).ToArray();

            var pinnedArray = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            var pointer = pinnedArray.AddrOfPinnedObject();

            var resultPointer = IntPtr.Zero;
            Int32 resultSize;
            IManagedByteBuffer managedBuffer = null;

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

                managedBuffer = memoryAllocator.AllocateManagedByteBuffer(resultSize);
                buffer = managedBuffer.Array;
                Marshal.Copy(resultPointer, buffer, 0, resultSize);

                using var ms = new MemoryStream(buffer, 0, resultSize);
                ms.CopyTo(stream);
            } finally {
                pinnedArray.Free();
                managedBuffer?.Dispose();

                if(resultPointer != IntPtr.Zero)
                    NativeLibrary.WebPFree(resultPointer);
            }
        }

        public Task EncodeAsync<TPixel>(Image<TPixel> image, Stream stream, CancellationToken cancellationToken) where TPixel : unmanaged, IPixel<TPixel> {
            Encode(image, stream);
            return Task.CompletedTask;
        }
    }
}
