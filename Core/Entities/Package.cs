using System;
using SpeccourierApiV2.Models.Base;

namespace SpeccourierApiV2.Models
{

    public class Package : BaseModel
    {
        public string Number { get; set; }
        
        public int TownId { get; set; }

        public int Status { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public int? DeliveryTimeFrom { get; set; }

        public int? DeliveryTimeTo { get; set; }
    }
}
