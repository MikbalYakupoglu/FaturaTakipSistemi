﻿using FaturaTakip.Data.Models;
using FaturaTakip.DataAccess.Core;

namespace FaturaTakip.DataAccess.Abstract
{
    public interface IPaymentDal : IEntityRepository<Payment>
    {
    }
}
