using LinFx.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinFx.Extensions.Configuration
{
    /// <summary>
    /// 配置实体
    /// </summary>
    [Table("Core_AppSetting")]
    public class AppSetting : Entity<string>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        public AppSetting(string id)
        {
            Id = id;
        }

        /// <summary>
        /// 内容
        /// </summary>
        [StringLength(450)]
        public string Value { get; set; }

        /// <summary>
        /// 模块
        /// </summary>
        [StringLength(450)]
        public string Module { get; set; }

        /// <summary>
        /// 格式化类型
        /// </summary>
        public AppSettingFormatType FormatType { get; set; } = AppSettingFormatType.Text;

        /// <summary>
        /// 类型
        /// </summary>
        [StringLength(450)]
        public string Type { get; set; }

        /// <summary>
        /// 是否在配置里显示
        /// </summary>
        public bool IsVisibleInCommonSettingPage { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(450)]
        public string Note { get; set; }
    }
}
