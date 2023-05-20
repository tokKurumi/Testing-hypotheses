using System.Runtime.CompilerServices;
using Modeling_sample.Distributions.Interfaces;

namespace Univariate_distributions.Models
{
	public class Exponential : IContinuousDistribution, IUnivariateDistribution, IDistribution
	{
		private System.Random _random;

		private readonly double _rate;

		/// <summary>
		/// Получает параметр скорости (λ) распределения. Диапозон: λ ≥ 0.
		/// </summary>
		public double Rate => _rate;

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
		public double Mean => 1.0 / _rate;

		/// <summary>
		/// Получает дисперсию распределения.
		/// </summary>
		public double Variance => 1.0 / (_rate * _rate);

		/// <summary>
		/// Получает среднее квадратичное отклонение.
		/// </summary>
		public double StdDev => 1.0 / _rate;

		/// <summary>
		/// Получает энтропию распределения.
		/// </summary>
		public double Entropy => 1.0 - Math.Log(_rate);

		/// <summary>
		/// Получает асимметрию распределения.
		/// </summary>
		public double Skewness => 2.0;

		/// <summary>
		/// Получает медиану распределения.
		/// </summary>
		public double Median => Math.Log(2.0) / _rate;

		/// <summary>
		/// Получает наименьший элемент в области распределения, который может быть представлен
		/// целым числом.
		/// </summary>
		public double Minimum => 0.0;

		/// <summary>
		/// Получает самый большой элемент в области распределений, который может быть представлен
		/// целым числом.
		/// </summary>
		public double Maximum => double.PositiveInfinity;

		/// <summary>
		/// Инициализирует новый экземпляр класса Exponential.
		/// </summary>
		/// <param name="rate">Скорость распределения (λ). Диапозон: λ ≥ 0.</param>
		/// <exception cref="ArgumentException">Если скорость распределения (λ) меньше или 0.</exception>
		public Exponential(double rate)
		{
			if (!IsValidParameterSet(rate))
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			_random = new Random();
			_rate = rate;
		}

		/// <summary>
		/// Инициализирует новый экземпляр класса Exponential.
		/// </summary>
		/// <param name="rate">Скорость распределения (λ). Диапозон: λ ≥ 0.</param>
		/// <param name="randomSource">Генератор случайных чисел.</param>
		/// <exception cref="ArgumentException">Если скорость распределения (λ) меньше или 0.</exception>
		public Exponential(double rate, System.Random randomSource)
		{
			if (!IsValidParameterSet(rate))
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			_random = randomSource ?? new Random();
			_rate = rate;
		}

