using Godot;

namespace SkiaSharp.Views.Godot;

public static class GDExtensions
{
	// Point
	public static SKPoint ToSKPoint(this Vector2 vector2) => new(vector2.X, vector2.Y);
	public static Vector2 ToVector2(this SKPoint skPoint) => new(skPoint.X, skPoint.Y);

	// Size
	public static SKSize ToSKSize(this Vector2 vector2) => new(vector2.X, vector2.Y);
	public static SKSizeI ToSKSizeI(this Vector2I vector2I) => new(vector2I.X, vector2I.Y);

	public static Vector2 ToVector2(this SKSize skSize) => new(skSize.Width, skSize.Height);
	public static Vector2I ToVector2I(this SKSizeI skSize) => new(skSize.Width, skSize.Height);
	
	// Rect
	public static SKRect ToSKRect(this Rect2 rect2) => new(rect2.Position.X, rect2.Position.Y, rect2.End.X, rect2.End.Y);
	public static Rect2 ToRect2(this SKRect skRect) => new(skRect.Left, skRect.Top, skRect.Width, skRect.Height);

	// Color
	public static SKColor ToSKColor(this Color color) => new((byte)color.R8, (byte)color.G8, (byte)color.B8, (byte)color.A8);
	public static Color ToColor(this SKColor skColor) => Color.Color8(skColor.Red, skColor.Green, skColor.Blue, skColor.Alpha);

    #region Conversion between Skia and Godot image formats
    
    // Todo: complete the lookup table according to the API documentations:

    // Skia color types: https://api.skia.org/SkColorType_8h.html#a3ea8ab47612dbc4801c18445176c6481
    // Godot image formats: https://docs.godot.community/classes/class_image#enumerations

    private static readonly IReadOnlyDictionary<SKColorType, Image.Format> ImageFormatsBySkColorType = new Dictionary<SKColorType, Image.Format>
    {
        [SKColorType.Rg88] = Image.Format.Rg8,
        [SKColorType.Rgb565] = Image.Format.Rgb565,
        [SKColorType.Rgba8888] = Image.Format.Rgba8,
        [SKColorType.Rgba16161616] = Image.Format.Rgbah,

        // SKColorType.Unknown => ,

        // SKColorType.Gray8 => ,

        // SKColorType.Alpha8 => ,
        // SKColorType.Alpha16 => ,
        // SKColorType.AlphaF16 => ,

        // SKColorType.Rg1616 => ,
        // SKColorType.RgF16 => ,

        // SKColorType.Rgb888x => ,
        // SKColorType.Rgb101010x => ,

        // SKColorType.Rgba1010102 => ,
        // SKColorType.RgbaF16Clamped => ,
        // SKColorType.RgbaF16 => ,
        // SKColorType.RgbaF32 => ,

        // SKColorType.Argb4444 => ,

        // SKColorType.Bgr101010x =>
        // SKColorType.Bgra8888 => ,
        // SKColorType.Bgra1010102 => ,
    };

    public static Image.Format ToImageFormat(this SKColorType skColorType)
    {
        GD.Print(skColorType);
        if (ImageFormatsBySkColorType.TryGetValue(skColorType, out var imageFormat))
            return imageFormat;

        throw new NotSupportedException();
    }

    public static SKColorType ToSKColorType(this Image.Format imageFormat)
    {
        foreach (var (skColorType, imgFormat) in ImageFormatsBySkColorType)
            if (imgFormat == imageFormat)
                return skColorType;

        throw new NotSupportedException();
    }
    #endregion

    #region Conversion from Skia image types to Godot Image type
    public static Image ToImage(this SKPicture skPicture, SKSizeI dimensions)
    {
        using var skImage = SKImage.FromPicture(skPicture, dimensions);
        return skImage.ToImage();
    }

    public static Image ToImage(this SKPixmap skPixmap)
    {
        using var skImage = SKImage.FromPixels(skPixmap);
        return skImage.ToImage();
    }

    public static Image ToImage(this SKImage skImage)
    {
	    using var skBitmap = SKBitmap.FromImage(skImage);
        return skBitmap.ToImage();
    }

    public static Image ToImage(this SKBitmap skBitmap) =>
	    Image.CreateFromData(skBitmap.Width, skBitmap.Height, false, skBitmap.ColorType.ToImageFormat(), skBitmap.Bytes);
    #endregion

    #region Conversion from Godot Image type to Skia image types
    
    // Todo: implement these methods
    
    //public static SKImage ToSKImage(this Image image) => SKImage.FromBitmap(image.ToSKBitmap());

	//public static SKPixmap ToSKPixmap(this Image image) => ;

    //public static SKPicture ToSKPicture(this Image image, SKSizeI dimensions) => ;
    
    //public static SKBitmap ToSKBitmap(this Image image) =>
    //    SKBitmap.Decode(image.GetData(), new SKImageInfo(image.GetWidth(), image.GetHeight(), image.GetFormat().ToSKColorType()));
    #endregion
}