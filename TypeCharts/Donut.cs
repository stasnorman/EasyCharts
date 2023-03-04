﻿namespace EasyCharts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SkiaSharp;

    public class Donut : SetChart
    {
        /// <summary>
        /// Получает или задает радиус отверстия в центре графика.
        /// /// </summary>
        /// <value>Радиус.</value>
        public float HoleRadius { get; set; } = 0;

        public override void DrawContent(SKCanvas canvas, int width, int height)
        {
            this.DrawCaption(canvas, width, height);
            using (new SKAutoCanvasRestore(canvas))
            {
                canvas.Translate(width / 2, height / 2);
                var sumValue = this.Entries.Sum(x => Math.Abs(x.Value));
                var radius = (Math.Min(width, height) - (2 * Margin)) / 2;

                var start = 0.0f;
                for (int i = 0; i < this.Entries.Count(); i++)
                {
                    var entry = this.Entries.ElementAt(i);
                    var end = start + (Math.Abs(entry.Value) / sumValue);

                    // Sector
                    var path = RadialHelpers.CreateSectorPath(start, end, radius, radius * this.HoleRadius);
                    using (var paint = new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Color = entry.Color,
                        IsAntialias = true,
                    })
                    {
                        canvas.DrawPath(path, paint);
                    }

                    start = end;
                }
            }
        }

        /// <summary>
        /// Отрисовка объекта
        /// </summary>
        /// <param name="canvas">Слой</param>
        /// <param name="width">Выота</param>
        /// <param name="height">Ширина</param>
        private void DrawCaption(SKCanvas canvas, int width, int height)
        {
            var sumValue = this.Entries.Sum(x => Math.Abs(x.Value));
            var rightValues = new List<SetValues>();
            var leftValues = new List<SetValues>();

            int i = 0;
            var current = 0.0f;
            while (i < this.Entries.Count() && (current < sumValue / 2))
            {
                var entry = this.Entries.ElementAt(i);
                rightValues.Add(entry);
                current += Math.Abs(entry.Value);
                i++;
            }

            while (i < this.Entries.Count())
            {
                var entry = this.Entries.ElementAt(i);
                leftValues.Add(entry);
                i++;
            }

            leftValues.Reverse();

            this.DrawCaptionElements(canvas, width, height, rightValues, false);
            this.DrawCaptionElements(canvas, width, height, leftValues, true);
        }
    }
}