using FaturaTakip.Data.Models;
using FaturaTakip.Data.Models.Abstract;
using FaturaTakip.Utils.Results;
using FaturaTakip.ViewModels;

namespace FaturaTakip.Business.Interface
{
    public interface IMessageService
    {
        Task<DataResult<IEnumerable<MessageVM>>> GetAllMessagesAsync();
        Task<DataResult<IEnumerable<MessageVM>>> GetMessagesByUserAsync(User user);
        bool IsAnyMessageExist();
        Task<DataResult<Message>> GetMessageByIdAsync(int? messageId);
        Task<Result> AddAsync(Message messageToAdd);
        Task<Result> DeleteAsync(int messageId);
        Task<Result> UpdateAsync(Message message);

    }
}
