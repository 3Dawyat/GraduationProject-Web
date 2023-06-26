﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pharmacy_DomainModels.Models.DB_Models
{
    [Keyless]
    public partial class VwItemsWithUnits
    {
        public int ItemUnitId { get; set; }
        public string Name { get; set; }
        public string ActiveIngredient { get; set; }
        public string Company { get; set; }
        public string Pamphlet { get; set; }
        public string Dosage { get; set; }
        public string Composition { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? SalesPrice { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? PuchasePrice { get; set; }
        [StringLength(200)]
        public string Barcode { get; set; }
        public int? ItemId { get; set; }
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
        public string ImageName { get; set; }
    }
}