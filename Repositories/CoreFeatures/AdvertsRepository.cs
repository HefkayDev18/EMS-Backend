using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Repositories.Interfaces.ICoreFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Repositories.CoreFeatures
{
    public class AdvertsRepository : IAdvertsRepository
    {
        private readonly ApplicationDbContext _context;

        public AdvertsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Advertisements> AddAdsAsync(Advertisements ads)
        {
            _context.Emp_Adverts.Add(ads);

            await _context.SaveChangesAsync();

            return ads;
        }

        public async Task<IEnumerable<Advertisements>> GetAdsByEmployeeIdAsync(int employeeId)
        {
            return await _context.Emp_Adverts
                .Include(a => a.EmployeeObj)
                .Where(a => a.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<Advertisements> GetAdsByAdsIdAsync(int adsId)
        {
            var adverts = await _context.Emp_Adverts.FindAsync(adsId);

            return adverts;
        }

        public bool AdsExists(int adsId)
        {
            return  _context.Emp_Adverts
                 .Any(e => e.AdvertisementId == adsId);
        }

        public async Task<Advertisements> UpdateAdsAsync(Advertisements advertisements)
        {
            _context.Entry(advertisements).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return advertisements;
        }


        public async Task<IEnumerable<Advertisements>> GetAdsAsync()
        {
            var ads = await _context.Emp_Adverts
                .Include(e => e.EmployeeObj)
                .Where(e => e.IsAdActive)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();

            return ads;
        }

        public async Task<bool> DeleteAdsAsync(int adsId)
        {
            var adverts = await _context.Emp_Adverts.FindAsync(adsId);

            if (adverts == null)
                return false;

            _context.Emp_Adverts.Remove(adverts);

            await _context.SaveChangesAsync();

            return true;
        }

    }
}
