using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace LinFx.Extensions.DynamicProxy;

/// <summary>
/// 拦截方法调用参数
/// </summary>
public interface IMethodInvocation
{
    /// <summary>
    /// 参数
    /// </summary>
    object[] Arguments { get; }

    /// <summary>
    /// 参数字典
    /// </summary>
    IReadOnlyDictionary<string, object> ArgumentsDictionary { get; }

    /// <summary>
    /// 泛型参数
    /// </summary>
    Type[] GenericArguments { get; }

    /// <summary>
    /// 目标对象
    /// </summary>
    object TargetObject { get; }

    /// <summary>
    /// 方法
    /// </summary>
    MethodInfo Method { get; }

    /// <summary>
    /// 返回值
    /// </summary>
    object ReturnValue { get; set; }

    /// <summary>
    /// 处理
    /// </summary>
    /// <returns></returns>
    Task ProceedAsync();
}
