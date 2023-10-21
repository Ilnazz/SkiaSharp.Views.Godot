using Godot;

using SkiaSharp.Views.Desktop;

namespace SkiaSharp.Views.Godot;

public partial class SKControl : Control
{
	public event EventHandler<SKPaintSurfaceEventArgs>? PaintSurface;

    private ImageTexture? _imageTexture;

    public override void _Draw()
    {
        int width = (int)Size.X,
            height = (int)Size.Y;

        if (Visible == false || width == 0 || height == 0)
            return;

        var imageInfo = new SKImageInfo(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
        using var surface = SKSurface.Create(imageInfo);
        
        OnPaintSurface(new SKPaintSurfaceEventArgs(surface, imageInfo));

        surface.Canvas.Flush();
            
        var image = Image.Create(width, height, false, Image.Format.Rgba8);
        image.LoadPngFromBuffer(surface.Snapshot().Encode().ToArray());

        if (_imageTexture is null)
            _imageTexture = ImageTexture.CreateFromImage(image);
        else if (_imageTexture.GetSize() != image.GetSize())
            _imageTexture.SetImage(image);
        else
            _imageTexture.Update(image);

        DrawTextureRect(_imageTexture, new Rect2(0, 0, width, height), false);
	}

	protected virtual void OnPaintSurface(SKPaintSurfaceEventArgs e) => PaintSurface?.Invoke(this, e);
}
