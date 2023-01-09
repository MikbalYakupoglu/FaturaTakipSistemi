using FaturaTakip.Core.DataAccess;
using FaturaTakip.Data.Models;

namespace FaturaTakip.DataAccess.Abstract;

public interface IMessageDal : IEntityRepository<Message>
{
    
}