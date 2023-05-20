namespace Modeling_sample.Distributions.Interfaces
{
    public interface IUnivariateDistribution
    {
        /// <summary>
        /// Получает математическое ожидаение распределения.
        /// </summary>
        double Mean { get; }

        /// <summary>
        /// Получает дисперсию распределения.
        /// </summary>
        double Variance { get; }

        /// <summary>
        /// Получает среднее квадратичное отклонение.
        /// </summary>
        double StdDev { get; }

        /// <summary>
        /// Получает энтропию распределения.
        /// </summary>.
        double Entropy { get; }

        /// <summary>
        /// Получает асимметрию распределения.
        /// </summary>
        double Skewness { get; }

        /// <summary>
        /// Получает медиану распределения.
        /// </summary>
        double Median { get; }

        /// <summary>
        /// Вычисляет значение функции распределения (CDF) в точке x, т.е. P(X ≤ x).
        /// </summary>
        /// <param name="x">Оценочная точка.</param>
        /// <returns>распределение (CDF) в точке x.</returns>
        double CumulativeDistribution(double x);
    }
}