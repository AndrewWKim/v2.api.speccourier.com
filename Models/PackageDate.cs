using SpeccourierApiV2.Models.Base;
using System;

namespace SpeccourierApiV2.Core.Entities
{
	public class PackageDate: BaseModel
	{
		public DateTime? DeliveryDate { get; set; }

		public int? DeliveryTimeFrom { get; set; }

		public int? DeliveryTimeTo { get; set; }
	}
}
