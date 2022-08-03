﻿namespace Hao.GroupBlog.Persistence.Database
{
    public class BaseEntity : BaseInfo
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 将其创建用户的Id
        /// </summary>
        public string? CreatedById { get; set; }

        /// <summary>
        /// 最近修改
        /// </summary>
        public DateTime? LastModifiedAt { get; set; }

        /// <summary>
        /// 最后的修改用户Id
        /// </summary>
        public string? LastModifiedById { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// 将其删除用户的Id
        /// </summary>
        public string? DeletedById { get; set; }
    }
}
