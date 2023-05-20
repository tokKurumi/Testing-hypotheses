using Meta.Numerics.Functions;
using Meta.Numerics;
using System.Runtime.CompilerServices;
using MathNet.Numerics.Integration;
using Modeling_sample.Distributions.Interfaces;

namespace Univariate_distributions.Models
{
	public class FisherSnedecor : IContinuousDistribution, IUnivariateDistribution, IDistribution
	{
		private System.Random _random;

		private readonly double _freedom1;

		private readonly double _freedom2;

		/// <summary>
		/// Получает первую степень свободы (d1) распределения. Диапозон: d1 > 0.
		/// </summary>
		public double DegreesOfFreedom1 => _freedom1;

		/// <summary>
		/// Получает вторую степень свободы (d1) распределения. Диапозон: d1 > 0.
		/// </summary>
		public double DegreesOfFreedom2 => _freedom2;

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
		public double Mean
		{
			get
			{
				if (_freedom2 <= 2.0)
				{
					throw new NotSupportedException();
				}

				return _freedom2 / (_freedom2 - 2.0);
			}
		}

		/// <summary>
		/// Получает дисперсию распределения.
		/// </summary>
		public double Variance
		{
			get
			{
				if (_freedom2 <= 4.0)
				{
					throw new NotSupportedException();
				}

				return 2.0 * _freedom2 * _freedom2 * (_freedom1 + _freedom2 - 2.0) / (_freedom1 * (_freedom2 - 2.0) * (_freedom2 - 2.0) * (_freedom2 - 4.0));
			}
		}

		/// <summary>
		/// Получает среднее квадратичное отклонение.
		/// </summary>
		public double StdDev => Math.Sqrt(Variance);

		/// <summary>
		/// Получает энтропию распределения.
		/// </summary>
		public double Entropy
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>
		/// Получает асимметрию распределения.
		/// </summary>
		public double Skewness
		{
			get
			{
				if (_freedom2 <= 6.0)
				{
					throw new NotSupportedException();
				}

				return (2.0 * _freedom1 + _freedom2 - 2.0) * Math.Sqrt(8.0 * (_freedom2 - 4.0)) / ((_freedom2 - 6.0) * Math.Sqrt(_freedom1 * (_freedom1 + _freedom2 - 2.0)));
			}
		}

		/// <summary>
		/// Получает медиану распределения.
		/// </summary>
		public double Median
		{
			get
			{
				throw new NotSupportedException();
			}
		}

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
		/// Инициализирует новый экземпляр класса FisherSnedecor.
		/// </summary>
		/// <param name="d1">Первая степень свободы (d1) распределения. Диапозон: d1 > 0.</param>
		/// <param name="d2">Вторая степень свободы (d2) распределения. Диапозон: d2 > 0.</param>
		/// <exception cref="ArgumentException">Если d1 или d2 ≤ 0.</exception>
		public FisherSnedecor(double d1, double d2)
		{
			if (!IsValidParameterSet(d1, d2))
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			_random = new Random();
			_freedom1 = d1;
			_freedom2 = d2;
		}

		/// <summary>
		/// Инициализирует новый экземпляр класса FisherSnedecor.
		/// </summary>
		/// <param name="d1">Первая степень свободы (d1) распределения. Диапозон: d1 > 0.</param>
		/// <param name="d2">Вторая степень свободы (d2) распределения. Диапозон: d2 > 0.</param>
		/// <param name="randomSource">Генератор случайных чисел.</param>
		/// <exception cref="ArgumentException">Если scale и freedom не положительны.</exception>
		public FisherSnedecor(double d1, double d2, System.Random randomSource)
		{
			if (!IsValidParameterSet(d1, d2))
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			_random = randomSource ?? new Random();
			_freedom1 = d1;
			_freedom2 = d2;
		}

		/// <summary>
		/// Создаёт строковое представление выборки.
		/// </summary>
		/// <returns>строковое представление выборки.</returns>
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 2);
			defaultInterpolatedStringHandler.AppendLiteral("FisherSnedecor(d1 = ");
			defaultInterpolatedStringHandler.AppendFormatted(_freedom1);
			defaultInterpolatedStringHandler.AppendLiteral(", d2 = ");
			defaultInterpolatedStringHandler.AppendFormatted(_freedom2);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		/// <summary>
		/// Проверяет, являются ли предоставленные значения допустимыми параметрами этого распределения.
		/// </summary>
		/// <param name="d1">Первая степень свободы (d1) распределения. Диапозон: d1 > 0.</param>
		/// <param name="d2">Вторая степень свободы (d2) распределения. Диапозон: d2 > 0.</param>
		/// <returns>true - допустимые, false - не допустимые.</returns>
		public static bool IsValidParameterSet(double d1, double d2)
		{
			if (d1 > 0.0)
			{
				return d2 > 0.0;
			}

			return false;
		}

