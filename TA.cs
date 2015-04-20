
using System;
using System.IO;
using System.Text;


/// <summary>
/// Recognition of basic TA patterns
/// </summary>
namespace TA
{
	public class Point
	{
		public Point(double price, double volume)
		{
			this.price = price;
			this.volume = volume;
		}

		public double price
		{
			get;
			protected set;
		}

		public double volume
		{
			get;
			protected set;
		}
	}

	public class Candle
	{
		public Candle(double open, double close, double high, double low, int volume)
		{
			this.open = open;
			this.close = close;
			this.high = high;
			this.low = low;
			this.volume = volume;
		}

		public double open
		{
			get;
			protected set;
		}

		public double close
		{
			get;
			protected set;
		}

		public double low
		{
			get;
			protected set;
		}

		public double high
		{
			get;
			protected set;
		}

		public int volume
		{
			get;
			protected set;
		}

		public override string ToString()
		{
			String s;
			s = JQuant.OutputUtils.FormatField(open, 8) +
				 JQuant.OutputUtils.FormatField(high, 8) +
				 JQuant.OutputUtils.FormatField(low, 8) +
				 JQuant.OutputUtils.FormatField(close, 8) +
				 JQuant.OutputUtils.FormatField(volume, 10);

			return s;
		}

	}

	public class CandleDaily : Candle
	{
		public CandleDaily(System.DateTime date, double open, double close, double high, double low, int volume)
			: base(open, close, high, low, volume)
		{
			Date = date;
		}

		public System.DateTime Date
		{
			get;
			protected set;
		}
	}


	/// <summary>
	/// stores price+volume series in list
	/// I need quick iterration through the list and quick adding to the list
	/// </summary>
	public class PriceVolumeSeries
	{
		public PriceVolumeSeries(int size)
		{
			Data = new System.Collections.ArrayList(size);
			paramsCalculated = false;
		}

		public void Add(double open, double close, double min, double max, int volume)
		{
			Add(new Candle(open, close, min, max, volume));
		}

		protected void Add(Candle candle)
		{
			paramsCalculated = false;
			Data.Add(candle);
		}

		public System.Collections.ArrayList Data
		{
			get;
			protected set;
		}

		public static void CalculateAverageStdDeviation
			(PriceVolumeSeries series, out double average, out double max, out double min, out double stdDeviation)
		{
			CalculateAverageStdDeviation(series, 0, series.Data.Count, out average, out max, out min, out stdDeviation);
		}

		public enum Format
		{
			ASCII,
			CSV,
			XML,
			Table
		}

		public override string ToString()
		{
			return ToString(Format.Table);
		}

		public string ToString(Format format)
		{
			string result = null;

			// preallocate some memory
			System.Text.StringBuilder sb = new System.Text.StringBuilder(Data.Count * 80);
			switch (format)
			{
				case Format.Table:
					foreach (Candle candle in Data)
					{
						sb.Append(JQuant.OutputUtils.FormatField(candle.open, 8) +
								  JQuant.OutputUtils.FormatField(candle.high, 8) +
								  JQuant.OutputUtils.FormatField(candle.low, 8) +
								  JQuant.OutputUtils.FormatField(candle.close, 8) +
								  JQuant.OutputUtils.FormatField(candle.volume, 10) +
								  "\n");
					}
					break;
				case Format.CSV:
					foreach (Candle candle in Data)
					{
						sb.Append("" + candle.open + "," + candle.high + "," + candle.low + "," + candle.close + "," + candle.volume + "\n");
					}
					break;
				case Format.XML:
				default:
					foreach (Candle candle in Data)
					{
						sb.Append("<Candle o=" + candle.open + ",h=" + candle.high +
								  ",l=" + candle.low + ",c=" + candle.close +
								  ",v=" + candle.volume + "></Candle>\n");
					}
					break;
			}

			result = sb.ToString();
			return result;
		}

