using System;
using System.Collections.Generic;
using ChartJSCore.Models;
using System.Linq;

namespace Trader.Models.Charts
{
    public class CryptoBarChart
    {
        Chart _chart;

		public Chart getChart
		{
			get { return _chart; }
		}

		public CryptoBarChart(List<string> labels, List<double> chartData)
        {
			Random RandGen = new Random();
			List<string> colours = labels
				.Select(x => "rgba(" + RandGen.Next(255) + "," + RandGen.Next(255) + "," + RandGen.Next(255) + ",1)")
											  .ToList();


			_chart = new Chart();

			ChartJSCore.Models.Options options2 = new Options()
			{
				MaintainAspectRatio = true,
				Responsive = false,
				Scales = new Scales()
				{
					XAxes = new List<Scale>()
					{
						new Scale(){
							Ticks = new Tick()
							{
								BeginAtZero = true
							}
						}
					},

					YAxes = new List<Scale>()
					{
						new Scale(){
							Ticks = new Tick()
							{
								BeginAtZero = true
							}
						}
					}
				}
			};
			_chart.Options = options2;


			_chart.Type = "bar";

			ChartJSCore.Models.Data data2 = new ChartJSCore.Models.Data();
            data2.Labels = labels;

			BarDataset dataset2 = new BarDataset()
			{
				Label = "My Second dataset",
				Data = chartData,

				BackgroundColor = colours
				//BorderColor = "rgba(100, 40, 60,1)",

			};

			data2.Datasets = new List<Dataset>();
			data2.Datasets.Add(dataset2);

			_chart.Data = data2;
        }

    }
}
