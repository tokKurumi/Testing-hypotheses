using MathNet.Numerics.RootFinding;
using Modeling_sample.Distributions.Interfaces;
using Testing_hypotheses.Models;

namespace Univariate_distributions.Models
{
	public static class SpecialFunctions
	{
		/// <summary>
		/// Вычисляет количество сочетаний без повторений C(N, K).
		/// </summary>
		/// <param name="N">Общее количество элеметов.</param>
		/// <param name="K">Размер группы.</param>
		/// <returns>количество сочетаний без повторений C(N, K).</returns>
		/// <remarks>http://blog.plover.com/math/choose.html</remarks>
		public static long GetBinCoeff(long N, long K)
		{
			long r = 1;
			long d;
			if (K > N) return 0;
			for (d = 1; d <= K; d++)
			{
				r *= N--;
				r /= d;
			}
			return r;
		}

		/// <summary>
		/// Вычисляет факториал числа N.
		/// </summary>
		/// <param name="N">Фактор-число в диапозоне N >= 0.</param>
		/// <returns>факториал числа N.</returns>
		public static long Factorial(long N)
		{
			if (N == 0)
			{
				return 1;
			}

			long result = 1;
			for (long i = 1; i <= N; ++i)
			{
				result *= i;
			}

			return result;
		}

		/// <summary>
		/// Вычисляет значение функции Лапасса в точке a с заданной точностью p.
		/// </summary>
		/// <param name="a">Точка для оценки.</param>
		/// <param name="precision">Задаваемая точность.</param>
		/// <returns></returns>
		public static double Laplace(double a, double precision)
		{
			double result = 0;
			for (double i = 0; i < a; i += precision)
			{
				result += precision * Math.Abs(Math.Exp(-.5 * Math.Pow(i, 2)) + Math.Exp(-.5 * Math.Pow((i + precision), 2))) / 2.0;
			}
			result *= 1.0 / Math.Pow(2 * Math.PI, .5);
			return result;
		}

		/// <summary>
		/// Вычисляет значение функции ошибок в точке x.
		/// </summary>
		/// <param name="x">Оцениваемая точка.</param>
		/// <returns>значение функции ошибок в точке x.</returns>
		public static double Erf(double x)
		{
			// constants
			double a1 = 0.254829592;
			double a2 = -0.284496736;
			double a3 = 1.421413741;
			double a4 = -1.453152027;
			double a5 = 1.061405429;
			double p = 0.3275911;

			// Save the sign of x
			int sign = 1;
			if (x < 0)
				sign = -1;
			x = Math.Abs(x);

			// A&S formula 7.1.26
			double t = 1.0 / (1.0 + p * x);
			double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

			return sign * y;
		}

		private static double Add(ref double location1, double value)
		{
			double newCurrentValue = location1; // non-volatile read, so may be stale
			while (true)
			{
				double currentValue = newCurrentValue;
				double newValue = currentValue + value;
				newCurrentValue = Interlocked.CompareExchange(ref location1, newValue, currentValue);
				if (newCurrentValue.Equals(currentValue)) // see "Update" below
					return newValue;
			}
		}

		/// <summary>
		/// Вычисляет интеграл функции на заданном отрезке.
		/// </summary>
		/// <param name="func">Функция интегрирования.</param>
		/// <param name="leftPoint">Начало отрезка интегрирования.</param>
		/// <param name="rightPoint">Конец отрезка интегрирования.</param>
		/// <param name="step">Шаг интегрирования.</param>
		/// <returns>Значение интеграла на заданном интервале.</returns>
		public static double Trapezoid(this Func<double, double> func, double leftPoint, double rightPoint, double step = 0.0001)
		{
			if (rightPoint - leftPoint < step)
			{
				return 0;
			}

			double sum = 0.0;

			Parallel.For(1, (int)((rightPoint - leftPoint) / step) - 1, () => 0.0, (i, state, local) =>
			{
				local += func(leftPoint + i * step);
				return local;
			}, local => Add(ref sum, local));

			return (sum + 0.5 * (func(leftPoint) + func(rightPoint))) * step;
		}

		/// <summary>
		/// Вычисляет Бета-фнкцию до точки x.
		/// </summary>
		/// <param name="a">Начальная точка.</param>
		/// <param name="b">Конечная точка.</param>
		/// <returns>значение Бета-функции.</returns>
		/// <remarks>
		/// https://en.wikipedia.org/wiki/Beta_function
		/// </remarks>
		public static double Beta(double a, double b)
		{
			return BetaIncomplete(a, b, 1);
		}

