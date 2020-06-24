using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SpeccourierApiV2.Core.Entities;
using SpeccourierApiV2.Core.Services;

namespace ORPrep.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PackageDateController : ControllerBase
	{
		private readonly IPackageService _packageDateService;
		private readonly IMapper _mapper;

		public PackageDateController(
			IPackageService packageDateService,
			IMapper mapper)
		{
			_packageDateService = packageDateService;
			_mapper = mapper;
		}

		[HttpGet("{number}")]
		[ProducesResponseType(200, Type = typeof(PackageDate))]
		public async Task<IActionResult> GetPackageDate(string number)
		{

			var package = await _packageDateService.GetPackageByNumber(number);
			var packageDate = _mapper.Map<PackageDate>(package);

			return Ok(packageDate);
		}

		[HttpPost("{number}")]
		public async Task<IActionResult> UpdatePackageDate(PackageDate packageDate, string number)
		{
			await _packageDateService.UpdatePackage(packageDate, number);

			return Ok();
		}
	}
}
