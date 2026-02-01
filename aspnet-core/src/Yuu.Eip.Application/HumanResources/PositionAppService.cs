using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Yuu.Eip.HumanResources.Positions;
using Yuu.Eip.Permissions;

namespace Yuu.Eip.HumanResources;

[Authorize(EipPermissions.HumanResources.Positions.Default)]
public class PositionAppService : EipAppService, IPositionAppService
{
    private readonly IRepository<Position, Guid> _positionRepository;
    private readonly HumanResourcesMapper _mapper;

    public PositionAppService(IRepository<Position, Guid> positionRepository)
    {
        _positionRepository = positionRepository;
        _mapper = new HumanResourcesMapper();
    }

    public async Task<PositionDto> GetAsync(Guid id)
    {
        var position = await _positionRepository.GetAsync(id);
        return _mapper.PositionToDto(position);
    }

    public async Task<PagedResultDto<PositionDto>> GetListAsync(GetPositionsInput input)
    {
        var query = await _positionRepository.GetQueryableAsync();

        if (!string.IsNullOrWhiteSpace(input.Filter))
        {
            query = query.Where(x =>
                x.Code.Contains(input.Filter) ||
                x.Name.Contains(input.Filter));
        }

        if (input.Level.HasValue)
        {
            query = query.Where(x => x.Level == input.Level);
        }

        if (input.IsActive.HasValue)
        {
            query = query.Where(x => x.IsActive == input.IsActive);
        }

        var totalCount = query.Count();

        query = query.OrderBy(x => x.Level).ThenBy(x => x.Code);

        var positions = query
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToList();

        var dtos = positions.Select(_mapper.PositionToDto).ToList();

        return new PagedResultDto<PositionDto>(totalCount, dtos);
    }

    [Authorize(EipPermissions.HumanResources.Positions.Create)]
    public async Task<PositionDto> CreateAsync(CreateUpdatePositionDto input)
    {
        var position = new Position(
            GuidGenerator.Create(),
            input.Code,
            input.Name,
            input.Level,
            input.BaseSalary,
            input.IsActive,
            CurrentTenant.Id);

        await _positionRepository.InsertAsync(position);

        return _mapper.PositionToDto(position);
    }

    [Authorize(EipPermissions.HumanResources.Positions.Update)]
    public async Task<PositionDto> UpdateAsync(Guid id, CreateUpdatePositionDto input)
    {
        var position = await _positionRepository.GetAsync(id);

        position.Code = input.Code;
        position.Name = input.Name;
        position.Level = input.Level;
        position.BaseSalary = input.BaseSalary;
        position.IsActive = input.IsActive;

        await _positionRepository.UpdateAsync(position);

        return _mapper.PositionToDto(position);
    }

    [Authorize(EipPermissions.HumanResources.Positions.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _positionRepository.DeleteAsync(id);
    }

    public async Task<List<PositionDto>> GetAllAsync()
    {
        var positions = await _positionRepository.GetListAsync(x => x.IsActive);
        return positions
            .OrderBy(x => x.Level)
            .ThenBy(x => x.Code)
            .Select(_mapper.PositionToDto)
            .ToList();
    }
}
