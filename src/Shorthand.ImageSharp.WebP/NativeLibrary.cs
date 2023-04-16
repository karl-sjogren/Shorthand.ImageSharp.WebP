using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Shorthand.ImageSharp.WebP;

public static class NativeLibrary {
    static NativeLibrary() {
        if(File.Exists("libwebp" + LibraryExtension))
            File.Delete("libwebp" + LibraryExtension);

        if(File.Exists("libsharpyuv" + LibraryExtension))
            File.Delete("libsharpyuv" + LibraryExtension);

        if(File.Exists(MainLibPath))
            File.Copy(MainLibPath, "libwebp" + LibraryExtension);

        if(File.Exists(LibYUVPath))
            File.Copy(LibYUVPath, "libsharpyuv" + LibraryExtension);
    }

    public static string MainLibPath => Path.Combine("native", OSFolder, "libwebp" + LibraryExtension);
    public static string LibYUVPath => Path.Combine("native", OSFolder, "libsharpyuv" + LibraryExtension);

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

    private static string LibraryExtension {
        get {
            if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return string.Empty;

            if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return ".so";

            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return ".dll";

            throw new InvalidOperationException("Somehow this system seems unsupported.");
        }
    }

    [DllImport("libwebp", CallingConvention = CallingConvention.Cdecl, EntryPoint = "WebPEncodeRGB")]
    public static extern Int32 WebPEncodeRGB(IntPtr imagePointer, Int32 width, Int32 height, Int32 stride, float quality, out IntPtr output);

    [DllImport("libwebp", CallingConvention = CallingConvention.Cdecl, EntryPoint = "WebPEncodeRGBA")]
    public static extern Int32 WebPEncodeRGBA(IntPtr imagePointer, Int32 width, Int32 height, Int32 stride, float quality, out IntPtr output);

    [DllImport("libwebp", CallingConvention = CallingConvention.Cdecl, EntryPoint = "WebPEncodeLosslessRGB")]
    public static extern Int32 WebPEncodeLosslessRGB(IntPtr imagePointer, Int32 width, Int32 height, Int32 stride, out IntPtr output);

    [DllImport("libwebp", CallingConvention = CallingConvention.Cdecl, EntryPoint = "WebPEncodeLosslessRGBA")]
    public static extern Int32 WebPEncodeLosslessRGBA(IntPtr imagePointer, Int32 width, Int32 height, Int32 stride, out IntPtr output);

    [DllImport("libwebp", CallingConvention = CallingConvention.Cdecl, EntryPoint = "WebPFree")]
    public static extern Int32 WebPFree(IntPtr pointer);
}
