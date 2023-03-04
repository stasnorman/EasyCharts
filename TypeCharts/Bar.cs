using System;
using System.Linq;
using SkiaSharp;

namespace EasyCharts
{
    public class Bar : PCharts
    {
        public Bar()
        {
            this.PointSize = 0;
        }

        /// <summary>
        /// Получает или устанавливает альфа-канал области фона бара.
        /// </summary>
        /// <value>Альфа области полосы.</value>
        public byte BarAreaAlpha { get; set; } = 32;

        /// <summary>
        /// Рисует содержимое диаграммы на указанном холсте.
        /// </summary>
        /// <param name="canvas">Выходной холст.</param>
        /// <param name="width">Ширина диаграммы.</param>
        /// <param name="height">Высота диаграммы.</param>
        public override void DrawContent(SKCanvas canvas, int width, int height)
        {
            var valueLabelSizes = MeasureValueLabels();
            var footerHeight = CalculateFooterHeight(valueLabelSizes);
            var headerHeight = CalculateHeaderHeight(valueLabelSizes);
            var itemSize = CalculateItemSize(width, height, footerHeight, headerHeight);
            var origin = CalculateYOrigin(itemSize.Height, headerHeight);
            var points = this.CalculatePoints(itemSize, origin, headerHeight);

            this.DrawBarAreas(canvas, points, itemSize, headerHeight);
            this.DrawBars(canvas, points, itemSize, origin, headerHeight);
            this.DrawPoints(canvas, points);
            this.DrawFooter(canvas, points, itemSize, height, footerHeight);
            this.DrawValueLabel(canvas, points, itemSize, height, valueLabelSizes);
        }

        /// <summary>
        /// Рисует столбцы значений.
        /// </summary>
        /// <param name="canvas">Холст.</param>
        /// <param name="points">Точки.</param>
        /// <param name="itemSize">Размер элемента.</param>
        /// <param name="origin">Источник.</param>
        /// <param name="headerHeight">Высота заголовка.</param>
        protected void DrawBars(SKCanvas canvas, SKPoint[] points, SKSize itemSize, float origin, float headerHeight)
        {
            const float MinBarHeight = 4;
            if (points.Length > 0)
            {
                for (int i = 0; i < this.Entries.Count(); i++)
                {
                    var entry = this.Entries.ElementAt(i);
                    var point = points[i];

                    using (var paint = new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Color = entry.Color,
                    })
                    {
                        var x = point.X - (itemSize.Width / 2);
                        var y = Math.Min(origin, point.Y);
                        var height = Math.Max(MinBarHeight, Math.Abs(origin - point.Y));
                        if (height < MinBarHeight)
                        {
                            height = MinBarHeight;
                            if (y + height > this.Margin + itemSize.Height)
                            {
                                y = headerHeight + itemSize.Height - height;
                            }
                        }

                        var rect = SKRect.Create(x, y, itemSize.Width, height);
                        canvas.DrawRect(rect, paint);
                    }
                }
            }
        }

        /// <summary>
        /// Рисует области фона бара.
        /// </summary>
        /// <param name="canvas">Выходной холст.</param>
        /// <param name="points">Точки входа.</param>
        /// <param name="itemSize">Размер элемента.</param>
        /// <param name="headerHeight">Высота заголовка.</param>
        protected void DrawBarAreas(SKCanvas canvas, SKPoint[] points, SKSize itemSize, float headerHeight)
        {
            if (points.Length > 0 && this.PointAreaAlpha > 0)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    var entry = this.Entries.ElementAt(i);
                    var point = points[i];

                    using (var paint = new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Color = entry.Color.WithAlpha(this.BarAreaAlpha),
                    })
                    {
                        var max = entry.Value > 0 ? headerHeight : headerHeight + itemSize.Height;
                        var height = Math.Abs(max - point.Y);
                        var y = Math.Min(max, point.Y);
                        canvas.DrawRect(SKRect.Create(point.X - (itemSize.Width / 2), y, itemSize.Width, height), paint);
                    }
                }
            }
        }
    }
}
