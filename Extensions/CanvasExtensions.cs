using Avalonia.EasyCharts.Mode;
using SkiaSharp;

namespace EasyCharts
{
    public static class CanvasExtensions
    {
        public static void DrawCaptionLabels(this SKCanvas objectCanvas, string objectLabel, SKColor objectLabelColor, string objectValue, SKColor objectValueColor, float textSize, SKPoint point, SKTextAlign horizontalAlignment)
        {
            bool isLabel = !string.IsNullOrEmpty(objectLabel);
            bool isValue = !string.IsNullOrEmpty(objectValue);

            if (isLabel || isValue)
            {
                var hasOffset = isLabel && isValue;
                var captionMargin = textSize * 0.60f;
                var scopeArea = hasOffset ? captionMargin : 0;

                if (isLabel)
                {
                    using (var paint = new SKPaint()
                    {
                        TextSize = textSize,
                        IsAntialias = true,
                        Color = objectLabelColor,
                        IsStroke = false,
                        TextAlign = horizontalAlignment,
                    })
                    {
                        var bounds = new SKRect();
                        var text = objectLabel;
                        paint.MeasureText(text, ref bounds);

                        var y = point.Y - ((bounds.Top + bounds.Bottom) / 2) - scopeArea;

                        objectCanvas.DrawText(text, point.X, y, paint);
                    }
                }

                if (isValue)
                {
                    using (var paint = new SKPaint()
                    {
                        TextSize = textSize,
                        IsAntialias = true,
                        FakeBoldText = true,
                        Color = objectValueColor,
                        IsStroke = false,
                        TextAlign = horizontalAlignment,
                    })
                    {
                        var bounds = new SKRect();
                        var text = objectValue;
                        paint.MeasureText(text, ref bounds);

                        var y = point.Y - ((bounds.Top + bounds.Bottom) / 2) + scopeArea;

                        objectCanvas.DrawText(text, point.X, y, paint);
                    }
                }
            }
        }

        /// <summary>
        /// Рисует заданную точку.
        /// </summary>
        /// <param name="canvas">Холст.</param>
        /// <param name="point">Точка.</param>
        /// <param name="color">Цвет заливки.</param>
        /// <param name="size">Размер в пунктах.</param>
        /// <param name="mode">Обработка точки.</param>
        public static void DrawPoint(this SKCanvas canvas, SKPoint point, SKColor color, float size, PointMode mode)
        {
            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                IsAntialias = true,
                Color = color,
            })
            {
                switch (mode)
                {
                    case PointMode.Square:
                        canvas.DrawRect(SKRect.Create(point.X - (size / 2), point.Y - (size / 2), size, size), paint);
                        break;

                    case PointMode.Circle:
                        paint.IsAntialias = true;
                        canvas.DrawCircle(point.X, point.Y, size / 2, paint);
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Рисует линию с градиентной обводкой.
        /// </summary>
        /// <param name="canvas">Холст.</param>
        /// <param name="startPoint">Начальная точка.</param>
        /// <param name="startColor">Начальный цвет.</param>
        /// <param name="endPoint">Конечная точка.</param>
        /// <param name="endColor">Конечный цвет.</param>
        /// <param name="size">Размер штриха.</param>
        public static void DrawGradientLine(this SKCanvas canvas, SKPoint startPoint, SKColor startColor, SKPoint endPoint, SKColor endColor, float size)
        {
            using (var shader = SKShader.CreateLinearGradient(startPoint, endPoint, new[] { startColor, endColor }, null, SKShaderTileMode.Clamp))
            {
                using (var paint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    StrokeWidth = size,
                    Shader = shader,
                    IsAntialias = true,
                })
                {
                    canvas.DrawLine(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y, paint);
                }
            }
        }
    }
}
