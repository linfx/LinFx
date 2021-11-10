namespace LinFx.Application.Models
{
    public interface IFilterRequest
    {
        /// <summary>
        /// 过滤条件
        /// </summary>
        string Filter { get; set; }
    }
}
