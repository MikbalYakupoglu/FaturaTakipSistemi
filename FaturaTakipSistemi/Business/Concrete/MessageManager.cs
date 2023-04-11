using AutoMapper;
using FaturaTakip.Business.Interface;
using FaturaTakip.Data.Models;
using FaturaTakip.Data.Models.Abstract;
using FaturaTakip.DataAccess.Abstract;
using FaturaTakip.Utils;
using FaturaTakip.Utils.Results;
using FaturaTakip.ViewModels;

namespace FaturaTakip.Business.Concrete
{
    public class MessageManager : IMessageService
    {
        private readonly IMessageDal _messageDal;
        private readonly IMapper _mapper;

        public MessageManager(IMessageDal messageDal,
            IMapper mapper)
        {
            _messageDal = messageDal;
            _mapper = mapper;
        }

        public async Task<DataResult<IEnumerable<MessageVM>>> GetAllMessagesAsync()
        {
            var messages = await _messageDal.GetAllMessagesWithRelationsAsync();

            if (!messages.Any())
                return new ErrorDataResult<IEnumerable<MessageVM>>(Enumerable.Empty<MessageVM>(), "Mesaj Bulunamadı.");

            return new SuccessDataResult<IEnumerable<MessageVM>>(_mapper.Map<IEnumerable<MessageVM>>(messages));
        }

        public async Task<DataResult<IEnumerable<MessageVM>>> GetMessagesByUserAsync(User user)
        {
            if (user.GetType() == typeof(Landlord))
            {
                var messages = await GetMessagesByLandlordIdAsync(user.Id);
                return new SuccessDataResult<IEnumerable<MessageVM>>(_mapper.Map<IEnumerable<MessageVM>>(messages.Data));
            }

            if (user.GetType() == typeof(Tenant))
            {
                var messages = await GetMessagesByTenantIdAsync(user.Id);
                return new SuccessDataResult<IEnumerable<MessageVM>>(_mapper.Map<IEnumerable<MessageVM>>(messages.Data));
            }


            return new ErrorDataResult<IEnumerable<MessageVM>>(Enumerable.Empty<MessageVM>(), "Girilen Kullanıcıya Ait Mesaj Bulunamadı.");
        }

        public async Task<DataResult<Message>> GetMessageByIdAsync(int? messageId)
        {
            var message = await _messageDal.GetMessageByIdWithRelationsAsync(messageId);
            if (message == null)
                return new ErrorDataResult<Message>("Mesaj Bulunamadı");

            return new SuccessDataResult<Message>(message);
        }
        public bool IsAnyMessageExist()
        {
            return _messageDal.IsAnyExist();
        }


        public async Task<Result> AddAsync(Message messageToAdd)
        {
            var message = await _messageDal.GetAsync(m => m.Id == messageToAdd.Id);
            if (message != null)
                return new ErrorResult("Mesaj Zaten Bulunuyor.");

            await _messageDal.AddAsync(messageToAdd);
            return new SuccessResult(Messages.AddSuccess);
        }
        public async Task<Result> DeleteAsync(int messageId)
        {
            var messageToDelete = await _messageDal.GetAsync(m => m.Id == messageId);
            if (messageToDelete == null)
                return new ErrorResult("Mesaj Bulunamadı.");

            await _messageDal.RemoveAsync(messageToDelete);
            return new SuccessResult(Messages.RemoveSuccess);
        }

        public async Task<Result> UpdateAsync(Message message)
        {
            var messageToUpdate = await _messageDal.GetAsync(m => m.Id == message.Id);
            if (messageToUpdate == null)
                return new ErrorResult("Mesaj Bulunamadı.");

            messageToUpdate.Title = message.Title;
            messageToUpdate.Body = message.Body;
            messageToUpdate.IsVisible = message.IsVisible;

            await _messageDal.UpdateAsync(messageToUpdate);
            return new SuccessResult(Messages.UpdateSuccess);
        }


        private async Task<DataResult<IEnumerable<Message>>> GetMessagesByLandlordIdAsync(int landlordId)
        {
            var messages = await _messageDal.GetLandlordsMessagesAsync(landlordId);

            if (!messages.Any())
                return new ErrorDataResult<IEnumerable<Message>>(Enumerable.Empty<Message>(), "Mesaj Bulunamadı.");

            return new SuccessDataResult<IEnumerable<Message>>(messages);
        }

        private async Task<DataResult<IEnumerable<Message>>> GetMessagesByTenantIdAsync(int tenantId)
        {
            var messages = await _messageDal.GetTenantsMessagesAsync(tenantId);

            if (!messages.Any())
                return new ErrorDataResult<IEnumerable<Message>>(Enumerable.Empty<Message>(), "Mesaj Bulunamadı.");

            return new SuccessDataResult<IEnumerable<Message>>(messages);
        }
    }
}
