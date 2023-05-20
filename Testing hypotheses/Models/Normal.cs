using System.Runtime.CompilerServices;
using Meta.Numerics.Functions;
using Modeling_sample.Distributions.Interfaces;

namespace Univariate_distributions.Models
{
	public class Normal : IContinuousDistribution, IUnivariateDistribution, IDistribution
	{
		private System.Random _random;

		private readonly double _mean;

		private readonly double _stdDev;

		/// <summary>
		/// Получает математическое ожидаение (μ) распределения.
		/// </summary>
		public double Mean => _mean;

		/// <summary>
		/// Получает среднее квадратичное отклонение (σ). Диапозон: σ ≥ 0.
		/// </summary>
		public double StdDev => _stdDev;

		/// <summary>
		/// Получает дисперсию распределения.
		/// </summary>
		public double Variance => _stdDev * _stdDev;

		/// <summary>
		/// Получает точность нормального распределенияю
		/// </summary>
		public double Precision => 1.0 / (_stdDev * _stdDev);

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
		/// Получает энтропию распределения.
		/// </summary>
		public double Entropy => Math.Log(_stdDev) + 1.4189385332046727;

		/// <summary>
		/// Получает асимметрию распределения.
		/// </summary>
		public double Skewness => 0.0;

		/// <summary>
		/// Получает медиану распределения.
		/// </summary>
		public double Median => _mean;

		/// <summary>
		/// Получает наименьший элемент в области распределения, который может быть представлен
		/// целым числом.
		/// </summary>
		public double Minimum => double.NegativeInfinity;

		/// <summary>
		/// Получает самый большой элемент в области распределений, который может быть представлен
		/// целым числом.
		/// </summary>
		public double Maximum => double.PositiveInfinity;

		/// <summary>
		/// Инициализирует новый экземпляр класса Summary.
		/// mean = 0.0, stddev = 1.0.
		/// </summary>
		public Normal()
			: this(0.0, 1.0)
		{ }

		/// <summary>
		/// Инициализирует новый экземпляр класса Summary.
		/// mean = 0.0, stddev = 1.0.
		/// </summary>
		/// <param name="randomSource">Генератор случайных чисел.</param>
		public Normal(System.Random randomSource)
			: this(0.0, 1.0, randomSource)
		{ }

		/// <summary>
		/// Инициализирует новый экземпляр класса Summary.
		/// </summary>
		/// <param name="mean">Математическое ожидаение (μ) распределения.</param>
		/// <param name="stddev">Cреднее квадратичное отклонение (σ). Диапозон: σ ≥ 0.</param>
		/// <exception cref="ArgumentException">Если квадратичное отклонение (σ) < 0.</exception>
		public Normal(double mean, double stddev)
		{
			if (!IsValidParameterSet(mean, stddev))
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			_random = new Random();
			_mean = mean;
			_stdDev = stddev;
		}

		/// <summary>
		/// Инициализирует новый экземпляр класса Summary.
		/// </summary>
		/// <param name="mean">Математическое ожидаение (μ) распределения.</param>
		/// <param name="stddev">Cреднее квадратичное отклонение (σ). Диапозон: σ ≥ 0.</param>
		/// <param name="randomSource">Генератор случайных чисел.</param>
		/// <exception cref="ArgumentException">Если квадратичное отклонение (σ) < 0.</exception>
		public Normal(double mean, double stddev, System.Random randomSource)
		{
			if (!IsValidParameterSet(mean, stddev))
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			_random = randomSource ?? new Random();
			_mean = mean;
			_stdDev = stddev;
		}

