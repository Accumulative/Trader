using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TraderData;
using Microsoft.AspNetCore.Authorization;
using Trader.Models.Charts;
using Microsoft.AspNetCore.Identity;
using Trader.Models;
using TraderData.Models;
using TraderData.Models.TradeImportModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using ChartJSCore;
using Trader.Models.InstrumentViewModels;
using System;
using TraderData.Models.InstrumentModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Trader.Controllers
{
    [ViewComponent(Name = "InstrumentGraphViewComponent")]
    public class InstrumentGraphViewComponent : ViewComponent
    {
		private readonly IInstrumentData _instrumentData;
		private readonly IReference _reference;

		public InstrumentGraphViewComponent(IInstrumentData instrumentData, IReference reference)
		{
			_reference = reference;
			_instrumentData = instrumentData;
		}
        public async Task<IViewComponentResult> InvokeAsync(InstrumentGraphFilter viewModel)
        {

            var insPrices = new List<InstrumentData>();

            // Apply filters
            if (viewModel != null)
            {
                if (viewModel.instrumentId != null)
                {
                    insPrices = await _instrumentData.GetValueBetweenDates((int)viewModel.instrumentId, DateTime.Now.AddYears(-1), DateTime.Now);
                }
                else
                {
                    insPrices = await _instrumentData.GetValueBetweenDates(1, DateTime.Now.AddYears(-1), DateTime.Now);
                }
                if ( viewModel.monthNo != null)
                {
                    insPrices = insPrices.Where(x => x.dateTime.Month + "/" + x.dateTime.Year == viewModel.monthNo).ToList();
                }
            }
            else
            {
                insPrices = await _instrumentData.GetValueBetweenDates(1, DateTime.Now.AddYears(-1), DateTime.Now);
            }
            

            insPrices = insPrices.OrderBy(x => x.dateTime).ToList();
            var insPricesToShow = insPrices
                .GroupBy(dt => dt.dateTime.Date)
                    .Select(g => new
                    {
                        Price = g.Average(x => x.price)

                    });
            CryptoLineChart lineChart = new CryptoLineChart(insPrices.Select(x => x.dateTime.Date.ToString()).Distinct().ToList(), new List<List<double>>() {
            insPricesToShow.Select(x => (double)x.Price).ToList()
            });

            InstrumentGraphViewModel model = new InstrumentGraphViewModel
            {
                chartOne = lineChart.getChart
            };

            return View(model);
        }
    }
}
