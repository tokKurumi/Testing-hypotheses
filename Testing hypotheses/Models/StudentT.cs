using MathNet.Numerics.RootFinding;
using Meta.Numerics.Functions;
using Modeling_sample.Distributions.Interfaces;
using System.Runtime.CompilerServices;

namespace Univariate_distributions.Models
{
	public class StudentT : IContinuousDistribution, IUnivariateDistribution, IDistribution
	{
		private System.Random _random;

		private readonly double _location;

		private readonly double _scale;

		private readonly double _freedom;

		/// <summary>
		/// Получает область (μ) для т-распределения Стьюдента.
		/// </summary>
		public double Location => _location;

		/// <summary>
		/// Получает масштаб (σ) для т-распределения Стьюдента. Диапозон: σ > 0.
		/// </summary>
		public double Scale => _scale;

		/// <summary>
		/// Получает степень свободы (v) для т-распределения Стьюдента. Диапозон: ν > 0.
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
		public double Mean
		{
			get
			{
				if (!(_freedom > 1.0))
				{
					return double.NaN;
				}

				return _location;
			}
		}

		/// <summary>
		/// Получает дисперсию распределения.
		/// </summary>
		public double Variance
		{
			get
			{
				if (double.IsPositiveInfinity(_freedom))
				{
					return _scale * _scale;
				}

				if (_freedom > 2.0)
				{
					return _freedom * _scale * _scale / (_freedom - 2.0);
				}

				if (!(_freedom > 1.0))
				{
					return double.NaN;
				}

				return double.PositiveInfinity;
			}
		}

		/// <summary>
		/// Получает среднее квадратичное отклонение.
		/// </summary>
		public double StdDev
		{
			get
			{
				if (double.IsPositiveInfinity(_freedom))
				{
					return Math.Sqrt(_scale * _scale);
				}

				if (_freedom > 2.0)
				{
					return Math.Sqrt(_freedom * _scale * _scale / (_freedom - 2.0));
				}

				if (!(_freedom > 1.0))
				{
					return double.NaN;
				}

				return double.PositiveInfinity;
			}
		}

		/// <summary>
		/// Получает энтропию распределения.
		/// </summary>
		public double Entropy
		{
			get
			{
				if (_location != 0.0 || _scale != 1.0)
				{
					throw new NotSupportedException();
				}

				return (_freedom + 1.0) / 2.0 * (AdvancedMath.Psi((1.0 + _freedom) / 2.0) - AdvancedMath.Psi(_freedom / 2.0)) + Math.Log(Math.Sqrt(_freedom) * AdvancedMath.Beta(_freedom / 2.0, 0.5));
			}
		}

		/// <summary>
		/// Получает асимметрию распределения.
		/// </summary>
		public double Skewness
		{
			get
			{
				if (_freedom <= 3.0)
				{
					throw new NotSupportedException();
				}

				return 0.0;
			}
		}

		/// <summary>
		/// Получает медиану распределения.
		/// </summary>
		public double Median => _location;

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
		/// Инициализирует новый экземпляр класса StudentT.
		/// location = 0.0, scale = 1.0, degrees = 1, freedom = 1.
		/// </summary>
		public StudentT()
		{
			_random = new Random();
			_location = 0.0;
			_scale = 1.0;
			_freedom = 1.0;
		}

		/// <summary>
		/// Вычисляет значение обратной функции распределения (CDF) в точке x, т.е. P(X ≤ x).
		/// </summary>
		/// <param name="location">Область (μ) для т-распределения Стьюдента.</param>
		/// <param name="scale">Масштаб (σ) для т-распределения Стьюдента. Диапозон: σ > 0.</param>
		/// <param name="freedom">Степень свободы (v) для т-распределения Стьюдента. Диапозон: ν > 0.</param>
		/// <param name="p">Вероятность (p) в допустимом диапозоне. Диапозон: 0 ≤ p ≤ 1.</param>
		/// <returns>значение обратной функции распределения (CDF) в точке x</returns>
		/// <exception cref="ArgumentException">Если scale и freedom не положительны.</exception>
		/// <remarks>WARNING: Не явная реализация, поэтому медленная и ненадежная.</remarks>
		public static double InvCDF(double location, double scale, double freedom, double p)
		{
			return MathNet.Numerics.Distributions.StudentT.InvCDF(location, scale, freedom, p);
		}

