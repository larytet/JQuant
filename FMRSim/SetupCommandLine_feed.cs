
using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;

#if USEFMRSIM
using TaskBarLibSim;
#else
using TaskBarLib;
#endif

namespace JQuant
{
	partial class Program
	{
		protected void feedGetToFileCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			feedGetSeriesCallback(iWrite, cmdName, cmdArguments, true);
		}

		protected void feedGetSeriesCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			feedGetSeriesCallback(iWrite, cmdName, cmdArguments, false);
		}

		protected TA.PriceVolumeSeries FeedSeries;
		protected void feedGetSeriesCallback(IWrite iWrite, string cmdName, object[] cmdArguments, bool outputToFile)
		{
			int argsNum = cmdArguments.Length;
			string symbol = null;
			DateTime from = DateTime.Today - TimeSpan.FromDays(30);
			DateTime to = DateTime.Now;
			DateTime tmp;
			bool result = true;
			string[] args = (string[])cmdArguments;
			string filename = null;

			switch (argsNum)
			{
				case 1:
					result = false;
					break;
				case 2:
					if (outputToFile)
					{
						result = false;
					}
					else
					{
						symbol = args[1];
					}
					break;
				case 3:
					if (outputToFile)
					{
						symbol = args[1];
						filename = args[2];
					}
					else
					{
						symbol = args[1];
						result = DateTime.TryParse(args[2], out tmp);
						if (result) from = tmp;
					}
					break;
				case 4:
					if (outputToFile)
					{
						symbol = args[1];
						filename = args[2];
						result = DateTime.TryParse(args[3], out tmp);
						if (result) from = tmp;
					}
					else
					{
						symbol = args[1];
						result = DateTime.TryParse(args[2], out tmp);
						if (result) from = tmp;
						result = DateTime.TryParse(args[3], out tmp);
						if (result) to = tmp;
					}
					break;
				case 5:
				default:
					if (outputToFile)
					{
						symbol = args[1];
						filename = args[2];
						result = DateTime.TryParse(args[3], out tmp);
						if (result) from = tmp;
						result = DateTime.TryParse(args[4], out tmp);
						if (result) to = tmp;
					}
					else
					{
						symbol = args[1];
						result = DateTime.TryParse(args[2], out tmp);
						if (result) from = tmp;
						result = DateTime.TryParse(args[3], out tmp);
						if (result) to = tmp;
					}
					break;
			}

			if (!result)
			{
				iWrite.WriteLine("Please, specify symbol, from and to date");
				return;
			}

			IDataFeed dataFeed = new FeedYahoo();
			result = dataFeed.GetSeries(from, to, new Equity(symbol), DataFeed.DataType.Daily, out FeedSeries);
			TA.PriceVolumeSeries series = FeedSeries;
			if (result)
			{
				System.IO.FileStream fileStream = null;
				iWrite.WriteLine("Parsed " + series.Data.Count + " entries");
				if (outputToFile)
				{
					bool shouldClose = false;
					try
					{
						fileStream = new System.IO.FileStream(filename, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
						shouldClose = true;
						StreamWriter streamWriter = new StreamWriter(fileStream);
						streamWriter.Write(series.ToString(TA.PriceVolumeSeries.Format.Table));
						streamWriter.Flush();
						fileStream.Close();
						shouldClose = false;
					}
					catch (IOException e)
					{
						iWrite.WriteLine(e.ToString());
					}
					if (shouldClose)
					{
						fileStream.Close();
					}
				}
				else
				{
					iWrite.WriteLine(series.ToString(TA.PriceVolumeSeries.Format.Table));
				}
			}
			else
			{
				iWrite.WriteLine("Failed to read data from server");
			}

		}

		protected void feedGetSeriesFromFileCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			int argsNum = cmdArguments.Length;
			string[] args = (string[])cmdArguments;
			//            string filename = "yahoo_feed_data.csv";
			string filename = "yahoo_feed_data_5y.csv";
			switch (argsNum)
			{
				case 2:
					filename = args[1];
					break;
				default:
					break;
			}

			if (filename == null)
			{
				iWrite.WriteLine("Filename is not specified");
			}
			else
			{
				IDataFeed dataFeed = new FeedYahoo();
				bool result = dataFeed.GetSeries(filename, out FeedSeries);
				TA.PriceVolumeSeries series = FeedSeries;
				if (result)
				{
					iWrite.WriteLine("Parsed " + series.Data.Count + " entries");
				}
				else
				{
					iWrite.WriteLine("Failed to read data from server");
				}
			}
		}

		protected void feedFisherTransformCallback(IWrite iWrite, string cmdName, object[] cmdArguments)
		{
			TA.PriceVolumeSeries series = FeedSeries;

			// first step is to calculate max, min, average etc.
			series.CalculateParams();
			iWrite.WriteLine("Series: count=" + series.Data.Count + ", max=" + series.Max + ", min=" + series.Min + ", average=" + series.Average + ", sd=" + series.StdDeviation);

			double average, max, min;

			double[] data = TA.PriceVolumeSeries.GetClose(series);
			TA.PriceVolumeSeries.CalculateAverage(data, 0, data.Length, out average, out max, out min);
			iWrite.WriteLine("Data: count=" + data.Length + ",max=" + series.Max + ", min=" + series.Min + ", average=" + series.Average + ", sd=" + series.StdDeviation);

			int windowSize = 10;
			// now normalize the data
			data = TA.PriceVolumeSeries.Normalize(data, windowSize);
			TA.PriceVolumeSeries.CalculateAverage(data, 0, data.Length, out average, out max, out min);
			iWrite.WriteLine("Normalize(data): count=" + data.Length + ",max=" + max + ", min=" + min + ", average=" + average);

			TA.PriceVolumeSeries.EMA(data, 0.5);
			TA.PriceVolumeSeries.CalculateAverage(data, 0, data.Length, out average, out max, out min);
			iWrite.WriteLine("EMA(Normalize(data)): count=" + data.Length + ",max=" + max + ", min=" + min + ", average=" + average);

			TA.PriceVolumeSeries.Fisher(data);
			TA.PriceVolumeSeries.CalculateAverage(data, 0, data.Length, out average, out max, out min);
			iWrite.WriteLine("Fisher(EMA(Normalize(data))): count=" + data.Length + ",max=" + max + ", min=" + min + ", average=" + average);

			TA.PriceVolumeSeries.EMA(data, 0.5);
			TA.PriceVolumeSeries.CalculateAverage(data, 0, data.Length, out average, out max, out min);
			iWrite.WriteLine("EMA(Fisher(EMA(Normalize(data)))): count=" + data.Length + ",max=" + max + ", min=" + min + ", average=" + average);

			int i;
			for (i = 0; i < data.Length; i++)
			{
				//                iWrite.WriteLine(""+data[i]);
			}

			signalPerformanceOptimization(series, data, windowSize);
		}

		protected class Trade
		{
			public double entry;
			public TA.Candle candleEntry;
			public double exit;
			public TA.Candle candleExit;
			public bool isBuy;
			public double p;
			public int days;
			public int idx;
		}
		protected class TradeSession
		{
			public double p;
			public int days;
			public int hits;
			public double maxDrawDown;
			public double stopLoss;
			public double sellSignal;
			public double buySignal;
			public int maxDays;

			public System.Collections.Generic.List<Trade> trades;
		}

		protected void signalPerformanceOptimization(TA.PriceVolumeSeries series, double[] data, int windowSize)
		{
			double stopLossFrom = 0.02;
			double stopLossTo = 0.04;
			double stopLossStep = 0.001;
			double buySignalFrom = 0.9; ;
			double buySignalTo = 1.1;
			double sellSignalFrom = 0.9;
			double sellSignalTo = 1.1;
			double signalStep = 0.002;
			int maxDays = 1000;
			int daysFrom = 1;
			int daysTo = 10;
			int daysStep = 1;
			long loopsTotal = (long)(((buySignalTo - buySignalFrom) / signalStep) * ((sellSignalFrom - sellSignalTo) / signalStep) * ((stopLossTo - stopLossFrom) / stopLossStep) * (daysFrom - daysTo));

			System.Collections.Generic.List<TradeSession> bestBlocks = new System.Collections.Generic.List<TradeSession>(40);

			double buySignal = buySignalFrom;
			while (buySignal < buySignalTo)
			{
				double sellSignal = sellSignalFrom;
				while (sellSignal < sellSignalTo)
				{
					double stopLoss = stopLossFrom;
					while (stopLoss < stopLossTo)
					{
						int days = daysFrom;
						while (days < daysTo)
						{
							signalPerformanceOptimization(series, data, windowSize, stopLoss, maxDays, sellSignal, buySignal, days, bestBlocks);
							loopsTotal--;
							if ((loopsTotal & 0xFFFF) == 0) System.Console.Write("." + loopsTotal);
							days = days + daysStep;
						}
						stopLoss += stopLossStep;
					}
					sellSignal += signalStep;
				}
				buySignal += signalStep;
			}

			TradeSession bs = findBest(bestBlocks);
			System.Console.WriteLine("pTotal=" + (int)(100 * bs.p) + ", days=" + bs.days +
									  ", hits=" + bs.hits + ", maxDrawDown=" + (int)(100 * bs.maxDrawDown) + "" +
										 ", trades=" + bs.trades.Count + ", stopLoss=" + bs.stopLoss + ", maxDays=" + bs.maxDays +
										 ", sellSig=" + bs.sellSignal + ", buySig=" + bs.buySignal + ", wind=" + windowSize);
			signalPerformancePrintTrades(bs.trades);

		}
		protected void _signalPerformanceOptimization(TA.PriceVolumeSeries series, double[] data, int windowSize)
		{
			double stopLossStep = 0.001;

			System.Collections.Generic.List<TradeSession> bestBlocks = new System.Collections.Generic.List<TradeSession>(40);

			double buySignal = -1.94;
			double sellSignal = 1.7;
			double stopLoss = 0.026;
			signalPerformanceOptimization(series, data, windowSize, stopLoss, 1000, 1.05, 0.9, 3, bestBlocks);

			TradeSession bs = findBest(bestBlocks);
			System.Console.WriteLine("pTotal=" + (int)(100 * bs.p) + ", days=" + bs.days +
									  ", hits=" + bs.hits + ", maxDrawDown=" + (int)(100 * bs.maxDrawDown) + "" +
										 ", trades=" + bs.trades.Count + ", stopLoss=" + bs.stopLoss + ", maxDays=" + bs.maxDays +
										 ", sellSig=" + bs.sellSignal + ", buySig=" + bs.buySignal + ", wind=" + windowSize);
			signalPerformancePrintTrades(bs.trades);

		}

		protected TradeSession findBest(System.Collections.Generic.List<TradeSession> sessions)
		{
			TradeSession bestSession = null;
			double p = Double.MinValue;
			double maxDrawDown = Double.MaxValue;
			int hits = Int32.MinValue;
			int trades = Int32.MinValue;
			foreach (TradeSession s in sessions)
			{
				if (s.p > p)
				{
					p = s.p;
					maxDrawDown = s.maxDrawDown;
					bestSession = s;
					trades = s.trades.Count;
				}
			}

			return bestSession;
		}

		protected static void signalPerformancePrintTrades(System.Collections.Generic.List<Trade> trades)
		{
			foreach (Trade t in trades)
			{
				string buy = "Buy:";
				if (!t.isBuy) buy = "Sell:";
				System.Console.WriteLine(buy + " entry=" + t.entry + " exit=" + t.exit + " days=" + t.days +
										 " p=" + (int)(100 * t.p) + " idx=" + t.idx);
				System.Console.WriteLine("\tEntry:" + t.candleEntry.ToString());
				System.Console.WriteLine("\tExit:" + t.candleExit.ToString());
				System.Console.WriteLine(" ");
			}
		}

		protected static void signalPerformanceGetTrades(System.Collections.Generic.List<Trade> trades, out int days, out int hits, out double p, out double maxDrawDown)
		{
			p = 1.0;
			days = 0;
			hits = 0;
			maxDrawDown = Double.MaxValue;

			foreach (Trade t in trades)
			{
				days += t.days;
				p = p * (1 + t.p);
				if (t.p > 0) hits++;
				if (t.p < maxDrawDown) maxDrawDown = t.p;
			}
		}

		protected bool signalSell(TA.PriceVolumeSeries series, int idx, double diff, int days)
		{
			bool res = false;

			if (idx >= days)
			{
				TA.Candle candleCur = (TA.Candle)series.Data[idx];
				TA.Candle candlePrev = (TA.Candle)series.Data[idx - days];
				res = (candleCur.high * diff < candlePrev.high);
			}

			return res;
		}

		protected bool signalBuy(TA.PriceVolumeSeries series, int idx, double diff, int days)
		{
			bool res = false;

			if (idx >= days)
			{
				TA.Candle candleCur = (TA.Candle)series.Data[idx];
				TA.Candle candlePrev = (TA.Candle)series.Data[idx - days];
				res = (candleCur.high * diff > candlePrev.high);
			}

			return res;
		}

		protected void signalPerformanceOptimization(TA.PriceVolumeSeries series, double[] data, int windowSize,
													 double stopLoss,
													 int maxDays, double diffSell, double diffBuy, int days,
													 System.Collections.Generic.List<TradeSession> bestBlocks)
		{
			Trade trade;
			System.Collections.Generic.List<Trade> trades = new System.Collections.Generic.List<Trade>(10);
			int i = 0;
			while (i < (data.Length - 1))
			{
				int idx = i + windowSize;
				TA.Candle candle = (TA.Candle)series.Data[idx];
				double p;
				if (signalSell(series, idx, diffSell, days))
				{
					signalPerformance(series, stopLoss, maxDays, idx, false, diffSell, diffBuy, days, out trade);
					if (trades.Count < 200) trades.Add(trade);
					else break;
					i += Math.Max(1, trade.days);
					//                        System.Console.Write("\tSell at "+idx+" entry="+trade.entry+" exit="+trade.exit+" "+candle.ToString());
					//                        System.Console.WriteLine(" p="+trade.p+", days="+trade.days+", exit at "+(idx+trade.days));
				}
				else if (signalBuy(series, idx, diffBuy, days))  // buy condition and trigger
				{
					signalPerformance(series, stopLoss, maxDays, idx, true, diffSell, diffBuy, days, out trade);
					if (trades.Count < 200) trades.Add(trade);
					else break;
					i += Math.Max(1, trade.days);
					//                        System.Console.Write("\tBuy at "+idx+" entry="+trade.entry+" exit="+trade.exit+" "+candle.ToString());
					//                        System.Console.WriteLine(" p="+trade.p+", days="+trade.days+", exit at "+(idx+trade.days));
				}
				else i++;
			}

			double pTotal;
			int daysTotal;
			int hits;
			double maxDrawDown;
			signalPerformanceGetTrades(trades, out daysTotal, out hits, out pTotal, out maxDrawDown);
			int misses = trades.Count - hits;
			if ((pTotal > 1.1) && (trades.Count > 5) && (bestBlocks.Count < 1000))
			{
				TradeSession ts = new TradeSession();
				ts.trades = trades;
				ts.maxDays = maxDays;
				ts.p = pTotal;
				ts.days = daysTotal;
				ts.hits = hits;
				ts.stopLoss = stopLoss;
				ts.sellSignal = diffSell;
				ts.buySignal = diffBuy;
				ts.maxDays = maxDays;
				ts.maxDrawDown = maxDrawDown;
				bestBlocks.Add(ts);
				System.Console.Write("+");
				//                System.Console.WriteLine("p="+pTotal);
				//                signalPerformancePrintTrades(trades);
			}
			if ((pTotal > 2) && (trades.Count > 5) && (bestBlocks.Count > 1000))
				System.Console.Write("-");
			//            System.Console.Write("1");
		}

		protected bool signalStop(bool isBuy, double stopLoss, int maxDays, double entry, double last, double current, int days, double bestClose)
		{
			double deltaTrade = (entry - current) / entry;
			double trailing = (bestClose - current) / bestClose;
			double deltaCandle = (last - current) / last;

			if (isBuy)
			{
				deltaTrade = -deltaTrade;
				deltaCandle = -deltaCandle;
				trailing = -trailing;
			}

			bool res = false;
			double stopLoss_1 = -stopLoss;
			double stopLoss_2 = -2 * stopLoss;
			double stopLoss_3 = -3 * stopLoss;
			res = res || (deltaTrade < stopLoss_1);
			res = res || (trailing < stopLoss_1);
			//            res = res || (days > maxDays);

			return res;
		}

		protected void signalPerformance(TA.PriceVolumeSeries series, double stopLoss, int maxDays, int idx, bool isBuy, double diffSell, double diffBuy, int sigDays, out Trade trade)
		{
			trade = new Trade();
			trade.isBuy = isBuy;
			trade.idx = idx;

			int count = series.Data.Count;
			TA.Candle candle = (TA.Candle)series.Data[idx];
			trade.candleEntry = candle;
			double entryPoint = candle.close;
			double close = entryPoint;
			double bestClose = close;
			bool isSell = !isBuy;
			int i;
			int days = 0;
			for (i = idx + 1; i < count; i++)
			{
				candle = (TA.Candle)series.Data[i];
				days++;
				if (signalStop(isBuy, stopLoss, maxDays, entryPoint, close, candle.close, days, bestClose))
				{
					break;
				}
				if (isBuy && signalSell(series, idx, diffSell, sigDays))
				{
					break;
				}
				if (isSell && signalBuy(series, idx, diffSell, sigDays))
				{
					break;
				}
				close = candle.close;
				if (isBuy)
				{
					if (bestClose < close) bestClose = close;
				}
				else
				{
					if (bestClose > close) bestClose = close;
				}
			}
			double delta;
			delta = entryPoint - candle.close;
			if (isBuy) delta = -delta;

			double p = delta / entryPoint;
			trade.candleExit = candle;
			trade.entry = entryPoint;
			trade.exit = candle.close;
			trade.p = p;
			trade.days = days;
		}






		protected void LoadCommandLineInterface_feed()
		{
			Menu menuFeed = cli.RootMenu.AddMenu("feed", "Trading data feeds",
								   " Get data from the data feeds, TA screens");
			menuFeed.AddCommand("get", "Get price/volume series",
								  " Get price/volume daily series for the specified stock symbol. Args: symbol [fromDate[toDate]]", feedGetSeriesCallback);
			menuFeed.AddCommand("gf", "Write price/volume series to file",
								  " Get price/volume daily series for the specified stock symbol and write to file. Args: symbol filename [fromDate[toDate]]", feedGetToFileCallback);
			menuFeed.AddCommand("rf", "Get price/volume series from file",
								  " Load price/volume daily series for the specified file. Args: filename", feedGetSeriesFromFileCallback);
			menuFeed.AddCommand("fisher", "Fisher transform",
								  "Apply Fisher transform to the latest loaded series", feedFisherTransformCallback);
		}

	}
}
