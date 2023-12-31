﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pharmacy_DomainModels.Models.DB_Models
{
    public partial class TbCategories
    {
        public TbCategories()
        {
            TbItems = new HashSet<TbItems>();
            TbPrinterSettings = new HashSet<TbPrinterSettings>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [InverseProperty("Category")]
        public virtual ICollection<TbItems> TbItems { get; set; }
        [InverseProperty("Category")]
        public virtual ICollection<TbPrinterSettings> TbPrinterSettings { get; set; }
    }
}