using Godot;
using SkiaSharp.Views.Desktop;

namespace SkiaSharp.Views.Godot;

public partial class SKControl : Control
{
    public SKSizeI CanvasSize => _bitmap is not null ? _imageInfo.Size : SKSizeI.Empty;

	public event EventHandler<SKPaintSurfaceEventArgs>? PaintSurface;

    #region Fields
    private SKImageInfo _imageInfo;
    
    private SKBitmap? _bitmap;
    private SKSurface? _surface;
    
    private ImageTexture? _imageTexture;
    #endregion

    public SKControl() => TreeExiting += OnTreeExiting;

    public override void _Draw()
    {
        int width = (int)Size.X,
            height = (int)Size.Y;

        if (!Visible || width is 0 || height is 0)
            return;

        if (_imageInfo.Width != width || _imageInfo.Height != height)
        {
            // It would be good if the color type was set by default for the current platform,
            // but the Godot's Image.Format does not support all color types of SKColorType.
            _imageInfo = new SKImageInfo(width, height, SKColorType.Rgba8888);

            _bitmap?.Dispose();
            _surface?.Dispose();

            _bitmap = new SKBitmap(_imageInfo);
            _surface = SKSurface.Create(_imageInfo, _bitmap.GetPixels());
        }

        OnPaintSurface(new SKPaintSurfaceEventArgs(_surface, _imageInfo));
        _surface!.Canvas.Flush();

        var image = Image.CreateFromData(width, height, false, Image.Format.Rgba8, _bitmap!.Bytes);

        if (_imageTexture is null)
            _imageTexture = ImageTexture.CreateFromImage(image);
        else if (_imageTexture.GetSize() != image.GetSize())
            _imageTexture.SetImage(image);
        else
            _imageTexture.Update(image);

        DrawTextureRect(_imageTexture, new Rect2(0, 0, width, height), false);
    }
    
    protected virtual void OnPaintSurface(SKPaintSurfaceEventArgs e) => PaintSurface?.Invoke(this, e);

    private void OnTreeExiting()
    {
        _bitmap?.Dispose();
        _surface?.Dispose();
        _imageTexture?.Dispose();
    }
}