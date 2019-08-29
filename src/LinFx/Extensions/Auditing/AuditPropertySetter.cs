using LinFx.Extensions.MultiTenancy;
using LinFx.Security.Users;
using System;

namespace LinFx.Extensions.Auditing
{
    [Service]
    public class AuditPropertySetter : IAuditPropertySetter
    {
        protected ICurrentUser CurrentUser { get; }
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

        private void SetCreationTime(object targetObject)
        {
            if (!(targetObject is IHasCreationTime objectWithCreationTime))
            {
                return;
            }

            if (objectWithCreationTime.CreationTime == default)
            {
                objectWithCreationTime.CreationTime = DateTimeOffset.Now;
            }
        }

        private void SetCreatorId(object targetObject)
        {
            if (string.IsNullOrEmpty(CurrentUser?.Id))
            {
                return;
            }

            if (targetObject is IMultiTenant multiTenantEntity)
            {
                if (multiTenantEntity.TenantId != CurrentUser?.TenantId)
                {
                    return;
                }
            }

            if (targetObject is IMayHaveCreator mayHaveCreatorObject)
            {
                if (!string.IsNullOrEmpty(mayHaveCreatorObject.CreatorId))
                {
                    return;
                }

                mayHaveCreatorObject.CreatorId = CurrentUser?.Id;
            }
            else if (targetObject is IMustHaveCreator mustHaveCreatorObject)
            {
                if (mustHaveCreatorObject.CreatorId != default)
                {
                    return;
                }

                mustHaveCreatorObject.CreatorId = CurrentUser?.Id;
            }
        }

        private void SetLastModificationTime(object targetObject)
        {
            if (targetObject is IHasModificationTime objectWithModificationTime)
            {
                objectWithModificationTime.LastModificationTime = DateTimeOffset.Now;
            }
        }

        private void SetLastModifierId(object targetObject)
        {
            if (!(targetObject is IModificationAuditedObject modificationAuditedObject))
            {
                return;
            }

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

        private void SetDeletionTime(object targetObject)
        {
            if (targetObject is IHasDeletionTime objectWithDeletionTime)
            {
                if (objectWithDeletionTime.DeletionTime == null)
                {
                    objectWithDeletionTime.DeletionTime = DateTimeOffset.Now;
                }
            }
        }

        private void SetDeleterId(object targetObject)
        {
            if (!(targetObject is IDeletionAuditedObject deletionAuditedObject))
            {
                return;
            }

            if (deletionAuditedObject.DeleterId != null)
            {
                return;
            }

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
}
