using System;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;

namespace Yuu.Eip.EntityFrameworkCore.ModelCreatingExtensions;

public static class IdentityRoleModelCreatingExtensions
{
    public static ObjectExtensionManager ConfigureIdentityRoles(this ObjectExtensionManager objectExtensionManager)
    {
        objectExtensionManager
            .MapEfCoreProperty<IdentityRole, Guid?>("creatorId",
                (entityBuilder, propertyBuilder) =>
                {
                    propertyBuilder
                    .IsRequired(false)
                    .HasColumnName("creator_id");
                }
            )
            .MapEfCoreProperty<IdentityRole, IPAddress>("creatorIpAddress",
                (entityBuilder, propertyBuilder) =>
                {
                    propertyBuilder
                    .IsRequired(false)
                    .HasColumnName("creator_ip_address");
                }
            )
            .MapEfCoreProperty<IdentityRole, Guid?>("lastModifierId",
                (entityBuilder, propertyBuilder) =>
                {
                    propertyBuilder
                    .IsRequired(false)
                    .HasColumnName("last_modifier_id");
                }
            )
            .MapEfCoreProperty<IdentityRole, DateTime?>("lastModificationTime",
                (entityBuilder, propertyBuilder) =>
                {
                    propertyBuilder
                    .IsRequired(false)
                    .HasColumnName("last_modification_time");
                }
            )
            .MapEfCoreProperty<IdentityRole, IPAddress>("lastModifierIpAddress",
                (entityBuilder, propertyBuilder) =>
                {
                    propertyBuilder
                    .IsRequired(false)
                    .HasColumnName("last_modifier_ip_address");
                }
            )
            .MapEfCoreProperty<IdentityRole, bool>("isDeleted",
                (entityBuilder, propertyBuilder) =>
                {
                    propertyBuilder
                    .IsRequired()
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");
                }
            )
            .MapEfCoreProperty<IdentityRole, DateTime?>("deletionTime",
                (entityBuilder, propertyBuilder) =>
                {
                    propertyBuilder
                    .IsRequired(false)
                    .HasColumnName("deletion_time");
                }
            )
            .MapEfCoreProperty<IdentityRole, Guid?>("deleterId",
                (entityBuilder, propertyBuilder) =>
                {
                    propertyBuilder
                    .IsRequired(false)
                    .HasColumnName("deleter_id");
                }
            )
            .MapEfCoreProperty<IdentityRole, IPAddress>("deleterIpAddress",
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
