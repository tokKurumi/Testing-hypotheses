using System.Runtime.CompilerServices;
using Modeling_sample.Distributions.Interfaces;

namespace Univariate_distributions.Models
{
    class Poisson : IDiscreteDistribution, IUnivariateDistribution, IDistribution
	{
		private System.Random _random;

		private readonly double _lambda;

		/// <summary>
		/// Получает параметр распределения Пуассона λ. Диапазон: λ > 0.
		/// </summary>
		public double Lambda => _lambda;

		/// <summary>
		/// Получает или задает генератор случайных чисел, который используется для построения
		/// случайных выборок.
		/// </summary>
		public System.Random RandomSource
		{
			get
			{
				return _random;
			}
			set
			{
				_random = value ?? new Random();
			}
		}

		/// <summary>
		/// Получает математическое ожидаение распределения.
		/// </summary>
		public double Mean => _lambda;

		/// <summary>
		/// Получает среднее квадратичное отклонение.
		/// </summary>
		public double StdDev => Math.Sqrt(_lambda);

		/// <summary>
		/// Получает дисперсию распределения.
		/// </summary>
		public double Variance => _lambda;

		/// <summary>
		/// Получает энтропию распределения.
		/// </summary>
		/// <remarks>https://en.wikipedia.org/wiki/Poisson_distribution</remarks>
		public double Entropy => 0.5 * Math.Log(17.079468445347132 * _lambda) - 1.0 / (12.0 * _lambda) - 1.0 / (24.0 * _lambda * _lambda) - 19.0 / (360.0 * _lambda * _lambda * _lambda);

		/// <summary>
		/// Получает асимметрию распределения.
		/// </summary>
		public double Skewness => 1.0 / Math.Sqrt(_lambda);

		/// <summary>
		/// Получает наименьший элемент в области распределения, который может быть представлен
		/// целым числом.
		/// </summary>
		public int Minimum => 0;

		/// <summary>
		/// Получает самый большой элемент в области распределений, который может быть представлен
		/// целым числом.
		/// </summary>
		public int Maximum => int.MaxValue;

		/// <summary>
		/// Получает медиану распределения.
		/// </summary>
		/// <remarks>https://en.wikipedia.org/wiki/Poisson_distribution</remarks>
		public double Median => Math.Floor(_lambda + 0.33333333333333331 - 0.02 / _lambda);

		/// <summary>
		/// Инициализирует новый экземпляр класса Poisson.
		/// </summary>
		/// <param name="lambda">Параметр лямбда (λ) распределения Пуассона. Диапазон: λ > 0.</param>
		/// <exception cref="ArgumentException">Если лямбда равна или меньше 0.</exception>
		public Poisson(double lambda)
		{
			if (!IsValidParameterSet(lambda))
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			_random = new Random();
			_lambda = lambda;
		}

		/// <summary>
		/// Инициализирует новый экземпляр класса Poisson.
		/// </summary>
		/// <param name="lambda">Параметр лямбда (λ) распределения Пуассона. Диапазон: λ > 0.</param>
		/// <param name="randomSource">Генератор случайных чисел, используемый для получения случайных выборок.</param>
		/// <exception cref="ArgumentException">Если лямбда равна или меньше 0.</exception>
		public Poisson(double lambda, System.Random randomSource)
		{
			if (!IsValidParameterSet(lambda))
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			_random = randomSource ?? new Random();
			_lambda = lambda;
		}

		/// <summary>
		/// Создаёт строковое представление выборки.
		/// </summary>
		/// <returns>строковое представление выборки.</returns>
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Poisson(lambda = ");
			defaultInterpolatedStringHandler.AppendFormatted(_lambda);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		/// <summary>
		/// Проверяет, являются ли предоставленные значения допустимыми параметрами этого распределения.
		/// </summary>
		/// <param name="lambda">Параметр лямбда (λ) распределения Пуассона. Диапазон: λ > 0.</param>
		/// <returns>true - допустимые, false - не допустимые.</returns>
		public static bool IsValidParameterSet(double lambda)
		{
			return lambda > 0.0;
		}

		/// <summary>
		/// Вычисляет значение функции вероятности (PMF) в точке k, т.е. P(X = k).
		/// </summary>
		/// <param name="k">Оценочная точка</param>
		/// <returns>вероятность (PMF) в точке k.</returns>
		public double Probability(int k)
		{
			return PMF(_lambda, k);
		}

		/// <summary>
		/// Вычисляет значение функции распределения (CDF) в точке x, т.е. P(X ≤ x).
		/// </summary>
		/// <param name="x">Оценочная точка.</param>
		/// <returns>распределение (CDF) в точке x.</returns>
		public double CumulativeDistribution(double x)
		{
			return CDF(_lambda, x);
		}

		/// <summary>
		/// Вычисляет значение функции вероятности (PMF) в точке k, т.е. P(X = k).
		/// </summary>
		/// <param name="lambda">Параметр лямбда (λ) распределения Пуассона. Диапазон: λ > 0.</param>
		/// <param name="k">Оценочная точка.</param>
		/// <returns>распределение (CDF) в точке x.</returns>
		/// <exception cref="ArgumentException">Если лямбда равна или меньше 0.</exception>
		public static double PMF(double lambda, int k)
		{
			if (!(lambda > 0.0))
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			var result = Math.Exp(-lambda) * Math.Pow(lambda, k) / SpecialFunctions.Factorial(k);

			return (result < 0) ? 0 : result;
		}

		/// <summary>
		/// Вычисляет значение функции распределения (CDF) в точке x, т.е. P(X ≤ x).
		/// </summary>
		/// <param name="lambda">Параметр лямбда (λ) распределения Пуассона. Диапазон: λ > 0.</param>
		/// <param name="x">Оценочная точка.</param>
		/// <returns>значение функции распределения (CDF) в точке x.</returns>
		/// <exception cref="ArgumentException">Если лямбда равна или меньше 0.</exception>
		public static double CDF(double lambda, double x)
		{
			if (!(lambda > 0.0))
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			double result = 0;
			for (int i = 0; i <= x; ++i)
			{
				var ttt = PMF(lambda, i);
				result += PMF(lambda, i);
			}

			return (result > 1) ? 1 : result;
		}
	}
}
