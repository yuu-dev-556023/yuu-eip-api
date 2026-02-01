using System.Linq;
using System.Net;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Yuu.Eip;

public class IPWhitelistMiddleware
{
    private readonly RequestDelegate _next;

    public IPWhitelistMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        /*
         *  1. 取得用戶的 IP 地址 如果為本機則通過
         *  2. 驗證 用戶有沒有登入
         *  3. 如果沒有 登入，則返回 401 Unauthorized
         *  4. 如果不在白名單內，則返回 403 Forbidden
         *  5. 如果在白名單內，則繼續處理請求
         */

        /* 1. 取得用戶的 IP 地址 如果為本機則通過 */
        var clientIpString = context.Request.Headers["X-Forwarded-For"].FirstOrDefault()?.Split(',').FirstOrDefault()?.Trim()
            ?? context.Connection.RemoteIpAddress?.ToString();

        if (clientIpString is "127.0.0.1" or "::1" or "0.0.0.1" || context.Request.Path.StartsWithSegments("/mcp"))
        {
            await _next(context);
            return;
        }

        /* 2. 驗證用戶有沒有登入 */
        var user = context.User;
        if (user?.Identity?.IsAuthenticated != true)
        {
            /* 3. 如果沒有登入，則返回 401 Unauthorized, API 不導向，Web 才導向 */
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }

            return;
        }

        if (string.IsNullOrWhiteSpace(clientIpString) || !IPAddress.TryParse(clientIpString, out var clientIp))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        /* var dbContext = context.RequestServices.GetRequiredService<IEipDbContext>(); */

        /* 取得所有啟用中且符合 IP 類型的白名單 */
        /* var clientIpKind = clientIp.AddressFamily == AddressFamily.InterNetwork ? IPKind.IPv4 : IPKind.IPv6; */
        /* var whitelists = await dbContext.IpWhitelists
            .Where(x => x.IsActive && x.IpKind == clientIpKind)
            .ToListAsync(); */

        /* 驗證用戶的 IP 是否在白名單內 */
        /* var isWhitelisted = whitelists.Any(whitelist => IsIpInRange(clientIp, whitelist)); */

        /* if (!isWhitelisted)
        {
            // 4. 如果不在白名單內，則返回 403 Forbidden
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        } */

        /* 5. 如果在白名單內，則繼續處理請求 */
        await _next(context);
    }

    /// <summary>
    /// 檢查 IP 是否在白名單範圍內
    /// </summary>
    /* private static bool IsIpInRange(IPAddress clientIp, IpWhitelist whitelist)
    {
        // 單一 IP 模式 (無 IpEnd)
        if (whitelist.IpEnd == null)
        {
            return IsIpInCidrRange(clientIp, whitelist.IpStart, whitelist.IpStartMask);
        }

        // IP 範圍模式 (IpStart ~ IpEnd)
        return IsIpInRange(clientIp, whitelist.IpStart, whitelist.IpEnd);
    } */

    /// <summary>
    /// 檢查 IP 是否在 CIDR 範圍內
    /// </summary>
    private static bool IsIpInCidrRange(IPAddress clientIp, IPAddress networkAddress, long cidrMask = 8)
    {
        var clientBytes = clientIp.GetAddressBytes();
        var networkBytes = networkAddress.GetAddressBytes();

        if (clientBytes.Length != networkBytes.Length)
        {
            return false;
        }

        var maskBits = (int)cidrMask;
        var maskBytes = CreateMaskBytes(clientBytes.Length, maskBits);

        for (var i = 0; i < clientBytes.Length; i++)
        {
            if ((clientBytes[i] & maskBytes[i]) != (networkBytes[i] & maskBytes[i]))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 檢查 IP 是否在起點和終點範圍內
    /// </summary>
    private static bool IsIpInRange(IPAddress clientIp, IPAddress startIp, IPAddress endIp)
    {
        var clientBytes = clientIp.GetAddressBytes();
        var startBytes = startIp.GetAddressBytes();
        var endBytes = endIp.GetAddressBytes();

        if (clientBytes.Length != startBytes.Length || clientBytes.Length != endBytes.Length)
        {
            return false;
        }

        var clientValue = new BigInteger(clientBytes, isUnsigned: true, isBigEndian: true);
        var startValue = new BigInteger(startBytes, isUnsigned: true, isBigEndian: true);
        var endValue = new BigInteger(endBytes, isUnsigned: true, isBigEndian: true);

        return clientValue >= startValue && clientValue <= endValue;
    }

    /// <summary>
    /// 建立子網路遮罩位元組陣列
    /// </summary>
    private static byte[] CreateMaskBytes(int length, int maskBits)
    {
        var maskBytes = new byte[length];
        var fullBytes = maskBits / 8;
        var remainingBits = maskBits % 8;

        for (var i = 0; i < fullBytes && i < length; i++)
        {
            maskBytes[i] = 0xFF;
        }

        if (fullBytes < length && remainingBits > 0)
        {
            maskBytes[fullBytes] = (byte)(0xFF << (8 - remainingBits));
        }

        return maskBytes;
    }
}
