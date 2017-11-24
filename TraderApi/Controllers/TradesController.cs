using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TraderData.Models.TradeImportModels;
using TraderData;
using TraderApi.ApiModels;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace TraderApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TradesController : Controller
    {
        private readonly ITrades _trades;
        private readonly IInstrumentData _instrumentData;

        public TradesController(ITrades trades, IInstrumentData instrumentData)
        {
            _trades = trades;
            _instrumentData = instrumentData;
        }
        // GET: api/Trades
        [HttpGet]
        public async Task<IEnumerable<ApiTrade>> GetAsync(bool? active)
        {
            
            IEnumerable<TradeImport> trades;
            if(active == true)
                trades = await _trades.getActive(await _trades.getAll());
            else
                trades = await _trades.getAll();

            var ins = trades.Select(x => x.Instrument).Distinct();
            var prices = await _instrumentData.InstrumentPriceCache();



            return trades.Select(x => new ApiTrade
            {
                Value = prices.FirstOrDefault(y => y.instrument.InstrumentID == x.InstrumentId).price,
                Name = x.Instrument.Ticker,
                Quantity = x.Quantity,
                OriginalPrice = x.Value
            });
        }

        // GET: api/Trades/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/Trades
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Trades/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
