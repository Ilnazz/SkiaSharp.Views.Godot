using Godot;

using SkiaSharp.Views.Desktop;
using System.Runtime.InteropServices;

namespace SkiaSharp.Views.Godot;

public partial class SKControl : Control
{
	public event EventHandler<SKPaintSurfaceEventArgs>? PaintSurface;

    private SKImageInfo? _imageInfo;
    private byte[]? _imageData;
    private GCHandle? _imageDataHandle;
    private SKSurface? _surface;
    private ImageTexture? _imageTexture;

    public override void _Draw()
    {
        int width = (int)Size.X,
            height = (int)Size.Y;

        if (Visible == false || width == 0 || height == 0)
            return;

        if (_imageInfo is null || _imageInfo.Value.Width != width || _imageInfo.Value.Height != height)
        {
            _imageInfo = new SKImageInfo(width, height, SKColorType.Rgba8888, SKAlphaType.Unpremul);

            _surface?.Dispose();
            _imageDataHandle?.Free();

            _imageData = new byte[_imageInfo.Value.BytesSize];
            _imageDataHandle = GCHandle.Alloc(_imageData, GCHandleType.Pinned);
            _surface = SKSurface.Create(_imageInfo.Value, _imageDataHandle.Value.AddrOfPinnedObject(), _imageInfo.Value.RowBytes);
        }

        //_surface!.Canvas.Clear();
        OnPaintSurface(new SKPaintSurfaceEventArgs(_surface, _imageInfo.Value));
        _surface!.Canvas.Flush();

        var image = Image.CreateFromData(width, height, false, Image.Format.Rgba8, _imageData);

        if (_imageTexture is null)
            _imageTexture = ImageTexture.CreateFromImage(image);
        else if (_imageTexture.GetSize() != image.GetSize())
            _imageTexture.SetImage(image);
        else
            _imageTexture.Update(image);

        DrawTextureRect(_imageTexture, new Rect2(0, 0, width, height), false);
    }

    public override void _ExitTree()
    {
        _imageDataHandle?.Free();
        _surface?.Dispose();
        _imageTexture?.Dispose();
    }

    protected virtual void OnPaintSurface(SKPaintSurfaceEventArgs e) => PaintSurface?.Invoke(this, e);
}