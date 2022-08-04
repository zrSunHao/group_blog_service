﻿using Hao.GroupBlog.Persistence.Attributes;

namespace Hao.GroupBlog.Persistence.Database
{
    public class BaseInfo
    {
        /// <summary>
        /// 表码
        /// </summary>
        private string _tablePrefix;

        /// <summary>
        /// 时间码
        /// </summary>
        private string _time = "";

        /// <summary>
        /// 顺序码
        /// </summary>
        private int _index = 1;

        private Random _random = new Random();
        private static object _lock = new object();

        /// <summary>
        /// 获取实体Id
        /// </summary>
        /// <param name="machine">机器码</param>
        /// <returns></returns>
        public string GetId(string machine)
        {
            if (string.IsNullOrEmpty(machine))
                throw new ArgumentNullException("机器码为空");
            lock (_lock)
            {
                string time = DateTime.Now.ToString("yyMMddHHmmss");
                if (time != _time) { _time = time; _index = 1; }
                else _index++;
                if (string.IsNullOrEmpty(_tablePrefix))
                {
                    var prefix = GetTablePrefix();
                    if (string.IsNullOrEmpty(prefix)) throw new ArgumentException("实体中未设置表的前缀");
                    if (prefix.Length > 4) throw new ArgumentException("实体设置的表前缀过长，最长为4位");
                    else _tablePrefix = prefix;
                }
            }
            string rand = _random.Next(100, 999).ToString();
            // 表前缀4位、时间13位、机器码4位、顺序码5位、随机码3位
            return $"{_tablePrefix}{_time}{machine}X{string.Format("{0:D5}", _index)}{rand}";
        }

        private string GetTablePrefix()
        {
            string prefix = "";
            if (!string.IsNullOrEmpty(_tablePrefix)) return _tablePrefix;
            Type type = typeof(TablePrefixAttribute);
            object? obj = this.GetType()
                .GetCustomAttributes(false)
                .ToList()
                .FirstOrDefault(x => x.GetType() == type);
            if (obj != null)
            {
                var attr = obj as TablePrefixAttribute;
                prefix = attr != null ? attr.Prefix : "";
            }
            return prefix;
        }
    }
}
