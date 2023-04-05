using FaturaTakip.Data.Models;
using FaturaTakip.Data.Models.Abstract;
using FaturaTakip.Utils.Results;

namespace FaturaTakip.Business.Interface
{
    public interface IMessageService
    {
        Task<DataResult<IEnumerable<Message>>> GetAllMessagesAsync();
        Task<DataResult<IEnumerable<Message>>> GetMessagesByTenantIdAsync(int tenantId);
        Task<DataResult<IEnumerable<Message>>> GetMessagesByLandlordIdAsync(int landlordId);
        Task<DataResult<IEnumerable<Message>>> GetLoginedUsersMessagesAsync(User user);
        bool IsAnyMessageExist();
        Task<DataResult<Message>> GetMessageByIdAsync(int? messageId);
        Task<Result> AddAsync(Message messageToAdd);
        Task<Result> DeleteAsync(int messageId);
        Task<Result> UpdateAsync(Message message);

    }
}
