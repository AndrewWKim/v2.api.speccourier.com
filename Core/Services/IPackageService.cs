using SpeccourierApiV2.Core.Entities;
using SpeccourierApiV2.Models;
using System.Threading.Tasks;

namespace SpeccourierApiV2.Core.Services
{
	public interface IPackageService
	{
		Task<Package> GetPackageByNumber(string number);

		Task UpdatePackage(PackageDate packageDate, string number);
	}
}
