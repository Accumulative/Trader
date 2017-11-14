using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using TraderData.Models;
using TraderData.Models.TradeImportModels;
using TraderData.Models.FileImportModels;
using TraderData.Models.AdminModels;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace TraderData
{
    //Use to create ApplicationDbContext for first time, make sure to match the parameters(just options)

    //class ApplicationDbContextFactory : IDbContextFactory<ApplicationDbContext>
    //{
    //    public ApplicationDbContext Create(DbContextFactoryOptions options)
    //    {
    //        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
    //        optionsBuilder.UseSqlServer("", b => b.MigrationsAssembly("TraderData"));
    //        return new ApplicationDbContext(optionsBuilder.Options);
    //    }
    //}

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        ILoggerFactory _loggerFactory;
        private const string BlahCacheKey = "blah-cache-key";
        private const string BlahCacheKey2 = "blah-cache-key2";
        private const string BlahCacheKey3 = "blah-cache-key4";
        private readonly IMemoryCache _cache;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILoggerFactory loggerFactory, IMemoryCache cache)
            : base(options)
        {
            _loggerFactory = loggerFactory;
            _cache = cache;
        }

		

		public async Task<IEnumerable<Instrument>> InstrumentCache()
		{
            if (_cache.TryGetValue(BlahCacheKey, out IEnumerable<Instrument> instruments))
			{
				return instruments;
			}

            return await UpdateInstrumentCache();
		}
		public async Task<IEnumerable<Exchange>> ExchangeCache()
		{
			if (_cache.TryGetValue(BlahCacheKey2, out IEnumerable<Exchange> exchanges))
			{
				return exchanges;
			}

            return await UpdateExchangeCache();
		}
		public async Task<Settings> SettingsCache()
		{
			if (_cache.TryGetValue(BlahCacheKey3, out Settings settings))
			{
				return settings;
			}

			return await UpdateSettingsCache();
		}
		public async Task<Settings> UpdateSettingsCache()
		{
			var settings = new Settings
			{
				RefreshTime = 5 * 60 //await Exchange.ToListAsync();
			};

			_cache.Set(BlahCacheKey3, settings);
            return settings;
		}
        public async Task<IEnumerable<Exchange>> UpdateExchangeCache()
        {
            var exchanges = await Exchange.ToListAsync();
            _cache.Set(BlahCacheKey2, exchanges);
            return exchanges;
        }

        public async Task<IEnumerable<Instrument>> UpdateInstrumentCache()
        {
            var instruments = await Instrument.ToListAsync();
            _cache.Set(BlahCacheKey, instruments);
            return instruments;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<TraderData.Models.TradeImportModels.TradeImport> TradeImport { get; set; }

        public DbSet<TraderData.Models.TradeImportModels.Instrument> Instrument { get; set; }

        public DbSet<TraderData.Models.FileImportModels.Exchange> Exchange { get; set; }

        public DbSet<TraderData.Models.FileImportModels.FileImport> FileImport { get; set; }

        public DbSet<TraderData.Models.NewsModels.NewsItem> NewsItem { get; set; }

        public DbSet<TraderData.Models.ContactModels.Message> Message { get; set; }

        public DbSet<TraderData.Models.ContactModels.Enquiry> Enquiry { get; set; }

        public DbSet<TraderData.Models.AdminModels.Settings> Settings { get; set; }
    }
}