		/// <summary>
		/// Вычисляет значение обратной функции распределения (CDF) в точке x, т.е. P(X ≤ x).
		/// </summary>
		/// <param name="p">Вероятность (p) в допустимом диапозоне. Диапозон: 0 ≤ p ≤ 1.</param>
		/// <returns>значение обратной функции распределения (CDF) в точке x</returns>
		/// <exception cref="ArgumentException">Если scale и freedom не положительны.</exception>
		/// <remarks>WARNING: Не явная реализация, поэтому медленная и ненадежная.</remarks>
		public double InverseCumulativeDistribution(double p)
		{
			return InvCDF(_location, _scale, _freedom, p);
		}

		/// <summary>
		/// Инициализирует новый экземпляр класса StudentT.
		/// </summary>
		/// <param name="location">Область (μ) для т-распределения Стьюдента.</param>
		/// <param name="scale">Масштаб (σ) для т-распределения Стьюдента. Диапозон: σ > 0.</param>
		/// <param name="freedom">Степень свободы (v) для т-распределения Стьюдента. Диапозон: ν > 0.</param>
		/// <exception cref="ArgumentException">Если scale и freedom не положительны.</exception>
		public StudentT(double location, double scale, double freedom)
		{
			if (!IsValidParameterSet(location, scale, freedom))
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			_random = new Random();
			_location = location;
			_scale = scale;
			_freedom = freedom;
		}

		/// <summary>
		/// Инициализирует новый экземпляр класса StudentT.
		/// </summary>
		/// <param name="location">Область (μ) для т-распределения Стьюдента.</param>
		/// <param name="scale">Масштаб (σ) для т-распределения Стьюдента. Диапозон: σ > 0.</param>
		/// <param name="freedom">Степень свободы (v) для т-распределения Стьюдента. Диапозон: ν > 0.</param>
		/// <param name="randomSource">Генератор случайных чисел.</param>
		/// <exception cref="ArgumentException">Если scale и freedom не положительны.</exception>
		public StudentT(double location, double scale, double freedom, System.Random randomSource)
		{
			if (!IsValidParameterSet(location, scale, freedom))
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			_random = randomSource ?? new Random();
			_location = location;
			_scale = scale;
			_freedom = freedom;
		}

