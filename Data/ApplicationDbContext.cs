﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Trader.Models;
using Trader.Models.TradeImportModels;
using Trader.Models.FileImportModels;

namespace Trader.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        ILoggerFactory _loggerFactory;
        private const string BlahCacheKey = "blah-cache-key";
        private const string BlahCacheKey2 = "blah-cache-key2";
        private readonly IMemoryCache _cache;

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILoggerFactory loggerFactory, IMemoryCache cache)
            : base(options)
        {
            _loggerFactory = loggerFactory;
            _cache = cache;
            //UpdateCache(); DOESNT WORK
        }

		public async Task<IEnumerable<Instrument>> InstrumentCache()
		{
            if (_cache.TryGetValue(BlahCacheKey, out IEnumerable<Instrument> instruments))
			{
				return instruments;
			}

			instruments = await Instrument.ToListAsync();

			_cache.Set(BlahCacheKey, instruments);

			return instruments;
            /*return await Instrument.ToListAsync();*/
		}
		public async Task<IEnumerable<Exchange>> ExchangeCache()
		{
			if (_cache.TryGetValue(BlahCacheKey2, out IEnumerable<Exchange> exchanges))
			{
				return exchanges;
			}

			exchanges = await Exchange.ToListAsync();

			_cache.Set(BlahCacheKey2, exchanges);

			return exchanges;
			/*return await Instrument.ToListAsync();*/
		}
        public void UpdateCache()
        {
			var logger = _loggerFactory.CreateLogger("LoggerCategory");
			logger.LogInformation("CACHED INSTRUMENTS CALL------");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Trader.Models.TradeImportModels.TradeImport> TradeImport { get; set; }

        public DbSet<Trader.Models.TradeImportModels.Instrument> Instrument { get; set; }

        public DbSet<Trader.Models.FileImportModels.Exchange> Exchange { get; set; }

        public DbSet<Trader.Models.FileImportModels.FileImport> FileImport { get; set; }
    }
}
