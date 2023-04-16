# Shorthand.ImageSharp.WebP [![Build Status](https://dev.azure.com/karl-sjogren/Shorthand.ImageSharp.WebP/_apis/build/status/karl-sjogren.Shorthand.ImageSharp.WebP?branchName=master)](https://dev.azure.com/karl-sjogren/Shorthand.ImageSharp.WebP/_build/latest?definitionId=4&branchName=master)

Adds support for encoding WebP images using libwebp to ImageSharp.

## Why is this needed?

When ImageSharp 2.x encodes transparent images with compression, the
file size is larger then even doing lossless webp encoding. This
was fixed in ImageSharp 3.x, but with the new license this isn't
feasible to all projects. So this makes it possible to encode transparent
WebP images properly on ImageSharp 2.0.

Issue: <https://github.com/SixLabors/ImageSharp/issues/2332>

## Usage

If running or deploying on Linux you need to install libwebp-dev.

```sh
sudo apt-get update -y && sudo apt-get install -y libwebp-dev
```

Sample images from Pexels, <https://www.pexels.com/sv-se/license/> or <https://www.pexels.com/sv-se/creative-commons-images/>.

<https://www.pexels.com/sv-se/@pok-rie-33563>
<https://www.pexels.com/sv-se/@naushil-ansari-151720>
