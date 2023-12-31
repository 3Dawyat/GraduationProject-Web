﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pharmacy_DomainModels.Models.DB_Models
{
    public partial class TbSalesInvoices
    {
        public TbSalesInvoices()
        {
            TbSalesInvoiceItems = new HashSet<TbSalesInvoiceItems>();
        }

        [Key]
        public int Id { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? InvoiceDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DelivryDate { get; set; }
        public int? SafeId { get; set; }
        public int? DelivryManId { get; set; }
        public int? CustomerId { get; set; }
        [StringLength(450)]
        public string UserId { get; set; }
        public int? SheftId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Discount { get; set; }
        public string Notes { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? RealeDate { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Cash { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Total { get; set; }
        public int? StoreId { get; set; }
        [StringLength(50)]
        public string BloodPressure { get; set; }
        public int Diabites { get; set; }
        public int BodyTempreature { get; set; }
        [Column(TypeName = "decimal(20, 2)")]
        public decimal? Later { get; set; }
        [StringLength(50)]
        public string InvoiceType { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? NextVisit { get; set; }
        public string Diagnosis { get; set; }

        [InverseProperty("Invoice")]
        public virtual ICollection<TbSalesInvoiceItems> TbSalesInvoiceItems { get; set; }
    }
}