using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using SixLabors.ImageSharp.Formats;

namespace Shorthand.ImageSharp.WebP {
    public static class Native {

        public static string CWebP => Path.Combine("native", OSFolder, "cwebp");

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
        
    }
}