		/// <summary>
		/// Вычисляет плотность выборки (PDF) в точке x, т.е. ∂P(X ≤ x)/∂x.
		/// </summary>
		/// <param name="x">Оценочная точка.</param>
		/// <returns>плотность выборки (PDF) в точке x.</returns>
		public double Density(double x)
		{
			return PDF(_freedom1, _freedom2, x);
		}

		/// <summary>
		/// Вычисляет значение функции распределения (CDF) в точке x, т.е. P(X ≤ x).
		/// </summary>
		/// <param name="x">Оценочная точка.</param>
		/// <returns>распределение (CDF) в точке x.</returns>
		public double CumulativeDistribution(double x)
		{
			return CDF(_freedom1, _freedom2, x);
		}

		/// <summary>
		/// Вычисляет значение функции плотности вероятности (PDF) в точке x, т.е. ∂P(X ≤ x)/∂x.
		/// </summary>
		/// <param name="d1">Первая степень свободы (d1) распределения. Диапозон: d1 > 0.</param>
		/// <param name="d2">Вторая степень свободы (d2) распределения. Диапозон: d2 > 0.</param>
		/// <param name="x">Оценочная точка.</param>
		/// <returns>значение функции плотности вероятности (PDF) в точке x.</returns>
		/// <exception cref="ArgumentException">Если scale и freedom не положительны.</exception>
		public static double PDF(double d1, double d2, double x)
		{
			if (d1 <= 0.0 || d2 <= 0.0)
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			return Math.Sqrt(Math.Pow(d1 * x, d1) * Math.Pow(d2, d2) / Math.Pow(d1 * x + d2, d1 + d2)) / (x * AdvancedMath.Gamma(d1 / 2.0) * AdvancedMath.Gamma(d2 / 2.0) / AdvancedMath.Gamma((d1 + d2) / 2.0));

			//return Math.Sqrt(Math.Pow(d1 * x, d1) * Math.Pow(d2, d2) / Math.Pow(d1 * x + d2, d1 + d2)) / (x * AdvancedMath.Beta(d1 / 2.0, d2 / 2.0));
		}

		/// <summary>
		/// Вычисляет значение функции распределения (CDF) в точке x, т.е. P(X ≤ x).
		/// </summary>
		/// <param name="d1">Первая степень свободы (d1) распределения. Диапозон: d1 > 0.</param>
		/// <param name="d2">Вторая степень свободы (d2) распределения. Диапозон: d2 > 0.</param>
		/// <param name="x">Оценочная точка.</param>
		/// <returns>распределение (CDF) в точке x.</returns>
		/// <exception cref="ArgumentException">Если scale и freedom не положительны.</exception>
		public static double CDF(double d1, double d2, double x)
		{
			if (d1 <= 0.0 || d2 <= 0.0)
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			return SpecialFunctions.BetaIncomplete(d1 / 2.0, d2 / 2.0, d1 * x / (d1 * x + d2)) / (AdvancedMath.Gamma(d1 / 2.0) * AdvancedMath.Gamma(d2 / 2.0) / AdvancedMath.Gamma(d1 / 2.0 + d2 / 2.0));

			// !!! return AdvancedMath.Gamma(d1 / 2.0, d1 * x / (d1 * x + d2)) * AdvancedMath.Gamma(d2 / 2.0, d1 * x / (d1 * x + d2)) / AdvancedMath.Gamma((d1 + d2) / 2.0, d1 * x / (d1 * x + d2)) / (AdvancedMath.Gamma(d1 / 2.0) * AdvancedMath.Gamma(d2 / 2.0) / AdvancedMath.Gamma(d1 / 2.0 + d2 / 2.0));

			//return AdvancedMath.Beta(d1 / 2.0, d2 / 2.0, d1 * x / (d1 * x + d2)) / (AdvancedMath.Gamma(d1 / 2.0) * AdvancedMath.Gamma(d2 / 2.0) / AdvancedMath.Gamma(d1 / 2.0 + d2 / 2.0));

			//return AdvancedMath.Beta(d1 / 2.0, d2 / 2.0, d1 * x / (d1 * x + d2)) / AdvancedMath.Beta(d1 / 2.0, d2 / 2.0);

			//return AdvancedMath.LeftRegularizedBeta(d1 / 2.0, d2 / 2.0, d1 * x / (d1 * x + d2));
		}
	}
}
