using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;

namespace EasyCharts
{
    public class RadialGauge : SetChart
    {
        /// <summary>
        /// Получает или устанавливает размер каждого датчика. Если отрицательный, то он будет рассчитываться из доступного места.
        /// </summary>
        /// <value>Задается значение</value>
        public float LineSize { get; set; } = -1;

        /// <summary>
        /// Получает или устанавливает альфа-канал области фона датчика.
        /// </summary>
        /// <value>Задается значение</value>
        public byte LineAreaAlpha { get; set; } = 52;

        /// <summary>
        /// Получает или задает начальный угол.
        /// </summary>
        /// <value>Задается значение.</value>
        public float StartAngle { get; set; } = -90;

        private float AbsoluteMinimum => this.Entries.Select(x => x.Value).Concat(new[] { this.MaxValue, this.MinValue, this.InternalMinValue ?? 0 }).Min(x => Math.Abs(x));

        private float AbsoluteMaximum => this.Entries.Select(x => x.Value).Concat(new[] { this.MaxValue, this.MinValue, this.InternalMinValue ?? 0 }).Max(x => Math.Abs(x));

        private float ValueRange => this.AbsoluteMaximum - this.AbsoluteMinimum;

        public void DrawGaugeArea(SKCanvas canvas, SetValues entry, float radius, int cx, int cy, float strokeWidth)
        {
            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = strokeWidth,
                Color = entry.Color.WithAlpha(this.LineAreaAlpha),
                IsAntialias = true,
            })
            {
                canvas.DrawCircle(cx, cy, radius, paint);
            }
        }

        public void DrawGauge(SKCanvas canvas, SetValues entry, float radius, int cx, int cy, float strokeWidth)
        {
            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = strokeWidth,
                StrokeCap = SKStrokeCap.Round,
                Color = entry.Color,
                IsAntialias = true,
            })
            {
                using (SKPath path = new SKPath())
                {
                    var sweepAngle = 360 * (Math.Abs(entry.Value) - this.AbsoluteMinimum) / this.ValueRange;
                    path.AddArc(SKRect.Create(cx - radius, cy - radius, 2 * radius, 2 * radius), this.StartAngle, sweepAngle);
                    canvas.DrawPath(path, paint);
                }
            }
        }

        public override void DrawContent(SKCanvas canvas, int width, int height)
        {
            this.DrawCaption(canvas, width, height);

            var sumValue = this.Entries.Sum(x => Math.Abs(x.Value));
            var radius = (Math.Min(width, height) - (2 * Margin)) / 2;
            var cx = width / 2;
            var cy = height / 2;
            var lineWidth = (this.LineSize < 0) ? (radius / ((this.Entries.Count() + 1) * 2)) : this.LineSize;
            var radiusSpace = lineWidth * 2;

            for (int i = 0; i < this.Entries.Count(); i++)
            {
                var entry = this.Entries.ElementAt(i);
                var entryRadius = (i + 1) * radiusSpace;
                this.DrawGaugeArea(canvas, entry, entryRadius, cx, cy, lineWidth);
                this.DrawGauge(canvas, entry, entryRadius, cx, cy, lineWidth);
            }
        }

        private void DrawCaption(SKCanvas canvas, int width, int height)
        {
            var range = this.ValueRange;
            var rightValues = new List<SetValues>();
            var leftValues = new List<SetValues>();

            foreach (var entry in this.Entries)
            {
                if (Math.Abs(entry.Value) < range / 2)
                {
                    rightValues.Add(entry);
                }
                else
                {
                    leftValues.Add(entry);
                }
            }

            leftValues.Reverse();

            this.DrawCaptionElements(canvas, width, height, rightValues, false);
            this.DrawCaptionElements(canvas, width, height, leftValues, true);
        }
    }
}