using System;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using EasyCharts;
using SkiaSharp;

namespace Avalonia.easyCharts
{
    public class GraphProcessing : Control
    {
        private CustomDrawingOperation custom = new CustomDrawingOperation();

        private SetChart chart;

        public SetChart Chart
        {
            get => chart;
            set => SetAndRaise(ChartProperty, ref chart, value);
        }

        public static readonly DirectProperty<GraphProcessing, SetChart> ChartProperty =
            AvaloniaProperty.RegisterDirect<GraphProcessing, SetChart>("Chart", c => c.Chart, (c, v) => c.Chart = v);

        public GraphProcessing()
        {
            this.GetObservable(ChartProperty).Subscribe(ChartChanged);
        }

        private void ChartChanged(SetChart chart)
        {
            this.InvalidateVisual();
        }

        protected override Size MeasureOverride(Size availableSize)
            => availableSize;

        public override void Render(DrawingContext context)
        {
            this.custom.Bounds = this.Bounds;
            this.custom.Chart = this.chart;

            context.Custom(custom);
        }

        private class CustomDrawingOperation : ICustomDrawOperation
        {
            public SetChart Chart { get; set; }
            public void Dispose() { }
            public bool HitTest(Point p) => false;
            public bool Equals(ICustomDrawOperation other) => this.Equals(other);

            public Rect Bounds { get; set; }

            public void Render(IDrawingContextImpl context)
            {
                var bitmap = new SKBitmap((int)Bounds.Width, (int)Bounds.Height, false);
                var canvas = new SKCanvas(bitmap);
                canvas.Save();

                Chart.Draw(canvas, (int)Bounds.Width, (int)Bounds.Height);

                if (context is ISkiaDrawingContextImpl skia)
                {
                    skia.SkCanvas.DrawBitmap(bitmap, (int)Bounds.X, (int)Bounds.Y);
                }
            }
        }
    }
}