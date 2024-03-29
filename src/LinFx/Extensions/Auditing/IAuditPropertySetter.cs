﻿namespace LinFx.Extensions.Auditing;

/// <summary>
/// 属性自动设置器
/// </summary>
public interface IAuditPropertySetter
{
    /// <summary>
    /// 设置创建属性
    /// </summary>
    /// <param name="targetObject"></param>
    void SetCreationProperties(object targetObject);

    /// <summary>
    /// 设置修改属性
    /// </summary>
    /// <param name="targetObject"></param>
    void SetModificationProperties(object targetObject);

    /// <summary>
    /// 设置删除属性
    /// </summary>
    /// <param name="targetObject"></param>
    void SetDeletionProperties(object targetObject);
}
