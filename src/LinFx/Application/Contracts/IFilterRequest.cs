using LinFx;
using LinFx.Application;
using LinFx.Application.Contracts;

namespace LinFx.Application.Contracts
{
    public interface IFilterRequest
    {
        /// <summary>
        /// 过滤条件
        /// </summary>
        string Filter { get; set; }
    }
}
