using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WpfAppWaves
{
	public class MarketViewModel : INotifyPropertyChanged
	{
		DataService ds = null;

		string baseUrl = @"https://query1.finance.yahoo.com/v7/finance";
		
		public ObservableCollection<MarketCandle> MarketCandles { get; set; } = new();
		public ObservableCollection<string> ValidRanges{ get; set; } = new();
		public ObservableCollection<string> StepIntervals { get; set; } = new();

		public MarketViewModel()
		{
			ds = new DataService();
			Symbol = "AAPL";

			SaveCommand = new Command(OnSave);
		}
		
		public string ValidRange
		{
			get; set;
		} = "1d";

		public string StepInterval
		{
			get; set;
		} = "1d";

		private string symbol = "";
		public string Symbol
		{
			get
			{
				return symbol.ToUpper();
			}

			set
			{
				if (symbol != value)
				{
					symbol = value;
					RaisePropertyChanged("Symbol");
				}
			}
		}

		private bool isSave = false;
		public bool IsSave
		{
			get
			{
				return isSave;
			}

			set
			{
				if (isSave != value)
				{
					isSave = value;
					RaisePropertyChanged("isSave");
				}
			}
		}

		//Save market Command
		public Command SaveCommand
		{
			get; set;
		}
				
		private void OnSave()
		{
			var options = new JsonSerializerOptions { WriteIndented = true };
			string markets = JsonSerializer.Serialize<ObservableCollection<MarketCandle>>(MarketCandles, options);			
			DataService.SaveFile(markets);
		}

		public async Task LoadMarketData()
		{
			MarketCandles.Clear();
			
			List<MarketCandle> candleList = new();

			string query = $"/chart/{Symbol}?range={ValidRange}&interval={StepInterval}&indicators=quote&includeTimestamps=true";

			Root market = await DataService.GetMarketData(baseUrl,query);

			if(market != null && market.chart != null && market.chart.error == null && market.chart.result.Count > 0)
			{
				var timestamp = market.chart.result[0].timestamp;
				
				//candels
				if (market.chart.result[0].indicators.quote.Count > 0)
				{
					var quotes = market.chart.result[0].indicators.quote[0];

					for (int i = 0; i < timestamp.Count; i++)
					{
						MarketCandle mc = new() 
						{ 
							OpenPrice = quotes.open[i],
							LowPrice = quotes.low[i],
							HighPrice = quotes.high[i],
							ClosePrice = quotes.close[i],
							Volume = quotes.volume[i],
							Timestamp = Utility.UnixTimeStampToDateTimeSeconds(timestamp[i])
					    };

						candleList.Add(mc);
					}

					foreach(var candle in candleList.OrderByDescending(x => x.Timestamp))
					{
						MarketCandles.Add(candle);
					}
				}

				if (ValidRanges.Count == 0)
				{
					//valid ranges and steps
					var validRanges = market.chart.result[0].meta.validRanges;

					foreach (var range in validRanges)
					{
						ValidRanges.Add(range);
						StepIntervals.Add(range);
					}
				}
			}

			IsSave = MarketCandles.Count > 0;
		}
				
		public event PropertyChangedEventHandler PropertyChanged;

		private void RaisePropertyChanged(string property)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(property));
			}
		}
	}
}
