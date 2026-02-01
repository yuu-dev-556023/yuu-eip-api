using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Yuu.Eip.HumanResources.Departments;
using Yuu.Eip.Permissions;

namespace Yuu.Eip.HumanResources;

[Authorize(EipPermissions.HumanResources.Departments.Default)]
public class DepartmentAppService : EipAppService, IDepartmentAppService
{
    private readonly IRepository<Department, Guid> _departmentRepository;
    private readonly IRepository<Employee, Guid> _employeeRepository;
    private readonly HumanResourcesMapper _mapper;

    /// <summary>
    /// 初始化部門應用服務
    /// </summary>
    /// <param name="departmentRepository">部門倉儲</param>
    /// <param name="employeeRepository">員工倉儲</param>
    public DepartmentAppService(
        IRepository<Department, Guid> departmentRepository,
        IRepository<Employee, Guid> employeeRepository)
    {
        _departmentRepository = departmentRepository;
        _employeeRepository = employeeRepository;
        _mapper = new HumanResourcesMapper();
    }

    /// <summary>
    /// 獲取指定ID的部門
    /// </summary>
    /// <param name="id">部門ID</param>
    /// <returns>部門詳情</returns>
    public async Task<DepartmentDto> GetAsync(Guid id)
    {
        var department = await _departmentRepository.GetAsync(id);
        var dto = _mapper.DepartmentToDto(department);
        await FillDepartmentNamesAsync(new List<DepartmentDto> { dto });
        return dto;
    }

    /// <summary>
    /// 獲取部門列表
    /// </summary>
    /// <param name="input">查詢參數，包含過濾條件和分頁參數</param>
    /// <returns>分頁的部門列表</returns>
    public async Task<PagedResultDto<DepartmentDto>> GetListAsync(GetDepartmentsInput input)
    {
        var query = await _departmentRepository.GetQueryableAsync();

        if (!string.IsNullOrWhiteSpace(input.Filter))
        {
            query = query.Where(x =>
                x.Code.Contains(input.Filter) ||
                x.Name.Contains(input.Filter));
        }

        if (input.ParentId.HasValue)
        {
            query = query.Where(x => x.ParentId == input.ParentId);
        }

        if (input.IsActive.HasValue)
        {
            query = query.Where(x => x.IsActive == input.IsActive);
        }

        var totalCount = query.Count();

        if (!string.IsNullOrWhiteSpace(input.Sorting))
        {
            query = query.OrderBy(x => x.Order).ThenBy(x => x.Code);
        }

        var departments = query
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToList();

        var dtos = departments.Select(_mapper.DepartmentToDto).ToList();
        await FillDepartmentNamesAsync(dtos);

        return new PagedResultDto<DepartmentDto>(totalCount, dtos);
    }

    /// <summary>
    /// 創建新部門
    /// </summary>
    /// <param name="input">部門創建參數，包含編碼、名稱、父部門ID等信息</param>
    /// <returns>創建的部門詳情</returns>
    [Authorize(EipPermissions.HumanResources.Departments.Create)]
    public async Task<DepartmentDto> CreateAsync(CreateUpdateDepartmentDto input)
    {
        var department = new Department(
            GuidGenerator.Create(),
            input.Code,
            input.Name,
            input.ParentId,
            input.ManagerId,
            input.Order,
            input.IsActive,
            CurrentTenant.Id);

        await _departmentRepository.InsertAsync(department);

        var dto = _mapper.DepartmentToDto(department);
        await FillDepartmentNamesAsync(new List<DepartmentDto> { dto });
        return dto;
    }

    /// <summary>
    /// 更新部門信息
    /// </summary>
    /// <param name="id">部門ID</param>
    /// <param name="input">部門更新參數</param>
    /// <returns>更新後的部門詳情</returns>
    [Authorize(EipPermissions.HumanResources.Departments.Update)]
    public async Task<DepartmentDto> UpdateAsync(Guid id, CreateUpdateDepartmentDto input)
    {
        var department = await _departmentRepository.GetAsync(id);

        department.Code = input.Code;
        department.Name = input.Name;
        department.ParentId = input.ParentId;
        department.ManagerId = input.ManagerId;
        department.Order = input.Order;
        department.IsActive = input.IsActive;

        await _departmentRepository.UpdateAsync(department);

        var dto = _mapper.DepartmentToDto(department);
        await FillDepartmentNamesAsync(new List<DepartmentDto> { dto });
        return dto;
    }

    /// <summary>
    /// 刪除部門
    /// </summary>
    /// <param name="id">部門ID</param>
    [Authorize(EipPermissions.HumanResources.Departments.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _departmentRepository.DeleteAsync(id);
    }

    /// <summary>
    /// 獲取所有啟用的部門
    /// </summary>
    /// <returns>所有啟用部門的列表</returns>
    public async Task<List<DepartmentDto>> GetAllAsync()
    {
        var departments = await _departmentRepository.GetListAsync(x => x.IsActive);
        var dtos = departments
            .OrderBy(x => x.Order)
            .ThenBy(x => x.Code)
            .Select(_mapper.DepartmentToDto)
            .ToList();

        await FillDepartmentNamesAsync(dtos);
        return dtos;
    }

    /// <summary>
    /// 獲取部門樹狀結構
    /// </summary>
    /// <returns>部門樹狀結構列表</returns>
    public async Task<List<DepartmentDto>> GetTreeAsync()
    {
        var all = await GetAllAsync();
        return BuildTree(all, null);
    }

    /// <summary>
    /// 構建部門樹狀結構
    /// </summary>
    /// <param name="all">所有部門列表</param>
    /// <param name="parentId">父部門ID</param>
    /// <returns>部門樹狀結構</returns>
    private List<DepartmentDto> BuildTree(List<DepartmentDto> all, Guid? parentId)
    {
        return all
            .Where(x => x.ParentId == parentId)
            .OrderBy(x => x.Order)
            .ToList();
    }

    /// <summary>
    /// 填充部門相關的名稱信息（父部門名稱和主管名稱）
    /// </summary>
    /// <param name="dtos">部門DTO列表</param>
    private async Task FillDepartmentNamesAsync(List<DepartmentDto> dtos)
    {
        var parentIds = dtos.Where(x => x.ParentId.HasValue).Select(x => x.ParentId!.Value).Distinct().ToList();
        var managerIds = dtos.Where(x => x.ManagerId.HasValue).Select(x => x.ManagerId!.Value).Distinct().ToList();

        if (parentIds.Any())
        {
            var parents = await _departmentRepository.GetListAsync(x => parentIds.Contains(x.Id));
            foreach (var dto in dtos.Where(x => x.ParentId.HasValue))
            {
                dto.ParentName = parents.FirstOrDefault(x => x.Id == dto.ParentId)?.Name;
            }
        }

        if (managerIds.Any())
        {
            var managers = await _employeeRepository.GetListAsync(x => managerIds.Contains(x.Id));
            foreach (var dto in dtos.Where(x => x.ManagerId.HasValue))
            {
                dto.ManagerName = managers.FirstOrDefault(x => x.Id == dto.ManagerId)?.Name;
            }
        }
    }
}
