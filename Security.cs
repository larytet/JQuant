
using System;
using System.ComponentModel;

namespace JQuant
{
	public enum SecurityType
	{
		Derivative,
		Stock,
		Bond
	}

	public enum OptionType
	{
		[Description("CALL")]
		CALL,
		[Description("PUT")]
		PUT,
	}

	/// <summary>
	/// Security class implements generic security
	/// serving base for all instruments traded on TASE
	/// </summary>
	abstract public class Security
	{
		//Public properties
		/// <summary>
		/// security's id number on TASE
		/// </summary>
		public int IdNum
		{
			get;
			set;
		}

		/// <summary>
		/// security's name
		/// </summary>
		public string Name
		{
			get;
			protected set;
		}

		/// <summary>
		/// short textual description, probably a long name
		/// </summary>
		public string Description
		{
			get;
			protected set;
		}

		/// <summary>
		/// whether it's a stock, bond or derivative
		/// </summary>
		public SecurityType SecType
		{
			get;
			protected set;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public Security()
		{
		}

	}   //class Security 

	/// <summary>
	/// Generic option type - Currently used for MAOF index options.
	/// </summary>
	/// 
	public class Option : Security
	{
		/// <summary>
		/// Returns "C" for CALL and "P" for PUT
		/// </summary>
		/// <param name="OT"><see cref="JQuant.OptionType"/></param>
		/// <returns><see cref="System.String"/></returns>
		public static string OptionTypeToShortString(OptionType OT)
		{
			return EnumUtils.GetDescription(OT).Substring(0, 1);
		}

		/// <summary>
		/// How many units of underlying asset the option 
		/// refers to. On TASE Maof and stock options it's 100, 
		/// (like for most US stock options )
		/// for USD-ILS it's 10,000$. I set it to 100 by default in the constructor.
		/// Using it in computations enables us to get all the risks (Greeks) in terms of NIS.
		/// </summary>
		public int Multiplier
		{
			get;
			protected set;
		}

		//Properties
		/// <summary>
		/// PUT or CALL
		/// </summary>
		public OptionType Type
		{
			get;
			protected set;
		}

		/// <summary>
		/// Strike price
		/// </summary>
		public double Strike
		{
			get;
			protected set;
		}

		/// <summary>
		/// Expiration date
		/// </summary>
		public DateTime ExpDate
		{
			get;
			protected set;
		}

		/// <summary>
		/// Time to expiration, in years. As the expiration gets closer, 
		/// more precise calculation (hourly) is done. This is particularly 
		/// important for the last couple of trading days, as Theta
		/// (as well as Gamma) changes become significant, by hours.
		/// </summary>
		public double T
		{
			get
			{
				TimeSpan ts = DateTime.Today - ExpDate;
				if (ts.TotalDays > 3)
				{
					return ts.TotalDays / 365.0;
				}
				else return ts.TotalHours / (365.0 * 24.0);
			}
		}

		/// <summary>
		/// Implied volatility. Not a property, since it depends on some
		/// outside parameters:
		/// </summary>
		/// <param name="Premium"><see cref="System.Double"/>Option's market price</param>
		/// <param name="Rf"><see cref="System.Double"/>risk-free interest rate (decimal, not percent)</param>
		/// <param name="S"><see cref="System.Double"/>underlying asset's price</param>
		/// <returns><see cref="System.Double"/> Implied Volatility</returns>
		public double IV(double Premium, double Rf, double S)
		{
			return StatUtils.CalcIV(Type, Premium / Multiplier, Rf, Strike, S, T);
		}

