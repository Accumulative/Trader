using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TraderData;
using Trader.Models;

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
            var instruments = await _reference.getInstruments();
            model.instrumentInfoList = instruments.Select(x => new InstrumentInfoModel()
            {
                instrument = x,
                value = _instrumentData.GetCurrentValue(x)
            });

            return View(model);
        }
    }
}