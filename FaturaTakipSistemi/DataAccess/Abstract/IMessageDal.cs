using FaturaTakip.Data.Models;
using FaturaTakip.DataAccess.Core;

namespace FaturaTakip.DataAccess.Abstract
{
    public interface IMessageDal : IEntityRepository<Message>
    {
        Task<Message> GetMessageByIdWithRelationsAsync(int? messageId);
        Task<IEnumerable<Message>> GetAllMessagesWithRelationsAsync();
        Task<IEnumerable<Message>> GetLandlordsMessagesAsync(int userId);
        Task<IEnumerable<Message>> GetTenantsMessagesAsync(int userId);

    }
}
