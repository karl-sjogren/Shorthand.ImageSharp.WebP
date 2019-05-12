using System;
using System.Collections.Generic;
using SixLabors.ImageSharp.Formats;

namespace Shorthand.ImageSharp.WebP {
    public class WebPFormat : IImageFormat {
        public string Name => "WebP";

        public string DefaultMimeType => "image/webp";

        public IEnumerable<string> MimeTypes => WebPConstants.MimeTypes;

        public IEnumerable<string> FileExtensions => WebPConstants.FileExtensions;
    }
}
