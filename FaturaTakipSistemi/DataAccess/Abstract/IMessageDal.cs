using FaturaTakip.Data.Models;
using FaturaTakip.DataAccess.Core;

namespace FaturaTakip.DataAccess.Abstract
{
    public interface IMessageDal : IEntityRepository<Message>
    {
        Task<Message> GetMessageByIdWithRelationsAsync(int? messageId);
        Task<IEnumerable<Message>> GetAllMessagesWithRelationsAsync();
        Task<IEnumerable<Message>> GetLandlordsMessagesAsync(int landlordId);
        Task<IEnumerable<Message>> GetTenantsMessagesAsync(int tenantId);

    }
}
