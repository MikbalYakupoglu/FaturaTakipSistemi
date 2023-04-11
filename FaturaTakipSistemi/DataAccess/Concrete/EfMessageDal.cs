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
                        .Include(m => m.Sender)
                        .ToListAsync();

                return messages;
            }
        }

        public async Task<IEnumerable<Message>> GetLandlordsMessagesAsync(int landlordId)
        {
            using (var context = new InvoiceTrackContext())
            {
                var messages = await context.Messages
                    .Include(m => m.Landlord)
                    .Include(m => m.Tenant)
                    .Include(m => m.Sender)
                    .Where(m => m.FKLandlordId == landlordId)
                    .ToListAsync();

                return messages;
            }
        }

        public async Task<IEnumerable<Message>> GetTenantsMessagesAsync(int tenantId)
        {
            using (var context = new InvoiceTrackContext())
            {
                var messages = await context.Messages
                .Include(m => m.Tenant)
                .Include(m => m.Landlord)
                .Include(m => m.Sender)
                .Where(m => m.FKTenantId == tenantId)
                .ToListAsync();

                return messages;
            }
        }

        public async Task<Message> GetMessageByIdWithRelationsAsync(int? messageId)
        {
            using (var context = new InvoiceTrackContext())
            {
                var message = await context.Messages
                    .Include(m => m.Landlord)
                    .Include(m => m.Tenant)
                    .Include(m => m.Sender)
                    .SingleOrDefaultAsync(m => m.Id == messageId);

                return message;
            }
        }

    }
}