		/// <summary>
		/// Создаёт строковое представление выборки.
		/// </summary>
		/// <returns>строковое представление выборки.</returns>
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Normal(Mu = ");
			defaultInterpolatedStringHandler.AppendFormatted(_mean);
			defaultInterpolatedStringHandler.AppendLiteral(", Sigma = ");
			defaultInterpolatedStringHandler.AppendFormatted(_stdDev);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		/// <summary>
		/// Проверяет, являются ли предоставленные значения допустимыми параметрами этого распределения.
		/// </summary>
		/// <param name="mean">Математическое ожидаение (μ) распределения.</param>
		/// <param name="stddev">Cреднее квадратичное отклонение (σ). Диапозон: σ ≥ 0.</param>
		/// <returns>true - допустимые, false - не допустимые.</returns>
		public static bool IsValidParameterSet(double mean, double stddev)
		{
			if (stddev >= 0.0)
			{
				return !double.IsNaN(mean);
			}

			return false;
		}

		/// <summary>
		/// Вычисляет значение функции вероятности (PDF) в точке k, т.е. P(X = k).
		/// </summary>
		/// <param name="k">Оценочная точка.</param>
		/// <returns>вероятность (PMF) в точке k.</returns>
		public double Density(double k)
		{
			return PDF(_mean, _stdDev, k);
		}

		/// <summary>
		/// Вычисляет значение функции распределения (CDF) в точке x, т.е. P(X ≤ x).
		/// </summary>
		/// <param name="x">Оцениваемая точка.</param>
		/// <returns>значение функции распределения (CDF) в точке x.</returns>
		public double CumulativeDistribution(double x)
		{
			return CDF(_mean, _stdDev, x);
		}

		/// <summary>
		/// Вычисляет значение обратной функции распределения (CDF) в точке x, т.е. P(X ≤ x).
		/// </summary>
		/// <param name="p">Оцениваемая точка.</param>
		/// <returns>значение функции распределения (CDF) в точке x.</returns>
		/// <exception cref="ArgumentException">Если квадратичное отклонение (σ) < 0.</exception>
		/// <remarks>MATLAB: norminv</remarks>
		public double InverseCumulativeDistribution(double p)
		{
			return InvCDF(_mean, _stdDev, p);
		}

		/// <summary>
		/// Вычисляет значение функции плотности вероятности (PDF) в точке x, т.е. ∂P(X ≤ x)/∂x.
		/// </summary>
		/// <param name="mean">Математическое ожидаение (μ) распределения.</param>
		/// <param name="stddev">Среднее квадратичное отклонение (σ). Диапозон: σ ≥ 0.</param>
		/// <param name="x">Точка для оценки.</param>
		/// <returns>значение функции плотности вероятности (PDF) в точке x</returns>
		/// <exception cref="ArgumentException">Если квадратичное отклонение (σ) < 0.</exception>
		/// <remarks>MATLAB: normpdf</remarks>
		public static double PDF(double mean, double stddev, double x)
		{
			if (stddev < 0.0)
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			//double num = (k - _mean) / _stdDev;
			//return Math.Exp(-0.5 * num * num) / (2.5066282746310007 * _stdDev);

			return (1.0 / (stddev * Math.Sqrt(2 * Math.PI))) * Math.Exp(-((x - mean) * (x - mean) / (2 * stddev * stddev)));
		}

		/// <summary>
		/// Вычисляет значение функции распределения (CDF) в точке x, т.е. P(X ≤ x).
		/// </summary>
		/// <param name="mean">Математическое ожидаение (μ) распределения.</param>
		/// <param name="stddev">Среднее квадратичное отклонение (σ). Диапозон: σ ≥ 0.</param>
		/// <param name="x">Оцениваемая точка.</param>
		/// <returns>значение функции распределения (CDF) в точке x.</returns>
		/// <exception cref="ArgumentException">Если квадратичное отклонение (σ) < 0.</exception>
		/// <remarks>MATLAB: normcdf</remarks>
		public static double CDF(double mean, double stddev, double x)
		{
			if (stddev < 0.0)
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			return 0.5 * AdvancedMath.Erfc((mean - x) / (stddev * 1.4142135623730951));
		}

		/// <summary>
		/// Вычисляет значение обратной функции распределения (CDF) в точке x, т.е. P(X ≤ x).
		/// </summary>
		/// <param name="mean">Математическое ожидаение (μ) распределения.</param>
		/// <param name="stddev">Среднее квадратичное отклонение (σ). Диапозон: σ ≥ 0.</param>
		/// <param name="p">Оцениваемая точка.</param>
		/// <returns>значение функции распределения (CDF) в точке x.</returns>
		/// <exception cref="ArgumentException">Если квадратичное отклонение (σ) < 0.</exception>
		/// <remarks>MATLAB: norminv</remarks>
		public static double InvCDF(double mean, double stddev, double p)
		{
			return MathNet.Numerics.Distributions.Normal.InvCDF(mean, stddev, p);
		}

		private static bool PolarTransform(double a, double b, out double x, out double y)
		{
			double num = 2.0 * a - 1.0;
			double num2 = 2.0 * b - 1.0;
			double num3 = num * num + num2 * num2;
			if (num3 >= 1.0 || num3 == 0.0)
			{
				x = 0.0;
				y = 0.0;
				return false;
			}

			double num4 = Math.Sqrt(-2.0 * Math.Log(num3) / num3);
			x = num * num4;
			y = num2 * num4;
			return true;
		}

		/// <summary>
		/// Создаёт случайное значение нормального распределения.
		/// </summary>
		/// <param name="mean">Математическое ожидаение (μ) распределения.</param>
		/// <param name="stddev">Среднее квадратичное отклонение (σ). Диапозон: σ ≥ 0.</param>
		/// <returns>случайное значение нормального распределения.</returns>
		/// <exception cref="ArgumentException">Если квадратичное отклонение (σ) < 0.</exception>
		public static double Sample(double mean, double stddev)
		{
			var rnd = new Random();

			double x;
			double y;
			while (!PolarTransform(rnd.NextDouble(), rnd.NextDouble(), out x, out y))
			{
			}

			return mean + stddev * x;
		}

		/// <summary>
		/// Создаёт случайное значение нормального распределения.
		/// </summary>
		/// <returns>случайное значение нормального распределения.</returns>
		/// <exception cref="ArgumentException">Если квадратичное отклонение (σ) < 0.</exception>
		public double Sample()
		{
			return Sample(_mean, _stdDev);
		}

		/// <summary>
		/// Создаёт выборку случайных значений нормального распределения с заданными параметрами.
		/// </summary>
		/// <param name="mean">Математическое ожидаение (μ) распределения.</param>
		/// <param name="stddev">Среднее квадратичное отклонение (σ). Диапозон: σ ≥ 0.</param>
		/// <param name="values">Массив для заполнения.</param>
		public static void Samples(double mean, double stddev, double[] values)
		{
			Parallel.For(0, values.Length, i =>
			{
				values[i] = Sample(mean, stddev);
			});
		}

		/// <summary>
		/// Создаёт выборку случайных значений нормального распределения с заданными параметрами.
		/// </summary>
		/// <param name="values">Массив для заполнения.</param>
		public void Samples(double[] values)
		{
			Samples(_mean, _stdDev, values);
		}

		/// <summary>
		/// Создаёт выборку случайных значений нормального распределения с заданными параметрами.
		/// </summary>
		/// <param name="mean">Математическое ожидаение (μ) распределения.</param>
		/// <param name="stddev">Среднее квадратичное отклонение (σ). Диапозон: σ ≥ 0.</param>
		/// <param name="count">Количество случайных величин.</param>
		/// <returns>выборка случайных значений нормального распределения.</returns>
		public static double[] Samples(double mean, double stddev, int count)
		{
			var result = new double[count];
			Samples(mean, stddev, result);

			return result;
		}

		/// <summary>
		/// Создаёт выборку случайных значений нормального распределения с заданными параметрами.
		/// </summary>
		/// <param name="count">Количество случайных величин.</param>
		/// <returns>выборка случайных значений нормального распределения.</returns>
		public double[] Samples(int count)
		{
			return Samples(_mean, _stdDev, count);
		}

		/// <summary>
		/// Вычисляет доверительный интервал математического ожидания экспоненциального распределения.
		/// </summary>
		/// <param name="samples">Значения экспоненциального распределения.</param>
		/// <param name="p">Вероятность (p) в допустимом диапозоне. Диапозон: 0 ≤ p ≤ 1.</param>
		/// <returns>доверительный интервал экспоненциального распределения.</returns>
		/// <remarks>MATLAB: expfit</remarks>
		public static (double lower, double upper) Estemate1(IEnumerable<double> samples, double p)
		{
			double h = PointEstemate1(samples);
			double S = PointEstemate2(samples);

			double lower = h + StudentT.InvCDF(0, 1, samples.Count() - 1, (1 - p) / 2) * S / Math.Sqrt(samples.Count());
			double upper = h - StudentT.InvCDF(0, 1, samples.Count() - 1, (1 - p) / 2) * S / Math.Sqrt(samples.Count());

			return (lower, upper);
		}

		/// <summary>
		/// Вычисляет доверительный интервал среднеквадратичного отклонения экспоненциального распределения.
		/// </summary>
		/// <param name="samples">Значения экспоненциального распределения.</param>
		/// <param name="p">Вероятность (p) в допустимом диапозоне. Диапозон: 0 ≤ p ≤ 1.</param>
		/// <remarks>MATLAB: expfit</remarks>
		/// <returns>доверительный интервал среднеквадратичного отклонения экспоненциального распределения.</returns>
		public static (double lower, double upper) Estemate2(IEnumerable<double> samples, double p)
		{
			double S = PointEstemate2(samples);

			double lower = S * Math.Sqrt(samples.Count() / ChiSquared.InvCDF(samples.Count() - 1, p / 2));
			double upper = S * Math.Sqrt(samples.Count() / ChiSquared.InvCDF(samples.Count() - 1, (1 - p) / 2));

			return (lower, upper);
		}

		/// <summary>
		/// Вычисляет точечную оценку математического ожидания нормального распределения.
		/// </summary>
		/// <param name="samples">Значения экспоненциального распределения.</param>
		/// <remarks>MATLAB: muhat</remarks>
		/// <returns>точечная оценка математического ожидания нормального распределения.</returns>
		public static double PointEstemate1(IEnumerable<double> samples)
		{
			return samples.Average();
		}

		/// <summary>
		/// Вычисляет точечную оценку среднеквадратичного отклонения нормального распределения.
		/// </summary>
		/// <param name="samples">Значения экспоненциального распределения.</param>
		/// <remarks>MATLAB: sigmahat</remarks>
		/// <returns>точечная оценка среднеквадратичного отклонения нормального распределения.</returns>
		public static double PointEstemate2(IEnumerable<double> samples)
		{
			var mean = PointEstemate1(samples);

			return Math.Sqrt(samples.Sum(x => (x - mean) * (x - mean)) / (samples.Count() - 1));
		}
	}
}