using System;
using System.Collections.Generic;
using ChartJSCore.Models;
using System.Linq;

namespace Trader.Models.Charts
{
    public class CryptoLineChart
    {
        Chart _chart;

		public Chart getChart
		{
			get { return _chart; }
		}

		public CryptoLineChart(List<string> labels, List<List<double>> chartData)
        {
			Random RandGen = new Random();
			List<string> colours = chartData
				.Select(x => "rgba(" + RandGen.Next(255) + "," + RandGen.Next(255) + "," + RandGen.Next(255))
											  .ToList();


			_chart = new Chart();

			ChartJSCore.Models.Options options = new Options()
			{
				MaintainAspectRatio = true,
				Responsive = true,
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
			_chart.Options = options;


			_chart.Type = "line";

			ChartJSCore.Models.Data data = new ChartJSCore.Models.Data();
            data.Labels = labels;
            data.Datasets = new List<Dataset>();
            var count = 0;
            foreach (var dataset in chartData)
            {
                count += 1;
				LineDataset ds = new LineDataset()
                {
                    Label = "My " + count.ToString() + " dataset",
					Data = dataset,
					Fill = false,
					LineTension = 0.1,
					BackgroundColor = colours[count-1]+",0.4)",
					BorderColor = colours[count - 1] + ",1)",
					BorderCapStyle = "butt",
					BorderDash = new List<int> { },
					BorderDashOffset = 0.0,
					BorderJoinStyle = "miter",
					PointBorderColor = new List<string>() { colours[count - 1] + ",1)" },
					PointBackgroundColor = new List<string>() { "#fff" },
					PointBorderWidth = new List<int> { 1 },
					PointHoverRadius = new List<int> { 5 },
					PointHoverBackgroundColor = new List<string>() { "rgba(75,192,192,1)" },
					PointHoverBorderColor = new List<string>() { "rgba(220,220,220,1)" },
					PointHoverBorderWidth = new List<int> { 2 },
					PointRadius = new List<int> { 1 },
					PointHitRadius = new List<int> { 10 },
					SpanGaps = false
				};


				data.Datasets.Add(ds);
            }


			_chart.Data = data;
        }

    }
}
