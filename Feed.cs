
using System;
using System.Text;
using System.Net;
using System.IO;

namespace JQuant
{

	public class Equity
	{
		public Equity(string symbol)
		{
			Symbol = symbol;
		}


		public string Symbol
		{
			get;
			protected set;
		}
	}

	public interface IDataFeed
	{
		bool GetSeries(DateTime start, DateTime end, Equity equity, DataFeed.DataType dataType, out TA.PriceVolumeSeries series);

		bool GetSeries(string fileName, out TA.PriceVolumeSeries series);

		DataFeed.DataType GetDataType();
	}

	public class DataFeed
	{
		[Flags]
		public enum DataType
		{
			Daily = 0x0001,
			Weekly = 0x0002,
			Monthly = 0x0004,
			Dividends = 0x0008,
			PriceToBook = 0x0010,
			PriceToEarnings = 0x0020
		}
	}

	/// <summary>
	/// opens data feed, build object TA.PriceVolumeSeries
	/// example of Yahoo Feed
	/// 
	/// MMM Jan 1, 1970 - 27 Sep,2009, daily
	/// http://ichart.finance.yahoo.com/table.csv?s=MMM&a=00&b=2&c=1970&d=08&e=27&f=2009&g=d&ignore=.csv
	/// 
	/// MMM Mar 5, 1970 - 27 Feb,2009, weekly
	/// http://ichart.finance.yahoo.com/table.csv?s=MMM&a=02&b=5&c=1970&d=01&e=27&f=2009&g=w&ignore=.csv
	/// 
	/// MMM Apr 5, 1970 - 27 Feb,2009, weekly
	/// http://ichart.finance.yahoo.com/table.csv?s=MMM&a=03&b=5&c=1970&d=01&e=27&f=2009&g=w&ignore=.csv
	/// 
	/// MMM Apr 5, 1970 - 27 Feb,2009, daily
	/// http://ichart.finance.yahoo.com/table.csv?s=MMM&a=03&b=5&c=1970&d=01&e=27&f=2009&g=d&ignore=.csv
	/// </summary>
	public class FeedYahoo : DataFeed, IDataFeed
	{
		public FeedYahoo()
		{
		}

		/// <summary>
		/// return list of equities SP500
		/// 
		/// URL: http://download.finance.yahoo.com/d/quotes.csv?s=@%5EGSPC&f=sl1d1t1c1ohgv&e=.csv&h=0
		/// 
		/// Content: "A",27.58,"9/25/2009","4:01pm",-0.25,27.77,27.84,27.48,3689109 followed by "0x0D0x0A"
		/// </summary>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// True if Ok
		/// </returns>
		public bool GetSP500(out System.Collections.Generic.List<Equity> equities)
		{
			equities = null;

			string url = "http://download.finance.yahoo.com/d/quotes.csv?s=@%5EGSPC&f=sl1d1t1c1ohgv&e=.csv&h=0";

			Console.WriteLine("Get data from URL " + url);

			HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
			HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
			Stream readStream = httpResponse.GetResponseStream();



			return false;
		}

		public bool GetSeries(string fileName, out TA.PriceVolumeSeries series)
		{
			bool result = false;
			series = null;
			FileStream fileStream = null;
			do
			{
				try
				{
					fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
				}
				catch (IOException e)
				{
					System.Console.WriteLine("Failed to open file " + fileName + " for reading");
					System.Console.WriteLine(e.ToString());
					break;
				}

				// preallocate some memory
				series = new TA.PriceVolumeSeries(50);
				result = fillDataArray(fileStream, series);
				if (!result)
				{
					System.Console.WriteLine("Failed to parse file " + fileName);
				}
			}
			while (false);

			if (fileStream != null)
			{
				fileStream.Close();
			}

			return result;
		}

		public bool GetSeries(DateTime start, DateTime end, Equity equity, DataFeed.DataType dataType, out TA.PriceVolumeSeries series)
		{
			string symbol = equity.Symbol;
			bool result = false;
			series = null;

			do
			{
				int size = (int)Math.Round((end - start).TotalDays);
				if (size <= 0)
				{
					break;
				}

				// preallocate some memory
				series = new TA.PriceVolumeSeries(size);

				string url;
				result = buildURL(symbol, start, end, dataType, out url);
				if (!result)
				{
					break;
				}


				Console.WriteLine("Get data from URL " + url + " between " + start + " and " + end);

				HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);

				HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();

				Stream readStream = httpResponse.GetResponseStream();

				result = fillDataArray(readStream, series);
			}
			while (false);


			return result;

		}

		public DataFeed.DataType GetDataType()
		{
			return DataFeed.DataType.Daily |
					DataFeed.DataType.Weekly |
					DataFeed.DataType.Monthly |
					DataFeed.DataType.Dividends;
		}