		/// <summary>
		/// Создаёт строковое представление выборки.
		/// </summary>
		/// <returns>строковое представление выборки.</returns>
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Exponential(lambda = ");
			defaultInterpolatedStringHandler.AppendFormatted(_rate);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		/// <summary>
		/// Проверяет, являются ли предоставленные значения допустимыми параметрами этого распределения.
		/// </summary>
		/// <param name="rate">Скорость распределения (λ). Диапозон: λ ≥ 0.</param>
		/// <returns>true - допустимые, false - не допустимые.</returns>
		public static bool IsValidParameterSet(double rate)
		{
			return rate >= 0.0;
		}

		/// <summary>
		/// Вычисляет плотность выборки (PDF) в точке x, т.е. ∂P(X ≤ x)/∂x.
		/// </summary>
		/// <param name="x">Оценочная точка.</param>
		/// <returns>плотность выборки (PDF) в точке x.</returns>
		public double Density(double x)
		{
			return PDF(_rate, x);
		}

		/// <summary>
		/// Вычисляет значение функции распределения (CDF) в точке x, т.е. P(X ≤ x).
		/// </summary>
		/// <param name="x">Оценочная точка.</param>
		/// <returns>распределение (CDF) в точке x.</returns>
		public double CumulativeDistribution(double x)
		{
			return CDF(_rate, x);
		}

		/// <summary>
		/// Вычисляет значение обратной функции распределения (InvCDF) в точке x, т.е. P(X ≤ x).
		/// </summary>
		/// <param name="p">Оценочная точка.</param>
		/// <returns>распределение (CDF) в точке x.</returns>
		public double InverseCumulativeDistribution(double p)
		{
			return InvCDF(_rate, p);
		}

		/// <summary>
		/// Вычисляет значение функции плотности вероятности (PDF) в точке x, т.е. ∂P(X ≤ x)/∂x.
		/// </summary>
		/// <param name="rate">Скорость распределения (λ). Диапозон: λ ≥ 0.</param>
		/// <param name="x">Оценочная точка.</param>
		/// <returns>значение функции плотности вероятности (PDF) в точке x</returns>
		/// <exception cref="ArgumentException">Если скорость распределения (λ) < 0.</exception>
		public static double PDF(double rate, double x)
		{
			if (rate < 0.0)
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			if (!(x < 0.0))
			{
				//return rate * Math.Exp((0.0 - rate) * x);
				return Math.Exp(-x / rate) / rate;
			}

			return 0.0;
		}

		/// <summary>
		/// Вычисляет значение функции распределения (CDF) в точке x, т.е. P(X ≤ x).
		/// </summary>
		/// <param name="rate">Скорость распределения (λ). Диапозон: λ ≥ 0.</param>
		/// <param name="x">Оценочная точка.</param>
		/// <returns>распределение (CDF) в точке x.</returns>
		/// <exception cref="ArgumentException">Если скорость распределения (λ) < 0.</exception>
		public static double CDF(double rate, double x)
		{
			if (rate < 0.0)
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			if (x >= 0.0)
			{
				return 1.0 - Math.Exp((0.0 - rate) * x);
			}

			return 0.0;
		}

		/// <summary>
		/// Вычисляет значение обратной функции распределения (InvCDF) в точке x, т.е. P(X ≤ x).
		/// </summary>
		/// <param name="rate">Скорость распределения (λ). Диапозон: λ ≥ 0.</param>
		/// <param name="p">Оценочная точка.</param>
		/// <returns>распределение (CDF) в точке x.</returns>
		/// <exception cref="ArgumentException">Если скорость распределения (λ) < 0.</exception>
		public static double InvCDF(double rate, double p)
		{
			if (rate < 0.0)
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			if (!(p >= 1.0))
			{
				return (0.0 - Math.Log(1.0 - p)) / rate;
			}

			return double.PositiveInfinity;
		}

		/// <summary>
		/// Создаёт случайное значение экспоненциального распределения.
		/// </summary>
		/// <param name="rate">Скорость распределения (λ). Диапозон: λ ≥ 0.</param>
		/// <returns>случайное значение экспоненциального распределения.</returns>
		/// <exception cref="ArgumentException">Если скорость распределения (λ) < 0.</exception>
		public static double Sample(double rate)
		{
			if (rate < 0.0)
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			return -Math.Log(1 - new Random().NextDouble()) / rate;
		}

		/// <summary>
		/// Создаёт случайное значение экспоненциального распределения.
		/// </summary>
		/// <returns>случайное значение экспоненциального распределения.</returns>
		/// <exception cref="ArgumentException">Если скорость распределения (λ) < 0.</exception>
		public double Sample()
		{
			return Sample(_rate);
		}

		/// <summary>
		/// Создаёт выборку случайных значений экспоненциального распределения с заданными параметрами.
		/// </summary>
		/// <param name="rate">Скорость распределения (λ). Диапозон: λ ≥ 0.</param>
		/// <param name="values">Массив для заполнения.</param>
		public static void Samples(double rate, double[] values)
		{
			Random rnd = new Random();
			Parallel.For(0, values.Length, i =>
			{
				lock (rnd)
				{
					values[i] = -Math.Log(1 - rnd.NextDouble()) / rate;
				}
			});
		}

		/// <summary>
		/// Создаёт выборку случайных значений экспоненциального распределения с заданными параметрами.
		/// </summary>
		/// <param name="rate">Скорость распределения (λ). Диапозон: λ ≥ 0.</param>
		/// <param name="count">Количество случайных величин.</param>
		/// <returns>выборка случайных значений экспоненциального распределения.</returns>
		public static double[] Samples(double rate, int count)
		{
			var results = new double[count];
			Samples(rate, results);

			return results;
		}

		/// <summary>
		/// Создаёт выборку случайных значений экспоненциального распределения с заданными параметрами.
		/// </summary>
		/// <param name="count">Количество случайных величин.</param>
		/// <returns>выборка случайных значений экспоненциального распределения.</returns>
		public double[] Samples(int count)
		{
			return Samples(_rate, count);
		}

		/// <summary>
		/// Вычисляет доверительный интервал экспоненциального распределения.
		/// </summary>
		/// <param name="samples">Значения экспоненциального распределения.</param>
		/// <param name="p">Вероятность (p) в допустимом диапозоне. Диапозон: 0 ≤ p ≤ 1.</param>
		/// <returns>доверительный интервал экспоненциального распределения.</returns>
		/// <remarks>MATLAB: expfit</remarks>
		public static (double lower, double upper) Estemate(IEnumerable<double> samples, double p)
		{
			double h = PointEstemate(samples);

			double lower = h * ChiSquared.InvCDF(2 * samples.Count(), p / 2) / (2 * (samples.Count() - 1));
			double upper = h * ChiSquared.InvCDF(2 * samples.Count(), (2 - p) / 2) / (2 * (samples.Count() - 1));

			return (lower, upper);
		}


		/// <summary>
		/// Вычисляет точечную оценку экспоненциального распределения.
		/// </summary>
		/// <param name="samples">Значения экспоненциального распределения.</param>
		/// <returns>точечная оценка экспоненциального распределения.</returns>
		/// <remarks>MATLAB: expfit</remarks>
		public static double PointEstemate(IEnumerable<double> samples)
		{
			return (samples.Count() - 1) / samples.Sum();
		}
	}
}
