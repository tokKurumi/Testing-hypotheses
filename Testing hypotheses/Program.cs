using Univariate_distributions.Models;
using static Univariate_distributions.Models.SpecialFunctions;

public class Program
{
	public static void Main()
	{
		//Значения
		var chiSquareValues = new double[]
		{
			4.9506, 6.1203, 1.4620, 6.3150, 3.6614, 4.8902, 11.7663, 8.2703, 1.7215, 2.1105, 21.9353, 2.6600, 4.5397, 2.5497, 4.4388, 3.0415, 4.5772,
			5.1293, 1.1323, 9.6146, 6.3091, 3.8107, 3.9854, 2.6705, 3.9978, 5.5348, 0.8111, 5.1975, 6.9759, 1.4899, 8.4501, 3.8086, 9.2413, 3.1875,
			2.1619, 1.8470, 3.9828, 2.7426, 2.9056, 6.8836, 8.1504, 3.4887, 5.1483, 2.3263, 5.6205, 4.4762, 5.3977, 3.2068, 3.3470, 7.8549
		}; // matlab chisquare
		var binomialValues = Binomial.Samples(0.5, 30, 50).Select(item => (double)item);
		var exponentionalValues = Exponential.Samples(0.5, 50);
		var normalValues = Normal.Samples(10, 1, 50);

		//Задание 1
		Console.WriteLine(HarkeBer(normalValues, 0.95));

		//Задание 2
		var freedom = GroupValues(normalValues).Count - 2;
		//var distribution = new ChiSquared(5);
		//var distribution = new Binomial(0.5, 30);
		//var distribution = new Exponential(0.5);
		var distribution = new Normal(10, 1);
		Console.WriteLine($"{ChiSquaredTest(distribution, normalValues)} и {ChiSquared.InvCDF(freedom, 0.95)}");
	}
}