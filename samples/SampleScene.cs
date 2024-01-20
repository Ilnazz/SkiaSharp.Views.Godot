using System;
using Godot;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.Godot;

namespace Sample;

public partial class SampleScene : Control
{
    public override void _Ready()
    {
        var skControl = (SKControl)FindChild("SKControl");
        skControl.PaintSurface += OnSKControlPaintSurface;
    }
    
    // Based on https://swharden.com/csdv/skiasharp/skiasharp/
    private static void OnSKControlPaintSurface(object? _, SKPaintSurfaceEventArgs e)
    {
        var imageInfo = e.Info;
        var canvas = e.Surface.Canvas;
        
        canvas.Clear(SKColor.Parse("#003366"));

        const int lineCount = 1000;
        
        for (var i = 0; i < lineCount; i++)
        {
            var lineColor = new SKColor
            (
                red: (byte)Random.Shared.Next(255),
                green: (byte)Random.Shared.Next(255),
                blue: (byte)Random.Shared.Next(255),
                alpha: (byte)Random.Shared.Next(255)
            );

            var lineWidth = Random.Shared.Next(1, 10);
            
            var linePaint = new SKPaint
            {
                Color = lineColor,
                StrokeWidth = lineWidth,
                IsAntialias = true,
                Style = SKPaintStyle.Stroke
            };

            int x1 = Random.Shared.Next(imageInfo.Width),
                y1 = Random.Shared.Next(imageInfo.Height),
                x2 = Random.Shared.Next(imageInfo.Width),
                y2 = Random.Shared.Next(imageInfo.Height);
            
            canvas.DrawLine(x1, y1, x2, y2, linePaint);
        }
    }
}