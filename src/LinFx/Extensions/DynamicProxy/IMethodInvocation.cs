using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace LinFx.Extensions.DynamicProxy
{
    /// <summary>
    /// 调用方法
    /// </summary>
    public interface IMethodInvocation
    {
        object[] Arguments { get; }

        IReadOnlyDictionary<string, object> ArgumentsDictionary { get; }

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
}