		static bool fillDataArray(Stream streamReader, TA.PriceVolumeSeries series)
		{
			bool result = false;
			byte[] buf = new byte[8192];
			StringBuilder sb = new StringBuilder();


			// read all data from the stream
			while (true)
			{
				int count = streamReader.Read(buf, 0, buf.Length);
				// nothing to read ?
				if (count == 0)
				{
					break;
				}

				// translate from bytes to ASCII text
				string tempString = Encoding.ASCII.GetString(buf, 0, count);

				// continue building the string
				sb.Append(tempString);
			}

			string str = sb.ToString();

			// buffer sb contains lines separated by 0x0A (line feed)
			// skip first line
			int indexEnd;
			int indexStart = str.IndexOf((char)0x0A, 0);
			result = false;

			do
			{
				indexStart += 1;
				indexEnd = str.IndexOf((char)0x0A, indexStart);

				if (indexEnd <= 0)
				{
					System.Console.WriteLine("195:Failed to parse price data " + str);
					break;
				}

				result = ((indexEnd - indexStart) > 1);
				if (!result)
				{
					System.Console.WriteLine("202:Failed to parse price data " + str);
					break;
				}

				string data = str.Substring(indexStart, indexEnd - indexStart);
				TA.Candle candle;
				result = strToCandle(data, out candle);
				if (!result)
				{
					System.Console.WriteLine("211:Failed to parse candle data " + data + " indexStart=" + indexStart + " indexEnd=" + indexEnd);
					break;
				}

				series.Data.Insert(0, candle);
				indexStart = indexEnd;

				// end of data ?
				if (indexEnd >= (str.Length - 1))
				{
					result = true;
					break;
				}
			}
			while (true);

			if (!result)
			{
			}

			return result;

		}

		/// <summary>
		/// Example
		/// Date,Open,High,Low,Close,Volume,Adj Close
		/// 2009-09-25,74.04,74.39,73.37,73.80,3470700,73.80
		/// </summary>
		static bool strToCandle(string str, out TA.Candle candle)
		{
			candle = null;
			int start = 0;
			int end = 0;
			bool result = true;

			// skip date
			end = str.IndexOf(',', start);
			if (end <= 1) // no date ?
			{
				System.Console.WriteLine("242:Failed to parse candle data " + str);
				return false;
			}
			start = end + 1;

			double open, close, max, min;
			int volume;
			string strVal;

			// open
			end = str.IndexOf(',', start);
			if ((end <= 1) || (end <= start)) // no open ?
			{
				System.Console.WriteLine("255:Failed to parse candle data " + str);
				return false;
			}
			strVal = str.Substring(start, end - start);
			result = Double.TryParse(strVal, out open);
			if (!result)
			{
				System.Console.WriteLine("262:Failed to parse candle data " + str + "!" + strVal);
				return false;
			}
			start = end + 1;

			// max
			end = str.IndexOf(',', start);
			if ((end <= 1) || (end <= start)) // no max ?
			{
				System.Console.WriteLine("271:Failed to parse candle data " + str);
				return false;
			}
			strVal = str.Substring(start, end - start);
			result = Double.TryParse(strVal, out max);
			if (!result)
			{
				System.Console.WriteLine("278:Failed to parse candle data " + str + "!" + strVal);
				return false;
			}
			start = end + 1;

			// min
			end = str.IndexOf(',', start);
			if ((end <= 1) || (end <= start)) // no min ?
			{
				System.Console.WriteLine("287:Failed to parse candle data " + str);
				return false;
			}
			strVal = str.Substring(start, end - start);
			result = Double.TryParse(strVal, out min);
			if (!result)
			{
				System.Console.WriteLine("294:Failed to parse candle data " + str + "!" + strVal);
				return false;
			}
			start = end + 1;


			// close
			end = str.IndexOf(',', start);
			if ((end <= 1) || (end <= start)) // no close ?
			{
				System.Console.WriteLine("304:Failed to parse candle data " + str);
				return false;
			}
			strVal = str.Substring(start, end - start);
			result = Double.TryParse(strVal, out close);
			if (!result)
			{
				System.Console.WriteLine("311:Failed to parse candle data " + str + "!" + strVal);
				return false;
			}
			start = end + 1;

			// volume
			end = str.IndexOf(',', start);
			if ((end <= 1) || (end <= start)) // no open ?
			{
				System.Console.WriteLine("320:Failed to parse candle data " + str);
				return false;
			}
			strVal = str.Substring(start, end - start);
			result = Int32.TryParse(strVal, out volume);
			if (!result)
			{
				System.Console.WriteLine("327:Failed to parse candle data " + str + "!" + strVal);
				return false;
			}
			start = end + 1;

			candle = new TA.Candle(open, close, min, max, volume);

			return true;
		}


		/// <summary>
		/// Example
		/// MMM Apr 5, 1970 - 27 Feb,2009, daily
		/// http://ichart.finance.yahoo.com/table.csv?s=MMM&a=03&b=5&c=1970&d=01&e=27&f=2009&g=d&ignore=.csv
		/// </summary>
		static bool buildURL(string symbol, DateTime start, DateTime end, DataFeed.DataType dataType, out string url)
		{
			url = "";
			string dataTypeURL = DataTypeToURL(dataType);

			if (dataTypeURL == "")
			{
				return false;
			}

			symbol = symbol.ToUpper();
			url = "http://ichart.finance.yahoo.com/table.csv?s=" + symbol +
				"&a=" + Month2Str(start.Month) +
				"&b=" + start.Day +
				"&c=" + start.Year +
				"&d=" + end.Month +
				"&e=" + end.Day +
				"&e=" + end.Year +
				"&g=" + dataTypeURL +
				"&ignore=.csv";

			return true;
		}

		static string Month2Str(int month)
		{
			string result;
			month = month - 1;
			if (month < 10)
				result = "0" + month;
			else
				result = "" + month;

			return result;
		}

		static string DataTypeToURL(DataFeed.DataType dataType)
		{
			switch (dataType)
			{
				case DataType.Daily:
					return "d";
				case DataType.Monthly:
					return "m";
				case DataType.Weekly:
					return "w";
				case DataType.Dividends:
					return "v";
				default:
					return "";
			}
		}
	}//class FeedYahoo
}//namespace JQuant
