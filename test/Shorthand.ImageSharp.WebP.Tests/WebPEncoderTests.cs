using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Xunit;

namespace Shorthand.ImageSharp.WebP.Tests {
    public class WebPEncoderTests {
        [Fact]
        public void EncodeSimpleImage() {
            using (var image = new Image<Rgba32>(20, 20)) {
                image.Mutate(x => x.BackgroundColor(Rgba32.ParseHex("FF6347")));
                using(var ms = new MemoryStream()) {
                    image.Save(ms, new WebPEncoder());

                    Assert.True(ms.Length > 0, "Output stream should not be empty.");
                    Assert.InRange(ms.Length, 60, 100);
                }
            }
        }

        [Fact]
        public void EncodeFromFile() {
            using (var image = Image.Load("Resources/test.jpg")) {
                using(var ms = new MemoryStream()) {
                    image.Save(ms, new WebPEncoder());

                    Assert.True(ms.Length > 0, "Output stream should not be empty.");
                    Assert.InRange(ms.Length, 33000, 34000);
                }
            }
        }
    }
}
