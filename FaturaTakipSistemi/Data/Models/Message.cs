﻿using FaturaTakip.Data.Models.Abstract;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaturaTakip.Data.Models
{
    public class Message : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int? FKTenantId { get; set; }
        public int? FKLandlordId { get; set; }
        public int? FkApartmentId { get; set; }
        public string? FKUserId { get; set; }
        public bool IsVisible { get; set; }


        [ValidateNever]
        [ForeignKey(nameof(FKTenantId))]
        public Tenant Tenant { get; set; }

        [ValidateNever]
        [ForeignKey(nameof(FKLandlordId))]
        public Landlord Landlord { get; set; }

        [ValidateNever]
        [ForeignKey(nameof(FkApartmentId))]
        public Apartment Apartment { get; set; }

        [ValidateNever]
        [ForeignKey(nameof(FKUserId))]
        public InvoiceTrackUser? User  { get; set; }
    }
}
