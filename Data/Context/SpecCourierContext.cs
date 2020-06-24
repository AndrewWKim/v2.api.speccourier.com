using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SpeccourierApiV2.Core.Configurations;
using SpeccourierApiV2.Models;

namespace SpeccourierApiV2.Data.Context
{
	public class SpecCourierContext : DbContext, ISpecCourierContext
	{
        public SpecCourierContext()
        {
        }

        public SpecCourierContext(DbContextOptions<SpecCourierContext> options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public virtual DbSet<Package> Packages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(GetSpecCourierConfig().ConnectionStrings.SpecCourierDatabase);
        }

        private SpecCourierConfig GetSpecCourierConfig()
        {
            var config = new SpecCourierConfig();
            Configuration.GetSection("SpecCourier").Bind(config);

            return config;
        }
    }
}
