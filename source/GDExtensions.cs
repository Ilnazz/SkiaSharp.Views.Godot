using Godot;

namespace SkiaSharp.Views.Godot;

public static class GDExtensions
{
	// Point
	public static SKPoint ToSKPoint(this Vector2 point) => new(point.X, point.Y);
	public static Vector2 ToGDPoint(this SKPoint point) => new(point.X, point.Y);

	// Rect
	public static SKRect ToSKRect(this Rect2 rect) => new(rect.Position.X, rect.Position.Y, rect.End.X, rect.End.Y);
	public static Rect2 ToGDRect(this SKRect rect) => new(rect.Left, rect.Top, rect.Width, rect.Height);

	// Color
	public static SKColor ToSKColor(this Color color) => new((byte)color.R8, (byte)color.G8, (byte)color.B8, (byte)color.A8);
	public static Color ToGDColor(this SKColor color) => Color.Color8(color.Red, color.Green, color.Blue, color.Alpha);

    // Image
    public static Image ToGDImage(this SKPicture picture, SKSizeI dimensions)
    {
        using var skiaImage = SKImage.FromPicture(picture, dimensions);
        return skiaImage.ToGDImage();
    }

    public static Image ToGDImage(this SKImage skiaImage)
    {
        var gdImage = Image.Create(skiaImage.Info.Width, skiaImage.Info.Height, false, Image.Format.Rgba8);
        gdImage.LoadPngFromBuffer(skiaImage.Encode().ToArray());
        return gdImage;
    }

    public static Image ToGDImage(this SKBitmap skiaBitmap)
    {
        using var pixmap = skiaBitmap.PeekPixels();
        using var skiaImage = SKImage.FromPixels(pixmap);
        return skiaImage.ToGDImage();
    }

    public static Image ToGDImage(this SKPixmap pixmap)
    {
        using var skiaImage = SKImage.FromPixels(pixmap);
        return skiaImage.ToGDImage();
    }

    // TODO:
    //public static SKBitmap ToSKBitmap(this Image gdImage)
    //{
    //}

    //public static SKImage ToSKImage(this Image gdImage)
    //{
    //}

    //public static void ToSKPixmap(this Image gdImage, SKPixmap pixmap)
    //{
    //}
}