		/// <summary>
		/// default constructor
		/// </summary>
		public Option()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="X"><see cref="System.Double"/> strike price</param>
		/// <param name="ot"><see cref="JQuant.OptionType"/> - PUT or CALL</param>
		/// <param name="ExDate"><see cref="System.DateTime"/> expiration date</param>
		/// <param name="OptID"><see cref="System.Int32"/> Option's Id number</param>
		public Option(double X, OptionType ot, DateTime ExDate, int OptID)
		{
			ExpDate = ExDate;
			Strike = X;
			Type = ot;
			IdNum = OptID;
			Multiplier = 100;

			SecType = SecurityType.Derivative;

			Name = OptionTypeToShortString(ot) + " " + X.ToString() + " "
				+ ExDate.Date.ToLongDateString().Split(',', ' ')[2].Substring(0, 3).ToUpper();  //3-letter month

			Description = EnumUtils.GetDescription(ot) + " " + X.ToString() + " " + ExDate.Date.ToString();
		}


		//Greeks
		/// <summary>
		/// Computes option's delta, given the market conditions:
		/// (volatility, underlying price, rf and T). Either numerical or analytical
		/// value is returned, depending on the presence of "Numerical" flag
		/// </summary>
		/// <param name="vol">volatility</param>
		/// <param name="rate">(risk-free) interest rate</param>
		/// <param name="S">underlying price</param>
		/// <param name="T">time to expiration</param>
		/// <param name="Numerical">perform numerical computation, rather than analytival one</param>
		/// <returns>Delta, options sensitivity to the underlying price changes</returns>
		public double Delta(double vol, double rate, double S, double T, bool Numerical)
		{
			if (Numerical) return StatUtils.CalcNDelta(Type, vol, rate, Strike, S, T, S + 1.0) * Multiplier;
			else return StatUtils.CalcDelta(Type, vol, rate, Strike, S, T) * Multiplier;
		}

		/// <summary>
		/// Computes option's Gamma, given the market conditions:
		/// (volatility, underlying price, rf and T)
		/// </summary>
		/// <param name="vol">volatility</param>
		/// <param name="rate">(risk-free) interest rate</param>
		/// <param name="S">underlying price</param>
		/// <param name="T">time to expiration</param>
		/// <returns>Gamma, option's Delta sensitivity 
		/// to the underlying price changes</returns>
		public double Gamma(double vol, double rate, double S, double T, bool Numerical)
		{
			if (Numerical) return StatUtils.CalcNGamma(Type, vol, rate, Strike, S, T, 1.0) * Multiplier;
			else return StatUtils.CalcGamma(vol, rate, Strike, S, T) * Multiplier;
		}

		/// <summary>
		/// Computes option's Vega, given the market conditions:
		/// (volatility, underlying price, rf and T)
		/// </summary>
		/// <param name="vol">volatility</param>
		/// <param name="rate">(risk-free) interest rate</param>
		/// <param name="S">underlying price</param>
		/// <param name="T">time to expiration</param>
		/// <returns>Vega, option's sensitivity 
		/// to the volatility changes</returns>
		public double Vega(double vol, double rate, double S, double T, bool Numerical)
		{
			if (Numerical) return StatUtils.CalcNVega(Type, vol, rate, Strike, S, T, vol / 100) * Multiplier;
			return StatUtils.CalcVega(vol, rate, Strike, S, T) * Multiplier;
		}
		/// <summary>
		/// Computes option's Theta, given the market conditions:
		/// (volatility, underlying price, rf and T)
		/// </summary>
		/// <param name="vol">volatility</param>
		/// <param name="rate">(risk-free) interest rate</param>
		/// <param name="S">underlying price</param>
		/// <param name="T">time to expiration</param>
		/// <returns>Theta, option's sensitivity 
		/// to the pass of time</returns>

		public double Theta(double vol, double rate, double S, double T, bool Numerical)
		{
			if (Numerical) return StatUtils.CalcNTheta(Type, vol, rate, Strike, S, T, 1.0 / 365.0) * Multiplier;
			else return StatUtils.CalcTheta(Type, vol, rate, Strike, S, T) * Multiplier;
		}



	}//class Option


	public class Stock : Security
	{
		public Stock()
			: base()
		{
			this.SecType = SecurityType.Stock;
		}
	}

}   //namespace JQuant
