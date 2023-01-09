﻿using System.ComponentModel.DataAnnotations;
using FaturaTakip.Core.DataAccess;

namespace FaturaTakip.Data.Models.Abstract;

public abstract class User : IEntity
{
    public int Id { get; set; }
    [StringLength(50)]
    public string Name { get; set; }

    [StringLength(50)]
    public string LastName { get; set; }

    [StringLength(11)]
    public string GovermentId { get; set; }

    public int YearOfBirth { get; set; }

    [StringLength(100)]
    public string Email { get; set; }

    [StringLength(10)]
    public string Phone { get; set; }

}