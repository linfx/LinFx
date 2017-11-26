using LinFx.Data.Extensions.Mapper;

namespace LinFx.SaaS.Web.Entities
{
    public class UserInfo : Authorization.Users.User
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public int Lng { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public int Lat { get; set; }
    }

    public sealed class UserInfoMap : ClassMapper<UserInfo>
    {
        public UserInfoMap()
        {
            Map(x => x.Id).Key(KeyType.Assigned);
            Map(x => x.Account).Ignore();
            AutoMap();
        }
    }
}