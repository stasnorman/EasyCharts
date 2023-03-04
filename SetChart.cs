using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;

namespace EasyCharts
{
    public abstract class SetChart
    {
        /// <summary>
        /// Получает или устанавливает глобальное поле.
        /// </summary>
        /// <value>Отступ</value>
        public float Margin { get; set; } = 20;

        /// <summary>
        /// Получает или задает размер текста меток.
        /// </summary>
        /// <value>Размер текста метки.</value>
        public float LabelTextSize { get; set; } = 16;

        /// <summary>
        /// Получает или задает цвет фона диаграммы.
        /// </summary>
        /// <value>Цвет фона.</value>
        public SKColor BackgroundColor { get; set; } = SKColors.White;

        /// <summary>
        /// Получает или задает записи данных.
        /// </summary>
        /// <value>Запись.</value>
        public IEnumerable<SetValues> Entries { get; set; }

        /// <summary>
        /// Получает или задает минимальное значение из записей. Если не определено, это будет минимум между нулем и минимальное значение входа.
        /// </summary>
        /// <value>Минимальное значение.</value>
        public float MinValue
        {
            get
            {
                if (!this.Entries.Any())
                {
                    return 0;
                }

                if (this.InternalMinValue == null)
                {
                    return Math.Min(0, this.Entries.Min(x => x.Value));
                }

                return Math.Min(this.InternalMinValue.Value, this.Entries.Min(x => x.Value));
            }

            set => this.InternalMinValue = value;
        }

        /// <summary>
        /// Получает или устанавливает максимальное значение из записей. Если не определено, это будет максимум между нулем и максимальное значение входа.
        /// </summary>
        /// <value>Минимальное значение.</value>
        public float MaxValue
        {
            get
            {
                if (!this.Entries.Any())
                {
                    return 0;
                }

                if (this.InternalMaxValue == null)
                {
                   return Math.Max(0, this.Entries.Max(x => x.Value));
                }

                return Math.Max(this.InternalMaxValue.Value, this.Entries.Max(x => x.Value));
            }

            set => this.InternalMaxValue = value;
        }

        /// <summary>
        /// Получает или устанавливает внутреннее минимальное значение (которое может быть нулевым).
        /// </summary>
        /// <value>Внутреннее минимальное значение.</value>
        protected float? InternalMinValue { get; set; }

        /// <summary>
        /// Получает или устанавливает внутреннее максимальное значение (которое может быть нулевым).
        /// </summary>
        /// <value>Внутреннее максимальное значение.</value>
        protected float? InternalMaxValue { get; set; }

        /// <summary>
        /// Отрисовка графика
        /// </summary>
        /// <param name="canvas">Слой.</param>
        /// <param name="width">Ширина</param>
        /// <param name="height">Высота</param>
        public void Draw(SKCanvas canvas, int width, int height)
        {
            canvas.Clear(this.BackgroundColor);

            this.DrawContent(canvas, width, height);
        }

        /// <summary>
        /// Отрисовка внутренней части графика
        /// </summary>
        /// <param name="canvas">Слой.</param>
        /// <param name="width">Ширина</param>
        /// <param name="height">Высота</param>
        public abstract void DrawContent(SKCanvas canvas, int width, int height);

        /// <summary>
        /// Рисует элементы заголовка справа или слева от диаграммы.
        /// </summary>
        /// <param name="canvas">Холст.</param>
        /// <param name="width">Ширина.</param>
        /// <param name="height">Высота.</param>
        /// <param name="entries">Записи.</param>
        /// <param name="isLeft">Если задано значение <c>true</c>, остается слева.</param>
        protected void DrawCaptionElements(SKCanvas canvas, int width, int height, List<SetValues> entries, bool isLeft)
        {
            var margin = 2 * this.Margin;
            var availableHeight = height - (2 * margin);
            var ySpace = (availableHeight - this.LabelTextSize) / ((entries.Count <= 1) ? 1 : entries.Count - 1);

            for (int i = 0; i < entries.Count; i++)
            {
                var entry = entries.ElementAt(i);
                var y = margin + (i * ySpace);
                if (entries.Count <= 1)
                {
                    y += (availableHeight - this.LabelTextSize) / 2;
                }

                var hasLabel = !string.IsNullOrEmpty(entry.Label);
                var hasValueLabel = !string.IsNullOrEmpty(entry.ValueLabel);

                if (hasLabel || hasValueLabel)
                {
                    var hasOffset = hasLabel && hasValueLabel;
                    var captionMargin = this.LabelTextSize * 0.60f;
                    var space = hasOffset ? captionMargin : 0;
                    var captionX = isLeft ? this.Margin : width - this.Margin - this.LabelTextSize;

                    using (var paint = new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Color = entry.Color,
                    })
                    {
                        var rect = SKRect.Create(captionX, y, this.LabelTextSize, this.LabelTextSize);
                        canvas.DrawRect(rect, paint);
                    }

                    if (isLeft)
                    {
                        captionX += this.LabelTextSize + captionMargin;
                    }
                    else
                    {
                        captionX -= captionMargin;
                    }

                    canvas.DrawCaptionLabels(entry.Label, entry.TextColor, entry.ValueLabel, entry.Color, this.LabelTextSize, new SKPoint(captionX, y + (this.LabelTextSize / 2)), isLeft ? SKTextAlign.Left : SKTextAlign.Right);
                }
            }
        }
    }
}