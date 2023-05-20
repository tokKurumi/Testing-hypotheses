using System.Runtime.CompilerServices;
using Modeling_sample.Distributions.Interfaces;

namespace Univariate_distributions.Models
{
	class ContinuousUniform : IContinuousDistribution, IUnivariateDistribution, IDistribution
	{
		private System.Random _random;

		private readonly double _lower;

		private readonly double _upper;

		/// <summary>
		/// Получает нижнюю границу распределения.
		/// </summary>
		public double LowerBound => _lower;

		/// <summary>
		/// Получает верхнюю границу распределения.
		/// </summary>
		public double UpperBound => _upper;

		/// <summary>
		/// Получает или принимает генератор случайных чисел выборки.
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
		public double Mean => (_lower + _upper) / 2.0;

		/// <summary>
		/// Получает дисперсию распределения.
		/// </summary>
		public double Variance => (_upper - _lower) * (_upper - _lower) / 12.0;

		/// <summary>
		/// Получает среднее квадратичное отклонение.
		/// </summary>
		public double StdDev => (_upper - _lower) / Math.Sqrt(12.0);

		/// <summary>
		/// Получает энтропию распределения.
		/// </summary>.
		public double Entropy => Math.Log(_upper - _lower);

		/// <summary>
		/// Получает асимметрию распределения.
		/// </summary>
		public double Skewness => 0.0;

		/// <summary>
		/// Получает медиану распределения.
		/// </summary>
		public double Median => (_lower + _upper) / 2.0;

		/// <summary>
		/// Получает наименьший элемент в области распределения, который может быть представлен
		/// целым числом.
		/// </summary>
		public double Minimum => _lower;

		/// <summary>
		/// Получает самый большой элемент в области распределений, который может быть представлен
		/// целым числом.
		/// </summary>
		public double Maximum => _upper;

		/// <summary>
		/// Инициализирует новый экземпляр класса ContinuousUniform.
		/// LoverBound = 0, UpperBound = 1.
		/// </summary>
		public ContinuousUniform()
			: this(0.0, 1.0)
		{ }

		/// <summary>
		/// Инициализирует новый экземпляр класса ContinuousUniform.
		/// </summary>
		/// <param name="lower">Нижняя граница. Диапозон: lower ≤ upper</param>
		/// <param name="upper">Верхняя граница. Диапозон: lower ≤ upper</param>
		/// <exception cref="ArgumentException">Если нижняя граница больше верхней границы.</exception>
		public ContinuousUniform(double lower, double upper)
		{
			if (!IsValidParameterSet(lower, upper))
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			_random = new Random();
			_lower = lower;
			_upper = upper;
		}

		/// <summary>
		/// Инициализирует новый экземпляр класса ContinuousUniform.
		/// </summary>
		/// <param name="lower">Нижняя граница. Диапозон: lower ≤ upper</param>
		/// <param name="upper">Верхняя граница. Диапозон: lower ≤ upper</param>
		/// <param name="randomSource">Генератор случайных чисел.</param>
		/// <exception cref="ArgumentException">Если нижняя граница больше верхней границы.</exception>
		public ContinuousUniform(double lower, double upper, System.Random randomSource)
		{
			if (!IsValidParameterSet(lower, upper))
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			_random = randomSource ?? new Random();
			_lower = lower;
			_upper = upper;
		}

		/// <summary>
		/// Создаёт строковое представление выборки.
		/// </summary>
		/// <returns>строковое представление выборки.</returns>
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 2);
			defaultInterpolatedStringHandler.AppendLiteral("ContinuousUniform(Lower = ");
			defaultInterpolatedStringHandler.AppendFormatted(_lower);
			defaultInterpolatedStringHandler.AppendLiteral(", Upper = ");
			defaultInterpolatedStringHandler.AppendFormatted(_upper);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		/// <summary>
		/// Проверяет, являются ли предоставленные значения допустимыми параметрами этого распределения.
		/// </summary>
		/// <param name="lower">Нижняя граница. Диапозон: lower ≤ upper</param>
		/// <param name="upper">Верхняя граница. Диапозон: lower ≤ upper</param>
		/// <returns>true - допустимые, false - не допустимые.</returns>
		public static bool IsValidParameterSet(double lower, double upper)
		{
			return lower <= upper;
		}

