using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppWaves
{
	public class MarketViewModel : INotifyPropertyChanged
	{	
		public ObservableCollection<Picture> Pictures { get; set; } = new ObservableCollection<Picture>();

		public MarketViewModel()
		{
			RunCommand = new Command(OnRun);
		}
				
		public Command RunCommand
		{
			get; set;
		}

		private void OnRun()
		{
			Pictures.Clear();

			for(int i = 1; i < 10000; i++)
			{
				Pictures.Add(new Picture { Number = i });
			}
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
