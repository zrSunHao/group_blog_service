using Hao.GroupBlog.Common.Enums;

namespace Hao.GroupBlog.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MyRoleAttribute: Attribute
    {
        /// <summary>
        /// 功能编码
        /// </summary>
        public RoleType[] Types { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code">功能编码</param>
        public MyRoleAttribute(params RoleType[] types)
        {
            Types = types;
        }
    }
}
