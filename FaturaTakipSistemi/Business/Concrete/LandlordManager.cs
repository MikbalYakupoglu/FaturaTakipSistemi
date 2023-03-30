using FaturaTakip.Business.Interface;
using FaturaTakip.Data;
using FaturaTakip.Data.Models;
using FaturaTakip.Utils.Results;
using Microsoft.EntityFrameworkCore;

namespace FaturaTakip.Business.Concrete
{
    public class LandlordManager : ILandlordService
    {
        private readonly InvoiceTrackContext _context;
        public LandlordManager(InvoiceTrackContext context)
        {
            _context = context;
        }

        public async Task<Result> DeleteLandlordAsync(int landlordId)
        {
            var landlordToDelete = await _context.Landlords.FirstOrDefaultAsync(l => l.Id == landlordId);
            if (!IsLandlordExistAsync(landlordToDelete.Id).Result)
                return new ErrorResult("Kiracı Bulunamadı.");

            if (IsLandlordRegisteredInHouseAsync(landlordToDelete.Id).Result)
                return new ErrorResult("Kiracı Evde Oturuyor.");

            _context.Landlords.Remove(landlordToDelete);

            return new SuccessResult("Kiracı Silindi.");
        }

        public async Task<DataResult<Landlord>> GetLandlordByUserIdAsync(string userId)
        {
            var landlord = await _context.Landlords.FirstOrDefaultAsync(l => l.FK_UserId == userId);
            if (landlord == null)
                return new ErrorDataResult<Landlord>("Ev Sahibi Bulunamadı");

            return new SuccessDataResult<Landlord>(landlord);
        }

        public async Task<bool> IsLandlordExistAsync(int landlordId)
        {
            return await _context.Landlords.AnyAsync(l => l.Id == landlordId);
        }

        public async Task<bool> IsLandlordExistAsync(string userId)
        {
            return await _context.Landlords.AnyAsync(l => l.FK_UserId == userId);
        }

        public async Task<bool> IsLandlordRegisteredInHouseAsync(int landlordId)
        {
            return await _context.Apartments.AnyAsync(ra => ra.FKLandlordId == landlordId);
        }

        public async Task<bool> IsLandlordRegisteredInHouseAsync(string userId)
        {
            var landlord = await GetLandlordByUserIdAsync(userId);
            return await _context.Apartments.AnyAsync(ra => ra.FKLandlordId == landlord.Data.Id);
        }
    }
}