		/// <summary>
		/// Создаёт строковое представление выборки.
		/// </summary>
		/// <returns>строковое представление выборки.</returns>
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 3);
			defaultInterpolatedStringHandler.AppendLiteral("StudentT(Mu = ");
			defaultInterpolatedStringHandler.AppendFormatted(_location);
			defaultInterpolatedStringHandler.AppendLiteral(", Sigma = ");
			defaultInterpolatedStringHandler.AppendFormatted(_scale);
			defaultInterpolatedStringHandler.AppendLiteral(", Vi = ");
			defaultInterpolatedStringHandler.AppendFormatted(_freedom);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		/// <summary>
		/// Проверяет, являются ли предоставленные значения допустимыми параметрами этого распределения.
		/// </summary>
		/// <param name="location">Область (μ) для т-распределения Стьюдента.</param>
		/// <param name="scale">Масштаб (σ) для т-распределения Стьюдента. Диапозон: σ > 0.</param>
		/// <param name="freedom">Cтепень свободы (k) для Хи-квадрат распределения. Диапозон: k > 0.</param>
		/// <returns>true - допустимые, false - не допустимые.</returns>
		public static bool IsValidParameterSet(double location, double scale, double freedom)
		{
			if (scale > 0.0 && freedom > 0.0)
			{
				return !double.IsNaN(location);
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
			return PDF(_location, _scale, _freedom, x);
		}

		/// <summary>
		/// Вычисляет значение функции распределения (CDF) в точке x, т.е. P(X ≤ x).
		/// </summary>
		/// <param name="x">Оценочная точка.</param>
		/// <returns>распределение (CDF) в точке x.</returns>
		public double CumulativeDistribution(double x)
		{
			return CDF(_location, _scale, _freedom, x);
		}

		/// <summary>
		/// Вычисляет значение функции плотности вероятности (PDF) в точке x, т.е. ∂P(X ≤ x)/∂x.
		/// </summary>
		/// <param name="location">Область (μ) для т-распределения Стьюдента.</param>
		/// <param name="scale">Масштаб (σ) для т-распределения Стьюдента. Диапозон: σ > 0.</param>
		/// <param name="freedom">Cтепень свободы (k) для Хи-квадрат распределения. Диапозон: k > 0.</param>
		/// <param name="x">Оценочная точка.</param>
		/// <returns>значение функции плотности вероятности (PDF) в точке x.</returns>
		/// <exception cref="ArgumentException">Если степень свободы (k) ≤ 0.</exception>
		public static double PDF(double location, double scale, double freedom, double x)
		{
			if (scale <= 0.0 || freedom <= 0.0)
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			if (freedom >= 100000000.0)
			{
				return Normal.PDF(location, scale, x);
			}

			double num = (x - location) / scale;
			return Math.Exp(Math.Log(AdvancedMath.Gamma((freedom + 1.0) / 2.0)) - Math.Log(AdvancedMath.Gamma(freedom / 2.0))) * Math.Pow(1.0 + num * num / freedom, -0.5 * (freedom + 1.0)) / Math.Sqrt(freedom * Math.PI) / scale;
		}

		/// <summary>
		/// Вычисляет значение функции распределения (CDF) в точке x, т.е. P(X ≤ x).
		/// </summary>
		/// <param name="location">Область (μ) для т-распределения Стьюдента.</param>
		/// <param name="scale">Масштаб (σ) для т-распределения Стьюдента. Диапозон: σ > 0.</param>
		/// <param name="freedom">Cтепень свободы (k) для Хи-квадрат распределения. Диапозон: k > 0.</param>
		/// <param name="x">Оценочная точка.</param>
		/// <returns>распределение (CDF) в точке x.</returns>
		/// <exception cref="ArgumentException">Если степень свободы (k) ≤ 0.</exception>
		public static double CDF(double location, double scale, double freedom, double x)
		{
			if (scale <= 0.0 || freedom <= 0.0)
			{
				throw new ArgumentException("Invalid parametrization for the distribution.");
			}

			if (double.IsPositiveInfinity(freedom))
			{
				return Normal.CDF(location, scale, x);
			}

			//return AdvancedMath.Gamma((freedom + 1.0) / 2.0) / AdvancedMath.Gamma(freedom / 2.0) / Math.Sqrt(freedom * Math.PI) / Math.Pow(1.0 + x * x / freedom, (freedom + 1.0) / 2.0);
			//return 0.5 + x * AdvancedMath.Gamma((freedom + 1.0) / 2.0) * 2 * AdvancedMath.Hypergeometric2F1(0.5, (freedom + 1.0) / 2.0, 1.5, (-1) * x * x / freedom) / Math.Sqrt(Math.PI * freedom) / AdvancedMath.Gamma(freedom / 2.0);

			double num = (x - location) / scale;
			double x2 = freedom / (freedom + num * num);
			double num2 = 0.5 * AdvancedMath.LeftRegularizedBeta(freedom / 2.0, 0.5, x2);
			if (!(x <= location))
			{
				return 1.0 - num2;
			}

			return num2;
		}
	}
}
