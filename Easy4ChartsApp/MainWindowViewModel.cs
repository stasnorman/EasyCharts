using EasyCharts;
using SkiaSharp;

namespace Avalonia.Microcharts.Example
{
    public class MainWindowViewModel
    {
        public SetValues[] Entries = new SetValues[]
        {
            new SetValues()
            {
                Value = 999,
                Label = "January",
                ValueLabel = "999",
                Color = SKColor.Parse("#266489")
            },
            new SetValues()
            {
                Value = 999,
                Label = "February",
                ValueLabel = "999",
                Color = SKColor.Parse("#68B9C0")
            },
            new SetValues()
            {
                Value = -999,
                Label = "March",
                ValueLabel = "-999",
                Color = SKColor.Parse("#90D585")
            }
        };

        public SetChart[] Charts { get; set; }

        public MainWindowViewModel()
        {
            this.Charts = new SetChart[]
            {
                new Bar() {Entries = this.Entries},
                new PCharts() {Entries = this.Entries},
                new Line() {Entries = this.Entries},
                new Donut() {Entries = this.Entries},
                new RadialGauge() {Entries = this.Entries},
                new Radar() {Entries = this.Entries}
            };
        }
    }
}