		/// <summary>
		/// the method does not calculate series std deviation, but a measurement of the single 
		/// candle variance or a single candle volatility
		/// Welford's Algorithm for single pass calculation
		///  long n = 0;
		///   double mu = 0.0;
		///   double sq = 0.0;
		///  
		///   void update(double x) {
		///       ++n;
		///       double muNew = mu + (x - mu)/n;
		///       sq += (x - mu) * (x - muNew)
		///       mu = muNew;
		///   }
		///   double mean() { return mu; }
		///   double var() { return n > 1 ? sq/n : 0.0; }
		/// More can be found here http://en.wikipedia.org/wiki/Algorithms_for_calculating_variance
		/// Example in Java for online calculaiton http://www.johndcook.com/standard_deviation.html
		/// </summary>
		public static void CalculateAverageStdDeviation
			(PriceVolumeSeries series, int start, int count, out double average, out double max, out double min, out double stdDeviation)
		{
			Candle candle;
			double close;
			average = 0;
			min = Int32.MaxValue;
			max = Int32.MinValue;
			stdDeviation = 0;

			double candleSize = 0;

			int end = start + count - 1;
			for (int i = start; i <= end; i++)
			{
				candle = (Candle)series.Data[i];
				close = candle.close;

				average += close;
				max = Math.Max(max, close);
				min = Math.Min(min, close);

				double d = Math.Abs(candle.open - close);
				// calculate "variance"
				candleSize += d;
			}

			average = average / count;
			double candleSizeAverage = candleSize / count;

			for (int i = start; i <= end; i++)
			{
				candle = (Candle)series.Data[i];
				close = candle.close;

				candleSize = Math.Abs(candle.open - close);
				double d = candleSize - candleSizeAverage;
				// calculate "candle variance"
				stdDeviation += d * d;
			}

			stdDeviation = Math.Sqrt(stdDeviation / (count - 1));
		}

		public static void CalculateAverage
			(PriceVolumeSeries series, int start, int count, out double average, out double max, out int maxIdx, out double min, out int minIdx)
		{
			Candle candle = (Candle)series.Data[start];
			double close = candle.close;
			average = close;
			max = close;
			maxIdx = 0;
			min = max;
			minIdx = 0;

			int end = start + count - 1;
			for (int i = start + 1; i <= end; i++)
			{
				candle = (Candle)series.Data[i];
				close = candle.close;
				if (max < close)
				{
					max = close;
					maxIdx = i;
				}
				if (min > close)
				{
					min = close;
					minIdx = i;
				}
				average += close;
			}

			average = average / count;
		}

		/// <summary>
		/// Initialize Max, Min, Average, StdDev properties
		/// This method will be called only once. 
		/// </summary>
		public void CalculateParams()
		{
			if (!paramsCalculated)
			{
				double average; double max; double min; double stdDeviation;

				CalculateAverageStdDeviation(this, out average, out max, out min, out stdDeviation);

				this.Average = average;
				this.Min = min;
				this.Max = max;
				this.StdDeviation = stdDeviation;

				this.paramsCalculated = true;
			}
		}

		/// <summary>
		/// Fisher transform 0.5*ln*[(1+x)/(1-x)] 
		/// </summary>
		public static void Fisher(double[] series)
		{
			int count = series.Length;
			for (int i = 0; i < count; i++)
			{
				double d = series[i];
				if (d < -0.9999) d = -0.9999;
				if (d > 0.9999) d = 0.9999;
				d = 0.5 * Math.Log((1 + d) / (1 - d), 2);
				series[i] = d;
			}
		}

		/// <summary>
		/// Two points EMA
		/// </summary>
		public static void EMA(double[] series, double weight)
		{
			int count = series.Length;
			count = count - 1;
			double weight_1 = 1 - weight;
			for (int i = 0; i < count; i++)
			{
				double data0 = series[i];
				double data1 = series[i + 1];
				double ema = data0 * weight + data1 * weight_1;
				series[i + 1] = ema;
			}
		}

		/// <summary>
		/// Method will normalize (bring to range from -1 to 1 the open, high, low, close prices
		/// Performance warning: created a new candle for every candle in the
		/// series
		/// </summary>
		/// <param name="series">
		/// A <see cref="PriceVolumeSeries"/>
		/// </param>
		/// <param name="start">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <param name="count">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// A <see cref="System.Double"/>
		/// <param name="max">

		/// </param>
		/// <param name="min">
		/// A <see cref="System.Double"/>
		/// </param>
		public static double[] Normalize
			(PriceVolumeSeries series, double max, double min)
		{
			int count = series.Data.Count;
			double[] result = new double[count];
			double max_min = max - min;

			for (int i = 0; i < count; i++)
			{
				Candle candle = (Candle)series.Data[i];
				double close = candle.close;
				close = 2 * ((close - min) / max_min - 0.5);
				result[i] = close;
			}

			return result;
		}

		public static double[] Normalize
			(PriceVolumeSeries series)
		{
			double max = series.Max;
			double min = series.Min;
			double[] result = Normalize(series, max, min);
			return result;
		}

