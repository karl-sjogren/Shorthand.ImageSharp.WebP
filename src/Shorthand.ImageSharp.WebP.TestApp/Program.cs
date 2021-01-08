using System;
using System.IO;
using SixLabors.ImageSharp;

namespace Shorthand.ImageSharp.WebP.TestApp {
    public static class Program {
        public static void Main() {
            Directory.CreateDirectory("output");

            var filenames = new[] { "pexels-naushil-ansari-638738.jpg", "pexels-pok-rie-5696873.jpg", "pexels-public-domain-pictures-40984.jpg", "test-24.png", "blue-marble.jpg" };

            foreach(var filename in filenames) {
                File.Copy(Path.Combine("sample-files", filename), Path.Combine("output", filename), true);
                ConvertImage(Path.Combine("sample-files", filename), Path.Combine("output", filename + ".webp"));
                ConvertImage(Path.Combine("sample-files", filename), Path.Combine("output", filename + "-00.webp"), 0);
                ConvertImage(Path.Combine("sample-files", filename), Path.Combine("output", filename + "-20.webp"), 20);
                ConvertImage(Path.Combine("sample-files", filename), Path.Combine("output", filename + "-40.webp"), 40);
                ConvertImage(Path.Combine("sample-files", filename), Path.Combine("output", filename + "-60.webp"), 60);
                ConvertImage(Path.Combine("sample-files", filename), Path.Combine("output", filename + "-80.webp"), 80);
                ConvertImage(Path.Combine("sample-files", filename), Path.Combine("output", filename + "-99.webp"), 99);
            }
        }

        private static void ConvertImage(string inputPath, string outputPath, Int32? quality = null) {
            using var image = Image.Load(inputPath);
            using var ms = File.Create(outputPath);
            image.Save(ms, new WebPEncoder { Quality = quality });
        }
    }
}
