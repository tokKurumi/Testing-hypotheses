namespace Modeling_sample.Distributions.Interfaces
{
    public interface IDistribution
    {
        /// <summary>
        /// Получает или принимает генератор случайных чисел выборки.
        /// </summary>
        Random RandomSource { get; set; }
    }
}