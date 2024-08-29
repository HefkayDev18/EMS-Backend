using EmployeeManagementSystem.Models.Entities;

namespace EmployeeManagementSystem.Repositories.Interfaces.ICoreFeatures
{
    public interface IAdvertsRepository
    {
        Task<Advertisements> AddAdsAsync(Advertisements ads);
        Task<IEnumerable<Advertisements>> GetAdsByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<Advertisements>> GetAdsAsync();
        Task<Advertisements> GetAdsByAdsIdAsync(int adsId);
        Task<bool> DeleteAdsAsync(int adsId);
        bool AdsExists(int adsId);
        Task<Advertisements> UpdateAdsAsync(Advertisements advertisements);
    }
}