		/// <summary>
		/// Normalize the data using rolling 'range' data points
		/// Results will be in the [-1;1]
		/// </summary>
		/// <param name="series">
		/// A <see cref="PriceVolumeSeries"/>
		/// </param>
		/// <param name="range">
		/// A <see cref="System.Int32"/>
		/// </param>
		public static double[] Normalize
			(double[] series, int range)
		{
			int count = series.Length;
			// no point to start to do anything
			if (count < range) return null;

			double[] result = new double[count - range + 1];

			int idx = (range - 1);
			int idxRes = 0;
			double min;
			double max;
			double first = series[0];

			CalculateMaxMin(series, idxRes, range, out max, out min);

			while (idx < count)
			{
				// I do not have to recalculate Max and Min in all cases, but only
				// when the entry i drop is min or max or ...
				if (first <= min) CalculateMaxMin(series, idxRes, range, out max, out min);
				else if (first >= max) CalculateMaxMin(series, idxRes, range, out max, out min);

				double d = series[idx];
				first = series[idx - range + 1];

				// .. or a new entry is a max or min
				max = Math.Max(max, d);
				min = Math.Min(min, d);

				if (max != min) d = 2.0 * ((d - min) / (max - min) - 0.5);
				else d = 0;

				// System.Console.WriteLine("max="+max+",min="+min+",d="+series[idx]+",dc="+d+",f="+first);
				result[idxRes] = d;

				// move to the next point
				idx++; idxRes++;
			}

			return result;
		}

		private static void CalculateMaxMin(double[] data, int start, int count, out double max, out double min)
		{
			min = Double.MaxValue;
			max = Double.MinValue;
			int end = start + count;
			for (int i = start; i < end; i++)
			{
				double d = data[i];
				max = Math.Max(d, max);
				min = Math.Min(d, min);
			}
		}

		public static void CalculateAverage
			(double[] series, int start, int count, out double average, out double max, out double min)
		{
			double close = series[0];
			average = close;
			max = close;
			min = max;

			int end = start + count;
			for (int i = start + 1; i < end; i++)
			{
				close = series[i];
				if (max < close)
				{
					max = close;
				}
				if (min > close)
				{
					min = close;
				}
				average += close;
			}

			average = average / count;
		}

		public static double[] GetClose(PriceVolumeSeries series)
		{
			int count = series.Data.Count;

			double[] result = new double[count];
			int idx = 0;
			foreach (Candle candle in series.Data)
			{
				result[idx] = candle.close;
				idx++;
			}
			return result;
		}


		public double Average
		{
			get
			{
				if (!paramsCalculated)
				{
					CalculateParams();
				}
				return average;
			}
			protected set
			{
				average = value;
			}
		}
		protected double average;

		public double Max
		{
			get
			{
				if (!paramsCalculated)
				{
					CalculateParams();
				}
				return max;
			}
			protected set
			{
				max = value;
			}
		}
		protected double max;

		public double Min
		{
			get
			{
				if (!paramsCalculated)
				{
					CalculateParams();
				}
				return min;
			}
			protected set
			{
				min = value;
			}
		}
		protected double min;

		public double StdDeviation
		{
			get
			{
				if (!paramsCalculated)
				{
					CalculateParams();
				}
				return stdDeviation;
			}
			protected set
			{
				stdDeviation = value;
			}
		}
		protected double stdDeviation;

		bool paramsCalculated;
	}


	/// <summary>
	/// stores price+volume series with tmestamp in list
	/// </summary>
	public class PriceVolumeSeriesDaily : PriceVolumeSeries
	{
		public PriceVolumeSeriesDaily(int size)
			: base(size)
		{
		}

		protected new void Add(double open, double close, double min, double max, int volume)
		{
			base.Add(open, close, min, max, volume);
		}

		public void Add(System.DateTime date, double open, double close, double min, double max, int volume)
		{
			Add(new CandleDaily(date, open, close, min, max, volume));
		}
	}

	/// <summary>
	/// describes triangle as a start and end index of the given series
	/// </summary>
	public class Shape
	{

		public Shape(int start, int end, PriceVolumeSeries series)
		{
			this.start = start;
			this.end = end;

			this.series = series;
		}

		public int start
		{
			get;
			protected set;
		}

		public int end
		{
			get;
			protected set;
		}

		public PriceVolumeSeries series
		{
			get;
			protected set;
		}

	}

	/// <summary>
	/// Class allows to create and read CSV files containing price and volume data
	/// </summary>
	public class CSVFile
	{
		public static bool Read(string filename, out PriceVolumeSeries series)
		{
			series = null;
			return false;
		}

