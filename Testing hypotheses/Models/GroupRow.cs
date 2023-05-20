namespace Testing_hypotheses.Models
{
	public struct GroupRow
	{
		public double BottomLine;
		public double TopLine;
		public int Frequency;
		public int AccumulatedFrequency;
		public double RelativeFrequency;
		public double RelativeCumulativeFrequency;

		public GroupRow(double bottomLine, double topLine, int frequency, int accumulatedFrequency, double relativeFrequency, double relativeCumulativeFrequency)
		{
			BottomLine = bottomLine;
			TopLine = topLine;
			Frequency = frequency;
			AccumulatedFrequency = accumulatedFrequency;
			RelativeFrequency = relativeFrequency;
			RelativeCumulativeFrequency = relativeCumulativeFrequency;
		}
	}
}
