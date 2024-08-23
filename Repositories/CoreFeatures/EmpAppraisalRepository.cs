using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Repositories.Interfaces.ICoreFeatures;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace EmployeeManagementSystem.Repositories.CoreFeatures
{
    public class EmpAppraisalRepository : IEmpAppraisalRepository
    {

        private ApplicationDbContext _context;

        public EmpAppraisalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //public async Task<IEnumerable<EmpAppraisal>> GetAppraisalByUserIdAsync(int employeeId)
        //{
        //    return await _context.Emp_Appraisals
        //        .Where(a => a.EmployeeId == employeeId)
        //    .ToListAsync();
        //}

        public async Task<EmpAppraisal> GetAppraisalByEmpIdAsync(int id)
        {
            var appraisal = await _context.Emp_Appraisals
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a => a.EmployeeId == id);

            return appraisal;
        }

        public async Task<IEnumerable<EmpAppraisal>> GetAppraisalsByEmployeeIdAsync(int employeeId)
        {
            return await _context.Emp_Appraisals
                .Where(a => a.EmployeeId == employeeId)
                .ToListAsync();
        }


        public async Task<EmpAppraisal> GetAppraisalByAppIdAsync(int appraisalId)
        {
            var appraisal = await _context.Emp_Appraisals
                .FirstOrDefaultAsync(a => a.AppraisalId == appraisalId);

            return appraisal;
        }

        public async Task<EmpAppraisal> AddAppraisalAsync(EmpAppraisal appraisal)
        {
            _context.Emp_Appraisals.Add(appraisal);

            await _context.SaveChangesAsync();

            return appraisal;
        }

        public async Task<EmpAppraisal> UpdateAppraisalAsync(EmpAppraisal appraisal)
        {
            _context.Entry(appraisal).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return appraisal;
        }

        public async Task<EmpAppraisal> GetRecentAppraisalAsync(int employeeId, DateTime cutoffDate)
        {
            var recentAppraisal = await _context.Emp_Appraisals
                .Where(a => a.EmployeeId == employeeId && a.AppDateSubmitted >= cutoffDate)
                .OrderByDescending(a => a.AppDateSubmitted)
                .FirstOrDefaultAsync();

            return recentAppraisal;
        }

        public async Task<PagedResultT<EmpAppraisal>> GetAppraisalsByStatusAsync(string status, int page, int pageSize)
        {
            IQueryable<EmpAppraisal> query = _context.Emp_Appraisals.AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(a => a.Status == status);
            }

            var totalCount = await query.CountAsync();

            var appraisals = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(e => e.AppDateCreated)
                .ToListAsync();

            return new PagedResultT<EmpAppraisal>
            {
                Items = appraisals,
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = page
            };
        }


        #region PagedResult
        public class PagedResultT<T>
        {
            public IList<T> Items { get; set; }
            public int TotalCount { get; set; }
            public int PageSize { get; set; }
            public int CurrentPage { get; set; }
        }
        #endregion


    }


    //#region appraisalComment
    //public class EmpAppraisalCommentRepository : IEmpAppraisalCommentRepository
    //{
    //    private ApplicationDbContext _context;

    //    public EmpAppraisalCommentRepository(ApplicationDbContext context)
    //    {
    //        _context = context;
    //    }

    //    public async Task<IEnumerable<EmpAppraisalComment>> GetCommentByAppraisalIdAsync(int appraisalId)
    //    {
    //        return await _context.Emp_AppraisalsComment
    //            .Where(c => c.AppraisalId == appraisalId)
    //            .ToListAsync();
    //    }
    //}
    //#endregion


}
