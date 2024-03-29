using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Shorthand.ImageSharp.WebP.Tests;

public class WebPNativeEncoderTests {
    [Fact]
    public void EncodeSimpleImage() {
        using var image = new Image<Rgba32>(20, 20);
        image.Mutate(x => x.BackgroundColor(Rgba32.ParseHex("FF6347")));

        using var ms = new MemoryStream();
        image.Save(ms, new WebPNativeEncoder());

        Assert.True(ms.Length > 0, "Output stream should not be empty.");
        Assert.InRange(ms.Length, 30, 50);
    }

    [Fact]
    public void EncodeFromFile() {
        using var image = Image.Load("Resources/test.jpg");
        using var ms = new MemoryStream();
        image.Save(ms, new WebPNativeEncoder());

        Assert.True(ms.Length > 0, "Output stream should not be empty.");
        Assert.InRange(ms.Length, 205_000, 206_000);
    }

    [Fact]
    public void EncodeFromFile24bit() {
        using var image = Image.Load("Resources/test-24.png");
        using var ms = new MemoryStream();
        image.Save(ms, new WebPNativeEncoder());

        Assert.True(ms.Length > 0, "Output stream should not be empty.");
        Assert.InRange(ms.Length, 280_000, 285_000);
    }

    [Fact]
    public async Task EncodeSimpleImageAsync() {
        using var image = new Image<Rgba32>(20, 20);
        image.Mutate(x => x.BackgroundColor(Rgba32.ParseHex("FF6347")));

        await using var ms = new MemoryStream();
        await image.SaveAsync(ms, new WebPNativeEncoder());

        Assert.True(ms.Length > 0, "Output stream should not be empty.");
        Assert.InRange(ms.Length, 30, 50);
    }

    [Fact]
    public async Task EncodeFromFileAsync() {
        using var image = await Image.LoadAsync("Resources/test.jpg");

        await using var ms = new MemoryStream();
        await image.SaveAsync(ms, new WebPNativeEncoder());

        Assert.True(ms.Length > 0, "Output stream should not be empty.");
        Assert.InRange(ms.Length, 205_000, 206_000);
    }

    [Fact]
    public async Task EncodeFromFileAndSetLowQualityAsync() {
        using var image = await Image.LoadAsync("Resources/test.jpg");

        await using var ms = new MemoryStream();
        await image.SaveAsync(ms, new WebPNativeEncoder { Quality = 20 });

        Assert.True(ms.Length > 0, "Output stream should not be empty.");
        Assert.InRange(ms.Length, 15_000, 17_000);
    }

    [Fact]
    public async Task EncodeFromFileAndSetHighQualityAsync() {
        using var image = await Image.LoadAsync("Resources/test.jpg");

        await using var ms = new MemoryStream();
        await image.SaveAsync(ms, new WebPNativeEncoder { Quality = 80 });

        Assert.True(ms.Length > 0, "Output stream should not be empty.");
        Assert.InRange(ms.Length, 38_000, 40_000);
    }

    [Fact]
    public async Task EncodeTransparentAndLossyAsync() {
        using var image = await Image.LoadAsync("Resources/transparent-dice.png");

        await using var ms = new MemoryStream();
        await image.SaveAsync(ms, new WebPNativeEncoder { Quality = 80 });

        Assert.True(ms.Length > 0, "Output stream should not be empty.");
        Assert.InRange(ms.Length, 65_000, 70_000);
    }
}
