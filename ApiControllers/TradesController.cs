using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TraderData.Models.TradeImportModels;
using TraderData;

namespace Trader.ApiControllers
{
    [Produces("application/json")]
    [Route("api/Trades")]
    public class TradesController : Controller
    {
        private readonly ITrades _trades;
        public TradesController(ITrades trades)
        {
            _trades = trades;
        }
        // GET: api/Trades
        [HttpGet]
        public async Task<IEnumerable<TradeImport>> GetAsync(bool? active)
        {
            if (active != null)
            {
                if(active == true)
                    return await _trades.getActive(await _trades.getAll());
            }
            return await _trades.getAll();
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
