using LinFx.Extensions.MultiTenancy;
using LinFx.Security.Users;
using System;

namespace LinFx.Extensions.Auditing;

[Service]
public class AuditPropertySetter : IAuditPropertySetter
{
    /// <summary>
    /// 当前用户
    /// </summary>
    protected ICurrentUser CurrentUser { get; }

    /// <summary>
    /// 当前租户
    /// </summary>
    protected ICurrentTenant CurrentTenant { get; }

    public AuditPropertySetter(ICurrentUser currentUser, ICurrentTenant currentTenant)
    {
        CurrentUser = currentUser;
        CurrentTenant = currentTenant;
    }

    public void SetCreationProperties(object targetObject)
    {
        SetCreationTime(targetObject);
        SetCreatorId(targetObject);
    }

    public void SetModificationProperties(object targetObject)
    {
        SetLastModificationTime(targetObject);
        SetLastModifierId(targetObject);
    }

    public void SetDeletionProperties(object targetObject)
    {
        SetDeletionTime(targetObject);
        SetDeleterId(targetObject);
    }

    /// <summary>
    /// 设置创建时间
    /// </summary>
    /// <param name="targetObject"></param>
    private void SetCreationTime(object targetObject)
    {
        if (targetObject is not IHasCreationTime objectWithCreationTime)
            return;

        if (objectWithCreationTime.CreationTime == default)
            objectWithCreationTime.CreationTime = DateTimeOffset.UtcNow;
    }

    private void SetCreatorId(object targetObject)
    {
        if (string.IsNullOrEmpty(CurrentUser?.Id))
            return;

        if (targetObject is IMultiTenant multiTenantEntity)
        {
            if (multiTenantEntity.TenantId != CurrentUser?.TenantId)
                return;
        }

        if (targetObject is IMayHaveCreator mayHaveCreatorObject)
        {
            if (!string.IsNullOrEmpty(mayHaveCreatorObject.CreatorId))
                return;

            mayHaveCreatorObject.CreatorId = CurrentUser?.Id;
        }
        else if (targetObject is IMustHaveCreator mustHaveCreatorObject)
        {
            if (mustHaveCreatorObject.CreatorId != default)
                return;

            mustHaveCreatorObject.CreatorId = CurrentUser?.Id;
        }
    }

    /// <summary>
    /// 设置最后修改时间
    /// </summary>
    /// <param name="targetObject"></param>
    private void SetLastModificationTime(object targetObject)
    {
        if (targetObject is IHasModificationTime objectWithModificationTime)
            objectWithModificationTime.LastModificationTime = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// 设备最后修改者Id
    /// </summary>
    /// <param name="targetObject"></param>
    private void SetLastModifierId(object targetObject)
    {
        if (targetObject is not IModificationAuditedObject modificationAuditedObject)
            return;

        if (string.IsNullOrEmpty(CurrentUser?.Id))
        {
            modificationAuditedObject.LastModifierId = null;
            return;
        }

        if (modificationAuditedObject is IMultiTenant multiTenantEntity)
        {
            if (multiTenantEntity.TenantId != CurrentUser.TenantId)
            {
                modificationAuditedObject.LastModifierId = null;
                return;
            }
        }

        modificationAuditedObject.LastModifierId = CurrentUser?.Id;
    }

    /// <summary>
    /// 设置删除时间
    /// </summary>
    /// <param name="targetObject"></param>
    private void SetDeletionTime(object targetObject)
    {
        if (targetObject is IHasDeletionTime objectWithDeletionTime)
        {
            if (objectWithDeletionTime.DeletionTime == null)
                objectWithDeletionTime.DeletionTime = DateTimeOffset.UtcNow;
        }
    }

    /// <summary>
    /// 设置删除者Id
    /// </summary>
    /// <param name="targetObject"></param>
    private void SetDeleterId(object targetObject)
    {
        if (targetObject is not IDeletionAuditedObject deletionAuditedObject)
            return;

        if (deletionAuditedObject.DeleterId != null)
            return;

        if (string.IsNullOrEmpty(CurrentUser?.Id))
        {
            deletionAuditedObject.DeleterId = null;
            return;
        }

        if (deletionAuditedObject is IMultiTenant multiTenantEntity)
        {
            if (multiTenantEntity.TenantId != CurrentUser?.TenantId)
            {
                deletionAuditedObject.DeleterId = null;
                return;
            }
        }

        deletionAuditedObject.DeleterId = CurrentUser?.Id;
    }
}
