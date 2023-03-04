using System;
using SkiaSharp;

namespace EasyCharts
{
    internal static class RadialHelpers
    {

        private const float VerticalAngle = (float)Math.PI / 2f;

        private const float GeneralAngle = 2f * (float)Math.PI;


        public static SKPath CreateSectorPath(float baseData, float finishData, float externalRadius, float insideRadius = 0.0f, float margin = 0.0f)
        {
            var data = new SKPath();

            // если у сектора нет размера, то у него нет данных
            if (baseData == finishData) return data;
            

            // сектор представляет собой полный круг, то делаем так
            if (finishData - baseData == 1.0f)
            {
                data.AddCircle(0, 0, externalRadius, SKPathDirection.Clockwise);
                data.AddCircle(0, 0, insideRadius, SKPathDirection.Clockwise);
                data.FillType = SKPathFillType.EvenOdd;
                return data;
            }

            // вычисляем углы
            var startAngle = (GeneralAngle * baseData) - VerticalAngle;
            var endAngle = (GeneralAngle * finishData) - VerticalAngle;
            var large = endAngle - startAngle > (float)Math.PI ? SKPathArcSize.Large : SKPathArcSize.Small;

            // вычисляем угол для полей
            var offsetR = externalRadius == 0 ? 0 : ((margin / (GeneralAngle * externalRadius)) * GeneralAngle);
            var offsetr = insideRadius == 0 ? 0 : ((margin / (GeneralAngle * insideRadius)) * GeneralAngle);

            // получаем данные
            var a = ProcessesCenterCircle(externalRadius, startAngle + offsetR);
            var b = ProcessesCenterCircle(externalRadius, endAngle - offsetR);
            var c = ProcessesCenterCircle(insideRadius, endAngle - offsetr);
            var d = ProcessesCenterCircle(insideRadius, startAngle + offsetr);

            // добавляем точки к пути
            data.MoveTo(a);
            data.ArcTo(externalRadius, externalRadius, 0, large, SKPathDirection.Clockwise, b.X, b.Y);
            data.LineTo(c);

            if (insideRadius == 0.0f)
            {
                /// сокращаем путь
                data.LineTo(d);
            }
            else
            {
                data.ArcTo(insideRadius, insideRadius, 0, large, SKPathDirection.CounterClockwise, d.X, d.Y);
            }

            data.Close();

            return data;
        }

        public static SKPoint ProcessesCenterCircle(float r, float angle) => new SKPoint(r * (float)Math.Cos(angle), r * (float)Math.Sin(angle));
     
    }
}
