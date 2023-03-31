
using FaturaTakip.Data.Models.Abstract;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaturaTakip.Data.Models
{
    public enum Block
    {
        A = 1,
        B = 2,
        C = 3,
        D = 4
    }

    public enum Type
    {
        [Display(Name = "None")]
        None,
        [Display(Name = "1+0")]
        OnePlusZero,
        [Display(Name = "1+1")]
        OnePlusOne,
        [Display(Name = "2+1")]
        TwoPlusOne,
        [Display(Name = "3+1")]
        ThreePlusOne,
    }
    public class Apartment : IEntity
    {
        public int Id { get; set; }
        public short Floor { get; set; }
        public int DoorNumber { get; set; }
        public Type Type { get; set; }
        public Block Block { get; set; }
        public int RentPrice { get; set; }
        public bool Rented { get; set; }

        public int FKLandlordId { get; set; }

        [ValidateNever]
        [ForeignKey(nameof(FKLandlordId))]
        public Landlord Landlord { get; set; }

    }
}
