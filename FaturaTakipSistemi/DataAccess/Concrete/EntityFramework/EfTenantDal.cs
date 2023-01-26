﻿using FaturaTakip.Core.DataAccess;
using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.DataAccess.Abstract;

namespace FaturaTakip.DataAccess.Concrete.EntityFramework;

public class EfTenantDal : EfEntityRepositoryBase<Tenant>, ITenantDal
{
    public EfTenantDal(InvoiceTrackContext context) : base(context)
    {
    }
}