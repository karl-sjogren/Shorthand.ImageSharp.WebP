﻿using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Shorthand.ImageSharp.WebP.Benchmarks;

public static class Program {
    public static void Main() {
        _ = BenchmarkRunner.Run<EncodeImageBenchmark>();
    }

    [MemoryDiagnoser]
    public class EncodeImageBenchmark {
        private readonly Image _image1;
        private readonly Image _image2;
        private readonly Image _image3;

        public EncodeImageBenchmark() {
            Configuration.Default.PreferContiguousImageBuffers = true;

            _image1 = new Image<Rgba32>(4096, 4096);
            _image1.Mutate(x => x.BackgroundColor(Rgba32.ParseHex("FF6347")));

            _image2 = Image.Load(Path.Combine("sample-files", "pexels-naushil-ansari-638738.jpg"));
            _image3 = Image.Load(Path.Combine("sample-files", "pexels-pok-rie-5696873.jpg"));
        }

        [Benchmark]
        public void EncodeImageNatively() {
            using(var ms = new MemoryStream())
                _image1.Save(ms, new WebPNativeEncoder { Quality = 80 });

            using(var ms = new MemoryStream())
                _image2.Save(ms, new WebPNativeEncoder { Quality = 80 });

            using(var ms = new MemoryStream())
                _image3.Save(ms, new WebPNativeEncoder { Quality = 80 });
        }

        [Benchmark]
        public void EncodeImageNativelyUnsafe() {
            using(var ms = new MemoryStream())
                _image1.Save(ms, new WebPNativeEncoder { Quality = 80 });

            using(var ms = new MemoryStream())
                _image2.Save(ms, new WebPNativeEncoder { Quality = 80 });

            using(var ms = new MemoryStream())
                _image3.Save(ms, new WebPNativeEncoder { Quality = 80 });
        }

        [Benchmark]
        public void EncodeImageNativelyManageAllocations() {
            using(var ms = new MemoryStream())
                _image1.Save(ms, new WebPNativeEncoderManagedAllocations { Quality = 80 });

            using(var ms = new MemoryStream())
                _image2.Save(ms, new WebPNativeEncoderManagedAllocations { Quality = 80 });

            using(var ms = new MemoryStream())
                _image3.Save(ms, new WebPNativeEncoderManagedAllocations { Quality = 80 });
        }

        [Benchmark]
        public void EncodeImageBuiltin() {
            using(var ms = new MemoryStream())
                _image1.Save(ms, new WebpEncoder { Quality = 80 });

            //using(var ms = new MemoryStream())
            //    _image2.Save(ms, new WebpEncoder { Quality = 80 });

            //using(var ms = new MemoryStream())
            //    _image3.Save(ms, new WebpEncoder { Quality = 80 });
        }
    }
}
