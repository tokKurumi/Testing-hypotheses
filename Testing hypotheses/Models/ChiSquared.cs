using Meta.Numerics.Functions;
using Modeling_sample.Distributions.Interfaces;
using System.Runtime.CompilerServices;

namespace Univariate_distributions.Models
{
	public class ChiSquared : IContinuousDistribution, IUnivariateDistribution, IDistribution
	{
		private System.Random _random;

		private readonly double _freedom;

		/// <summary>
		/// Получает степень свободы (k) для Хи-квадрат распределения. Диапозон: k > 0.
		/// </summary>
		public double DegreesOfFreedom => _freedom;

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
		public double Mean => _freedom;

		/// <summary>
		/// Получает дисперсию распределения.
		/// </summary>
		public double Variance => 2.0 * _freedom;

		/// <summary>
		/// Получает среднее квадратичное отклонение.
		/// </summary>
		public double StdDev => Math.Sqrt(2.0 * _freedom);

		/// <summary>
		/// Получает энтропию распределения.
		/// </summary>
		public double Entropy => _freedom / 2.0 + Math.Log(2.0 * AdvancedMath.Gamma(_freedom / 2.0)) + (1.0 - _freedom / 2.0) * AdvancedMath.Psi(_freedom / 2.0);

		/// <summary>
		/// Получает асимметрию распределения.
		/// </summary>
		public double Skewness => Math.Sqrt(8.0 / _freedom);

		/// <summary>
		/// Получает медиану распределения.
		/// </summary>
		public double Median => _freedom - 2.0 / 3.0;

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
		/// <param name="freedom">Cтепень свободы (k) для Хи-квадрат распределения. Диапозон: k > 0.</param>
		/// <exception cref="ArgumentException">Если степень свободы (k) ≤ 0.</exception>
		public ChiSquared(double freedom)
		{
			if (!IsValidParameterSet(freedom))
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			_random = new Random();
			_freedom = freedom;
		}

		/// <summary>
		/// Инициализирует новый экземпляр класса Exponential.
		/// </summary>
		/// <param name="freedom">Cтепень свободы (k) для Хи-квадрат распределения. Диапозон: k > 0.</param>
		/// <param name="randomSource">Генератор случайных чисел.</param>
		/// <exception cref="ArgumentException">Если степень свободы (k) ≤ 0.</exception>
		public ChiSquared(double freedom, System.Random randomSource)
		{
			if (!IsValidParameterSet(freedom))
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			_random = randomSource ?? new Random();
			_freedom = freedom;
		}

		/// <summary>
		/// Создаёт строковое представление выборки.
		/// </summary>
		/// <returns>строковое представление выборки.</returns>
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(16, 1);
			defaultInterpolatedStringHandler.AppendLiteral("ChiSquared(k = ");
			defaultInterpolatedStringHandler.AppendFormatted(_freedom);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		/// <summary>
		/// Проверяет, являются ли предоставленные значения допустимыми параметрами этого распределения.
		/// </summary>
		/// <param name="freedom">Cтепень свободы (k) для Хи-квадрат распределения. Диапозон: k > 0.</param>
		/// <returns>true - допустимые, false - не допустимые.</returns>
		public static bool IsValidParameterSet(double freedom)
		{
			return freedom > 0.0;
		}

		/// <summary>
		/// Вычисляет плотность выборки (PDF) в точке x, т.е. ∂P(X ≤ x)/∂x.
		/// </summary>
		/// <param name="x">Оценочная точка.</param>
		/// <returns>плотность выборки (PDF) в точке x.</returns>
		public double Density(double x)
		{
			return PDF(_freedom, x);
		}

		/// <summary>
		/// Вычисляет логирифм плотности выборки (lnPDF) в точке x, т.к. ln(∂P(X ≤ x)/∂x).
		/// </summary>
		/// <param name="x">Оценочная точка.</param>
		/// <returns>логирифм плотности выборки (lnPDF) в точке x.</returns>
		public double DensityLn(double x)
		{
			return PDFLn(_freedom, x);
		}

