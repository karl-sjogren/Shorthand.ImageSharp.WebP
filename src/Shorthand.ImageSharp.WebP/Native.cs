using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Shorthand.ImageSharp.WebP {
    public static class Native {
        public static string CWebP => Path.Combine("native", OSFolder, "cwebp") + ExecutableExtension;
        public static string LibWebP => Path.Combine("native", OSFolder, "libwebp") + LibraryExtension;

        private static string OSFolder {
            get {
                if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    return "osx";

                if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    return "linux";

                if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                    if(Environment.Is64BitProcess)
                        return "win-x64";

                    return "win-x86";
                }

                throw new InvalidOperationException("Somehow this system seems unsupported.");
            }
        }

        private static string ExecutableExtension {
            get {
                if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    return string.Empty;

                if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    return string.Empty;

                if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    return ".exe";

                throw new InvalidOperationException("Somehow this system seems unsupported.");
            }
        }

        private static string LibraryExtension {
            get {
                if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    return string.Empty;

                if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    return string.Empty;

                if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    return ".dll";

                throw new InvalidOperationException("Somehow this system seems unsupported.");
            }
        }
    }
}
