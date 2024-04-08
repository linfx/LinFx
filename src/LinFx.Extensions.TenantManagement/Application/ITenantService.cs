using LinFx.Application.Dtos;

namespace LinFx.Extensions.TenantManagement;

/// <summary>
/// 租户服务
/// </summary>
public interface ITenantService
{
    ValueTask<PagedResult<TenantDto>> GetListAsync(TenantRequest input);

    ValueTask<TenantDto> GetAsync(string id);

    ValueTask<TenantDto> CreateAsync(TenantEditInput input);

    ValueTask<TenantDto> UpdateAsync(string id, TenantEditInput input);

    ValueTask DeleteAsync(string id);
}
