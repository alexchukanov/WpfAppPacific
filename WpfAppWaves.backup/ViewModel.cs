using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppWaves
{
	public class ViewModel : INotifyPropertyChanged
	{
		DataService ds = null;

		string baseUrl = @"https://query1.finance.yahoo.com/v7/finance";
		//string query = @"/download/AAPL?period1=1492524105&period2=1495116105&interval=1d&events=history&crumb=tO1hNZoUQeQ";
		//string query = @"/chart/AAPL?period1=1492524105&period2=1495116105&interval=1d&events=history&crumb=tO1hNZoUQeQ";
		string query = @"/chart/AAPL?range=1mo&interval=1d&indicators=quote&includeTimestamps=true";

		public ViewModel()
		{
			ds = new DataService();

			LoadCommand = new Command(OnLoad);
		}
				
		public Command LoadCommand
		{
			get; set;
		}

		private async void OnLoad()
		{
			Root response = await DataService.GetMarketData(baseUrl,query);
			//var responseStatus = Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(response);

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