		/// <summary>
		/// Вычисляет Незавершённую Бета-фнкцию до точки x.
		/// </summary>
		/// <param name="a">Начальная точка.</param>
		/// <param name="b">Конечная точка.</param>
		/// <param name="x">Конечная точка интегрирования.</param>
		/// <returns>значение незавершённой Бета-функции до точки x.</returns>
		/// <remarks>
		/// https://en.wikipedia.org/wiki/Beta_function#Incomplete_beta_function
		/// </remarks>
		public static double BetaIncomplete(double a, double b, double x)
		{
			return Trapezoid(t => Math.Pow(t, a - 1) * Math.Pow(1 - t, b - 1), 0, x);
		}

		/// <summary>
		/// Вычисляет регуляризованную незаверщённую Бета-функцию до точки x.
		/// </summary>
		/// <param name="a">Начальная точка.</param>
		/// <param name="b">Конечная точка.</param>
		/// <param name="x">Конечная точка интегрирования.</param>
		/// <returns>значение регуляризованной незавершённой Бета-функции до точки x.</returns>
		public static double BetaRegularized(double a, double b, double x)
		{
			return BetaIncomplete(a, b, x) / Beta(a, b);
		}

		/// <summary>
		/// Проверяет произашло ли событие по Бернули при заданной вероятности p.
		/// </summary>
		/// <param name="p">Заданная вероятность проверки.</param>
		/// <returns>произайдёт ли событие.</returns>
		public static bool Bernoulli(double p)
		{
			Random rnd = new Random();
			return rnd.NextDouble() < p;
		}

		/// <summary>
		/// Заполяет выборку по закону Бернули при заданной вероятности p.
		/// </summary>
		/// <param name="p">Заданная вероятность проверки.</param>
		public static void Bernoulli(double p, bool[] array)
		{
			var rnd = new Random();
			Parallel.For(0, array.Length, i =>
			{
				lock (rnd)
				{
					array[i] = rnd.NextDouble() < p;
				}
			});
		}

		/// <summary>
		/// Вычисляет обратную функцию ошибок в z.
		/// </summary>
		/// <param name="z">Значение для оценки.</param>
		/// <returns>Обратное значение функции ошибок в точке z.</returns>
		/// <remarks>https://numerics.mathdotnet.com/</remarks>
		public static double ErfcInv(double z)
		{
			return MathNet.Numerics.SpecialFunctions.ErfcInv(z);
		}

		/// <summary>
		/// Генерирует выборку по закону Бернули при заданной вероятности p.
		/// </summary>
		/// <param name="p">Заданная вероятность проверки.</param>
		/// <param name="count">Количество элементов генрации.</param>
		/// <returns>выборка закона Бернули.</returns>
		public static bool[] Bernoulli(double p, int count)
		{
			bool[] result = new bool[count];
			Bernoulli(p, result);

			return result;
		}

		public static (bool result, double JBSTAT, double kv) HarkeBer(IEnumerable<double> values, double alpha)
		{
			var averenge = values.Average();
			var s = Math.Sqrt(values.Sum(elem => (elem - averenge) * (elem - averenge)) / values.Count());
			var Sk = values.Sum(elem => (elem - averenge) * (elem - averenge) * (elem - averenge)) / values.Count() / (s * s * s);
			var Kur = values.Sum(elem => (elem - averenge) * (elem - averenge) * (elem - averenge) * (elem - averenge)) / values.Count() / (s * s * s * s);
			var JBSTAT = values.Count() * ((Sk * Sk / 6) + (Kur - 3) * (Kur - 3) / 24);
			var kv = ChiSquared.InvCDF(2, alpha);

			return (JBSTAT < kv, JBSTAT, kv);
		}

		public enum Distribution
		{
			Binomial,
			Exponentional,
			Normal
		}

