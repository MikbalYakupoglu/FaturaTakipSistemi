using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.DataAccess.Abstract;
using FaturaTakip.DataAccess.Core;
using Microsoft.EntityFrameworkCore;

namespace FaturaTakip.DataAccess.Concrete
{
    public class EfMessageDal : EfEntityRepositoryBase<Message, InvoiceTrackContext>, IMessageDal
    {
        public async Task<IEnumerable<Message>> GetAllMessagesWithRelationsAsync()
        {
            using (var context = new InvoiceTrackContext())
            {
                var messages = await context.Messages
                        .Include(m => m.Landlord)
                        .Include(m => m.Tenant)
                        .ToListAsync();

                return messages;
            }
        }

        public async Task<IEnumerable<Message>> GetLandlordsMessagesAsync(int userId)
        {
            using (var context = new InvoiceTrackContext())
            {
                var messages = await context.Messages
                    .Include(m => m.Landlord)
                    .Where(m => m.FKLandlordId == userId)
                    .ToListAsync();

                return messages;
            }
        }

        public async Task<Message> GetMessageByIdWithRelationsAsync(int? messageId)
        {
            using (var context = new InvoiceTrackContext())
            {
                var message = await context.Messages
                    .Include(m=> m.Landlord)
                    .Include(m=> m.Tenant)
                    .SingleOrDefaultAsync(m=> m.Id == messageId);

                return message;
            }
        }

        public async Task<IEnumerable<Message>> GetTenantsMessagesAsync(int userId)
        {
            using (var context = new InvoiceTrackContext())
            {
                var messages = await context.Messages
                .Include(m => m.Tenant)
                .Where(m => m.FKTenantId == userId)
                .ToListAsync();

                return messages;
            }
        }
    }
}
