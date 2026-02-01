using System.Net;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;

namespace Yuu.Eip.EntityFrameworkCore.ModelCreatingExtensions;

public static class IdentityUserModelCreatingExtensions
{
    public static ObjectExtensionManager ConfigureIdentityUsers(this ObjectExtensionManager objectExtensionManager)
    {
        objectExtensionManager
            .MapEfCoreProperty<IdentityUser, IPAddress>("creatorIpAddress",
                (entityBuilder, propertyBuilder) =>
                {
                    propertyBuilder
                    .IsRequired(false)
                    .HasColumnName("creator_ip_address");
                }
            )
            .MapEfCoreProperty<IdentityUser, IPAddress>("lastModifierIpAddress",
                (entityBuilder, propertyBuilder) =>
                {
                    propertyBuilder
                    .IsRequired(false)
                    .HasColumnName("last_modifier_ip_address");
                }
            )
            .MapEfCoreProperty<IdentityUser, IPAddress>("deleterIpAddress",
                (entityBuilder, propertyBuilder) =>
                {
                    propertyBuilder
                    .IsRequired(false)
                    .HasColumnName("deleter_ip_address");
                }
            );

        return objectExtensionManager;
    }
}
