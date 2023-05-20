namespace Modeling_sample.Distributions.Interfaces
{
    public interface IContinuousDistribution
    {
        /// <summary>
        /// Получает наименьший элемент в области распределения, который может быть представлен
        /// целым числом.
        /// </summary>
        double Minimum { get; }

        /// <summary>
        /// Получает самый большой элемент в области распределений, который может быть представлен
        /// целым числом.
        /// </summary>
        double Maximum { get; }

        /// <summary>
        /// Вычисляет плотность выборки (PDF) в точке x, т.е. ∂P(X ≤ x)/∂x.
        /// </summary>
        /// <param name="x">Оценочная точка.</param>
        /// <returns>плотность выборки (PDF) в точке x.</returns>
        double Density(double x);
    }
}