		public static bool Write(string filename, PriceVolumeSeries series)
		{
			System.IO.FileStream fileStream = null;
			bool shouldClose = false;
			try
			{
				fileStream = new System.IO.FileStream(filename, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
				shouldClose = true;
				StreamWriter streamWriter = new StreamWriter(fileStream);
				streamWriter.Write(series.ToString(TA.PriceVolumeSeries.Format.CSV));
				streamWriter.Flush();
				fileStream.Close();
				shouldClose = false;
				fileStream = null;
			}
			catch (IOException e)
			{
				Console.WriteLine(e.ToString());
			}

			if (shouldClose)
			{
				fileStream.Close();
			}

			return false;
		}
	}




	/// <summary>
	/// ascending triangle is defined as 3 lows and two highs where susequent 
	/// lows are higher than previous
	/// and tops are the same. 
	/// </summary>
	public class AscendingTriangle
	{

		/// <summary>
		/// find ascending triangel in the series
		/// </summary>
		/// <param name="series">
		/// A <see cref="PriceVolumeSeries"/>
		/// The data to analyse
		/// </param>
		public AscendingTriangle(PriceVolumeSeries series, double stdDeviations)
		{
			this.series = series;
			this.shapes = new System.Collections.Generic.List<TA.Shape>(1);

			StdDeviations = stdDeviations;
		}
		/// <summary>
		/// find ascending triangel in the series
		/// </summary>
		/// <param name="series">
		/// A <see cref="PriceVolumeSeries"/>
		/// The data to analyse
		/// </param>
		public AscendingTriangle(PriceVolumeSeries series)
			: this(series, 0.2)
		{
		}

		/// <summary>
		/// process the data
		/// looks for largest triangles, ignores nesting (fractal shapes)
		/// </summary>
		public void Process()
		{
			// make sure that basic staff is calculated
			series.CalculateParams();
		}




		/// <summary>
		/// divide the series in two (or three) parts, calculate maximum and minimum in each part parts
		/// maximums should be "close" enough and subsequent low should be higher than the previous
		/// check if i have at least three points luying on the same ascending line and at least two
		/// points around the maximum
		/// 
		/// When dividing the range in two or three segments two different approaches are used
		/// - equal time
		/// - fib (0.38, 0.618, 1) time
		/// </summary>
		public bool isTriangle(int start, int end, PriceVolumeSeries series)
		{
			bool result = false;

			do
			{
				// no enough points for triangle
				if ((end - start) < 6) break;

				int halfPoint = (end - start) / 2;
				double average, min1, max1, min2, max2;
				int min1Idx, max1Idx, min2Idx, max2Idx;

				double stdDeviation = StdDeviations * series.StdDeviation;

				if ((series.Max - series.Min) < 2 * stdDeviation)
				{
					break;
				}

				PriceVolumeSeries.CalculateAverage(series, start, halfPoint - start + 1, out average, out min1, out min1Idx, out max1, out max1Idx);
				PriceVolumeSeries.CalculateAverage(series, halfPoint, end - halfPoint + 1, out average, out min2, out min2Idx, out max2, out max2Idx);

				//   max2-stdDeviation < max1 < max2+stdDeviation
				if ((max1 <= (max2 - stdDeviation)) || (max1 >= (max2 + stdDeviation)))
				{
					break;
				}

				if (min2 < min1 + stdDeviation)
				{
					break;
				}

				// i have two conditions - maxs are "close" and second low is higher than the first
				// do i have two highs and three lows ? two highs (two maxs) are in place. now lows
				double tangent = (min2 - min1) / (min2Idx - min1Idx);
				double target = min1 - tangent * (min1Idx - start);
				bool thirdPoint = false;
				bool closeUnder = false;

				// look for 3rd close on line described by the tangent
				// on the way I make sure that all closes are above the ascending line
				for (int i = start; i <= end; i++)
				{
					Candle candle = (Candle)series.Data[i];
					double close = candle.close;

					if ((i != min1Idx) && (i != min2Idx) && (close < target - stdDeviation))
					{
						closeUnder = true;
						break;
					}

					thirdPoint = thirdPoint | ((i != min1Idx) && (i != min2Idx) && (close > target - stdDeviation) && (close < target + stdDeviation));

					target = target + tangent;
				}

				result = !closeUnder && thirdPoint;

			}
			while (false);


			return result;
		}


		public PriceVolumeSeries series
		{
			get;
			protected set;
		}

		public double StdDeviations
		{
			get;
			protected set;
		}

		public System.Collections.Generic.List<Shape> shapes;
	}

}//namespace TA
