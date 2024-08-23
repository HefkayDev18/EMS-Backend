using EmployeeManagementSystem.Models.Entities;
using System.Linq.Dynamic.Core;
using static EmployeeManagementSystem.Repositories.CoreFeatures.EmpAppraisalRepository;

namespace EmployeeManagementSystem.Repositories.Interfaces.ICoreFeatures
{
    public interface IEmpAppraisalRepository
    {
        //Task<IEnumerable<EmpAppraisal>> GetAppraisalByUserIdAsync(int userId);
        Task<EmpAppraisal> GetAppraisalByEmpIdAsync(int id);
        Task<IEnumerable<EmpAppraisal>> GetAppraisalsByEmployeeIdAsync(int employeeId);
        Task<EmpAppraisal> GetAppraisalByAppIdAsync(int appraisalId);
        Task<EmpAppraisal> AddAppraisalAsync(EmpAppraisal appraisal);
        Task<EmpAppraisal> UpdateAppraisalAsync(EmpAppraisal appraisal);
        Task<EmpAppraisal> GetRecentAppraisalAsync(int employeeId, DateTime cutoffDate);
        Task<PagedResultT<EmpAppraisal>> GetAppraisalsByStatusAsync(string status, int page, int pageSize);
    }


    //#region appraisalComment
    //public interface IEmpAppraisalCommentRepository
    //{
    //    Task<IEnumerable<EmpAppraisalComment>> GetCommentByAppraisalIdAsync(int appraisalId);
    //}
    //#endregion

}