		/// <summary>
		/// Вычисляет значение функции распределения (CDF) в точке x, т.е. P(X ≤ x).
		/// </summary>
		/// <param name="x">Оценочная точка.</param>
		/// <returns>распределение (CDF) в точке x.</returns>
		public double CumulativeDistribution(double x)
		{
			return CDF(_freedom, x);
		}

		/// <summary>
		/// Вычисляет значение обратной функции распределения (InvCDF) в точке x, т.е. P(X ≤ x).
		/// </summary>
		/// <param name="p">Оценочная точка.</param>
		/// <returns>распределение (CDF) в точке x.</returns>
		public double InverseCumulativeDistribution(double p)
		{
			return InvCDF(_freedom, p);
		}

		/// <summary>
		/// Вычисляет значение функции плотности вероятности (PDF) в точке x, т.е. ∂P(X ≤ x)/∂x.
		/// </summary>
		/// <param name="freedom">Cтепень свободы (k) для Хи-квадрат распределения. Диапозон: k > 0.</param>
		/// <param name="x">Оценочная точка.</param>
		/// <returns>значение функции плотности вероятности (PDF) в точке x.</returns>
		/// <exception cref="ArgumentException">Если степень свободы (k) ≤ 0.</exception>
		public static double PDF(double freedom, double x)
		{
			if (freedom <= 0.0)
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			if (double.IsPositiveInfinity(freedom) || double.IsPositiveInfinity(x) || x == 0.0)
			{
				return 0.0;
			}

			if (freedom > 160.0)
			{
				return Math.Exp(PDFLn(freedom, x));
			}

			return Math.Pow(x, freedom / 2.0 - 1.0) * Math.Exp((0.0 - x) / 2.0) / (Math.Pow(2.0, freedom / 2.0) * AdvancedMath.Gamma(freedom / 2.0));
		}

		/// <summary>
		/// Вычисляет логирифм плотности выборки (lnPDF) в точке x, т.к. ln(∂P(X ≤ x)/∂x).
		/// </summary>
		/// <param name="freedom">Cтепень свободы (k) для Хи-квадрат распределения. Диапозон: k > 0.</param>
		/// <param name="x">Оценочная точка.</param>
		/// <returns>логирифм плотности выборки (lnPDF) в точке x.</returns>
		public static double PDFLn(double freedom, double x)
		{
			if (freedom <= 0.0)
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			if (double.IsPositiveInfinity(freedom) || double.IsPositiveInfinity(x) || x == 0.0)
			{
				return double.NegativeInfinity;
			}

			return (0.0 - x) / 2.0 + (freedom / 2.0 - 1.0) * Math.Log(x) - freedom / 2.0 * Math.Log(2.0) - AdvancedMath.LogGamma(freedom / 2.0);
		}

		/// <summary>
		/// Вычисляет значение функции распределения (CDF) в точке x, т.е. P(X ≤ x).
		/// </summary>
		/// <param name="freedom">Cтепень свободы (k) для Хи-квадрат распределения. Диапозон: k > 0.</param>
		/// <param name="x">Оценочная точка.</param>
		/// <returns>распределение (CDF) в точке x.</returns>
		/// <exception cref="ArgumentException">Если степень свободы (k) ≤ 0.</exception>
		public static double CDF(double freedom, double x)
		{
			if (freedom <= 0.0)
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			if (double.IsPositiveInfinity(x))
			{
				return 1.0;
			}

			if (double.IsPositiveInfinity(freedom))
			{
				return 1.0;
			}

			return AdvancedMath.LeftRegularizedGamma(freedom / 2.0, x / 2.0);
		}

		/// <summary>
		/// Вычисляет значение обратной функции распределения (InvCDF) в точке x, т.е. P(X ≤ x).
		/// </summary>
		/// <param name="freedom">Cтепень свободы (k) для Хи-квадрат распределения. Диапозон: k > 0.</param>
		/// <param name="p">Оценочная точка.</param>
		/// <returns>распределение (CDF) в точке x.</returns>
		/// <exception cref="ArgumentException">Если степень свободы (k) ≤ 0.</exception>
		public static double InvCDF(double freedom, double p)
		{
			return MathNet.Numerics.Distributions.ChiSquared.InvCDF(freedom, p);
		}
	}
}
