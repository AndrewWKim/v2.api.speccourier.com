using Microsoft.EntityFrameworkCore;
using SpeccourierApiV2.Core.Entities;
using SpeccourierApiV2.Core.Services;
using SpeccourierApiV2.Data.Context;
using SpeccourierApiV2.Models;
using System.Threading.Tasks;

namespace SpeccourierApiV2.Services
{
	public class PackageService : IPackageService
	{
		private readonly ISpecCourierContext _specCourierContext;

		public PackageService(
			ISpecCourierContext specCourierContext)
		{
			_specCourierContext = specCourierContext;
		}

		public async Task<Package> GetPackageByNumber(string number)
		{
			var package = await _specCourierContext.Packages.FirstOrDefaultAsync(p => p.Number == number);

			return package;
		}

		public async Task UpdatePackage(PackageDate packageDate, string number)
		{
			var package = await _specCourierContext.Packages.FirstOrDefaultAsync(p => p.Number == number);
			package.DeliveryDate = packageDate.DeliveryDate ?? package.DeliveryDate;
			package.DeliveryTimeFrom = packageDate.DeliveryTimeFrom ?? package.DeliveryTimeFrom;
			package.DeliveryTimeTo = packageDate.DeliveryTimeTo ?? package.DeliveryTimeTo;

			_specCourierContext.Packages.Update(package);
			await _specCourierContext.SaveChangesAsync();
		}
	}
}
