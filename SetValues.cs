using SkiaSharp;

namespace EasyCharts
{
    /// <summary>
    /// Ввод данных для диаграммы.
	/// </summary>
    public class SetValues
	{
        /// <summary>
		/// Получить значение
		/// </summary>
		/// <value>Введите значение</value>
		public float Value { get; set; }

        /// <summary>
        /// Получает или задает метку заголовка.
        /// </summary>
        /// <value>Введите метку</value>
        public string Label { get; set; }

        /// <summary>
        /// Получает или задает метку, связанную со значением.
        /// </summary>
        /// <value>Значение метки</value>
        public string ValueLabel { get; set; }

        /// <summary>
        /// Получает или задает цвет заливки.
        /// </summary>
        /// <value>Введите цвет.</value>
        public SKColor Color { get; set; } = SKColors.Black;

        /// <summary>
        /// Получает или задает цвет текста (для метки заголовка).
        /// </summary>
        /// <value>Цвет текста.</value>
        public SKColor TextColor { get; set; } = SKColors.Gray;
    }
}