		/// <summary>
		/// Вычисляет значение функции распределения (CDF) в точке x, т.е. P(X ≤ x).
		/// </summary>
		/// <param name="x">Оценочная точка.</param>
		/// <returns>распределение (CDF) в точке x.</returns>
		public double CumulativeDistribution(double x)
		{
			return CDF(_lower, _upper, x);
		}

		/// <summary>
		/// Вычисляет значение функции вероятности (PDF) в точке k, т.е. P(X = k).
		/// </summary>
		/// <param name="k">Оценочная точка.</param>
		/// <returns>вероятность (PMF) в точке k.</returns>
		public double Density(double k)
		{
			return PDF(_lower, _upper, k);
		}

		/// <summary>
		/// Вычисляет значение обратной функции распределения (InvCDF) в точке x, т.е. P(X ≤ x).
		/// </summary>
		/// <param name="p">Оценочная точка.</param>
		/// <returns>распределение (CDF) в точке x.</returns>
		public double InverseCumulativeDistribution(double p)
		{
			return InvCDF(_lower, _upper, p);
		}

		/// <summary>
		/// Вычисляет значение функции плотности вероятности (PDF) в точке x, т.е. ∂P(X ≤ x)/∂x.
		/// </summary>
		/// <param name="lower">Нижняя граница. Диапозон: lower ≤ upper.</param>
		/// <param name="upper">Верхняя граница. Диапозон: lower ≤ upper.</param>
		/// <param name="x">Оценочная точка.</param>
		/// <returns>значение функции плотности вероятности (PDF) в точке x</returns>
		/// <exception cref="ArgumentException">Если нижняя граница больше верхней границы.</exception>
		public static double PDF(double lower, double upper, double x)
		{
			if (upper < lower)
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			if (!(x < lower) && !(x > upper))
			{
				return 1.0 / (upper - lower);
			}

			return 0.0;
		}

		/// <summary>
		/// Вычисляет значение функции распределения (CDF) в точке x, т.е. P(X ≤ x).
		/// </summary>
		/// <param name="lower">Нижняя граница. Диапозон: lower ≤ upper</param>
		/// <param name="upper">Верхняя граница. Диапозон: lower ≤ upper</param>
		/// <param name="x">Оценочная точка.</param>
		/// <returns>распределение (CDF) в точке x.</returns>
		/// <exception cref="ArgumentException">Если нижняя граница больше верхней границы.</exception>
		public static double CDF(double lower, double upper, double x)
		{
			if (upper < lower)
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			if (!(x <= lower))
			{
				if (!(x >= upper))
				{
					return (x - lower) / (upper - lower);
				}

				return 1.0;
			}

			return 0.0;
		}

		/// <summary>
		/// Вычисляет значение обратной функции распределения (InvCDF) в точке x, т.е. P(X ≤ x).
		/// </summary>
		/// <param name="lower">Нижняя граница. Диапозон: lower ≤ upper</param>
		/// <param name="upper">Верхняя граница. Диапозон: lower ≤ upper</param>
		/// <param name="p">Оценочная точка.</param>
		/// <returns>распределение (CDF) в точке x.</returns>
		/// <exception cref="ArgumentException">Если нижняя граница больше верхней границы.</exception>
		public static double InvCDF(double lower, double upper, double p)
		{
			if (upper < lower)
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			if (!(p <= 0.0))
			{
				if (!(p >= 1.0))
				{
					return lower * (1.0 - p) + upper * p;
				}

				return upper;
			}

			return lower;
		}
	}
}