		public static List<GroupRow> GroupValues(IEnumerable<double> observed)
		{
			var min = observed.Min();
			var max = observed.Max();
			var stepsCount = Convert.ToInt16(1 + 3.32 * Math.Log10(observed.Count()));
			var step = (max - min) / stepsCount;

			var result = new List<GroupRow>();
			for (int i = 0; i < stepsCount; i++)
			{
				var BottomLine = min + step * i;
				var TopLine = BottomLine + step;
				var Frequency = observed.Count(x => (x >= min + step * i) && (x < min + step * (i + 1)));
				var AccumulatedFrequency = result.Select(x => x.Frequency).Sum() + Frequency;
				var RelativeFrequency = (double)Frequency / observed.Count();
				var RelativeCumulativeFrequency = result.Select(x => x.RelativeFrequency).Sum() + RelativeFrequency;

				result.Add(new GroupRow(
					BottomLine,
					TopLine,
					Frequency,
					AccumulatedFrequency,
					RelativeFrequency,
					RelativeCumulativeFrequency)
				);
			}

			return result;
		}

		public static double ChiSquaredTest(IUnivariateDistribution distribution, IEnumerable<double> observed)
		{
			var gropedList = GroupValues(observed);

			var theoretical_frequencies = new List<double>();
			for (int i = 0; i < gropedList.Count; i++)
			{
				theoretical_frequencies.Add(distribution switch
				{
					IDiscreteDistribution => observed.Count() * (distribution.CumulativeDistribution(gropedList[i].TopLine) - distribution.CumulativeDistribution(gropedList[i].BottomLine)),
					IContinuousDistribution => observed.Count() * (distribution.CumulativeDistribution(gropedList[i].TopLine) - distribution.CumulativeDistribution(gropedList[i].BottomLine)),
					_ => throw new ArgumentOutOfRangeException(nameof(distribution), $"Not expected direction value: {distribution}"),
				});
			}

			return gropedList.Zip(theoretical_frequencies)
				.Sum(zipedElem => (zipedElem.First.Frequency - zipedElem.Second) * (zipedElem.First.Frequency - zipedElem.Second) / zipedElem.Second);
		}

		//public static double ChiSquaredTest(IEnumerable<double> observed, Distribution distribution)
		//{
		//	var min = observed.Min();
		//	var max = observed.Max();
		//	var stepsCount = Convert.ToInt16(1 + 3.32 * Math.Log10(observed.Count()));
		//	var step = (max - min) / stepsCount;

		//	var w = max - min;
		//	var b = w / stepsCount;

		//	var groupRows = new List<GroupRow>();
		//	var theoretical_frequencies = new List<double>();
		//	for (int i = 0; i < stepsCount; i++)
		//	{
		//		var BottomLine = min + step * i;
		//		var TopLine = BottomLine + step;
		//		var Frequency = observed.Count(x => (x >= min + step * i) && (x < min + step * (i + 1)));
		//		var AccumulatedFrequency = groupRows.Select(x => x.Frequency).Sum() + Frequency;
		//		var RelativeFrequency = (double)Frequency / observed.Count();
		//		var RelativeCumulativeFrequency = groupRows.Select(x => x.RelativeFrequency).Sum() + RelativeFrequency;

		//		groupRows.Add(new GroupRow(
		//			BottomLine,
		//			TopLine,
		//			Frequency,
		//			AccumulatedFrequency,
		//			RelativeFrequency,
		//			RelativeCumulativeFrequency)
		//		);


		//		switch (distribution)
		//		{
		//			case Distribution.Binomial:
		//				{
		//					var temp1 = Binomial.CDF(Program.P_FOR_BINO, )
		//					break;
		//				}

		//			case Distribution.Exponentional:
		//				{
		//					var a_zv = (observed.Count() - 1) / observed.Sum();
		//					var temp1 = Exponential.CDF(a_zv, BottomLine);
		//					var temp2 = Exponential.CDF(a_zv, TopLine);

		//					theoretical_frequencies.Add(observed.Count() * (temp2 - temp1));
		//					break;
		//				}

		//			case Distribution.Normal:
		//				{

		//					break;
		//				}
		//		}

		//	}
		//	//dynamic cdf = distribution switch
		//	//{
		//	//	Distribution.Binomial => Binomial.CDF,
		//	//	Distribution.Exponentional => Exponential.CDF,
		//	//	Distribution.Normal => Normal.CDF
		//	//};



		//	//var expected = ChiSquared.Samples()
		//	//return observed.Zip(expected).Sum(elem => (elem.First - elem.Second) * (elem.First - elem.Second) / elem.Second);
		//}
	}
}
