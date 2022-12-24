using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FaturaTakip.Data.Models.Abstract;
using Microsoft.AspNetCore.Identity;


// Add profile data for application users by adding properties to the FaturaTakipUser class
public class InvoiceTrackUser : IdentityUser
{
    [StringLength(50)]    public string Name { get; set; }

    [StringLength(50)]    public string LastName { get; set; }

    [StringLength(11)]    public string GovermentId { get; set; }

    [Range(1900, 2022)]   public int YearOfBirth { get; set; }

    [StringLength(10)]    public string Phone { get; set; }
}

