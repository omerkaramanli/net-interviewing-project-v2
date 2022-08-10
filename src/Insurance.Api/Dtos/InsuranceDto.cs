﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Insurance.Api.Dtos
{
    public class InsuranceDto
    {
        [Required]
        [MinLength(1)]
        public int ProductId { get; set; }
        public float InsuranceValue { get; set; }
        [JsonIgnore]
        public string ProductTypeName { get; set; }
        [JsonIgnore]
        public bool ProductTypeHasInsurance { get; set; }
        [JsonIgnore]
        public float SalesPrice { get; set; }
        [JsonIgnore]
        public float SurchargeRate { get; set; }
    }
}
