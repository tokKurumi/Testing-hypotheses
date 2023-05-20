namespace Modeling_sample.Distributions.Interfaces
{
    //
    // Summary:
    //     Discrete Univariate Probability Distribution.
    public interface IDiscreteDistribution
    {
        /// <summary>
        /// Получает наименьший элемент в области распределения, который может быть представлен
        /// целым числом.
        /// </summary>
        int Minimum { get; }

        /// <summary>
        /// Получает самый большой элемент в области распределений, который может быть представлен
        /// целым числом.
        /// </summary>
        int Maximum { get; }

        /// <summary>
        /// Вычисляет значение функции вероятности (PMF) в точке k, т.е. P(X = k).
        /// </summary>
        /// <param name="k"></param>
        /// <returns>вероятность (PMF) в точке k.</returns>
        double Probability(int k);
    }
}