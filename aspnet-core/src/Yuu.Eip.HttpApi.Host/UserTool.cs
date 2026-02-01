using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ModelContextProtocol.Server;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace Yuu.Eip;

[McpServerToolType]
[Authorize(IdentityPermissions.Users.Default)]
public class UserTool
{
    private readonly IIdentityUserAppService _identityUserAppService;
    private readonly ICurrentUser _currentUser;

    public UserTool(IIdentityUserAppService identityUserAppService,
        ICurrentUser currentUser
    )
    {
        _identityUserAppService = identityUserAppService;
        _currentUser = currentUser;
    }

    [McpServerTool, Description("取得所有使用者")]
    public async Task<PagedResultDto<IdentityUserDto>> GetAllUsers()
    {
        var items = await _identityUserAppService.GetListAsync(new GetIdentityUsersInput
        {
            SkipCount = 0,
            MaxResultCount = 10
        });

        return items;
    }
}
