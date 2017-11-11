using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TraderData;
using Trader.Models;
using TraderData.Models.InstrumentModels;

namespace Trader.Controllers
{
    public class InstrumentCarouselViewComponent : ViewComponent
    {
        private readonly IInstrumentData _instrumentData;
        private readonly IReference _reference;

        public InstrumentCarouselViewComponent(IInstrumentData instrumentData, IReference reference)
        {
            _instrumentData = instrumentData;
            _reference = reference;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new InstrumentInfoViewModel();

            var temp2 = await _instrumentData.InstrumentPriceCache();
            var temp = temp2.Select(x => new InstrumentInfoModel()
            {
                instrument = x.instrument,
                price = x.price
            });
            model.instrumentInfoList = temp;

            return View(model);
        }
    }
}