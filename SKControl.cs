using Godot;

using SkiaSharp.Views.Desktop;

namespace SkiaSharp.Views.Godot;

public partial class SKControl : Control
{
	public event EventHandler<SKPaintSurfaceEventArgs>? PaintSurface;

    private Image? _image;
    private ImageTexture? _imageTexture;

    private SKImageInfo? _imageInfo;
    private SKSurface? _surface;

    public override void _Draw()
    {
        int width = (int)Size.X,
            height = (int)Size.Y;

        if (Visible == false || width == 0 || height == 0)
            return;

        if (_imageInfo is null || _imageInfo.Value.Width != width || _imageInfo.Value.Height != height)
        {
            _imageInfo = new SKImageInfo(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
            
            _surface?.Dispose();
            _surface = SKSurface.Create(_imageInfo.Value);
        }

        _surface!.Canvas.Clear();

        OnPaintSurface(new SKPaintSurfaceEventArgs(_surface, _imageInfo.Value));

        _surface.Canvas.Flush();
        
        if (_image is null || _image.GetWidth() != width || _image.GetHeight() != height)
            _image = Image.Create(width, height, false, Image.Format.Rgba8);

        _image.LoadPngFromBuffer(_surface.Snapshot().Encode().ToArray());

        if (_imageTexture is null)
            _imageTexture = ImageTexture.CreateFromImage(_image);
        else if (_imageTexture.GetSize() != _image.GetSize())
            _imageTexture.SetImage(_image);
        else
            _imageTexture.Update(_image);

        DrawTextureRect(_imageTexture, new Rect2(0, 0, width, height), false);
	}

    public override void _ExitTree()
    {
        _image?.Dispose();
        _imageTexture?.Dispose();
        _surface?.Dispose();
    }

    protected virtual void OnPaintSurface(SKPaintSurfaceEventArgs e) => PaintSurface?.Invoke(this, e);
}
