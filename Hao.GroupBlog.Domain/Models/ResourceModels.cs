using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hao.GroupBlog.Domain.Models
{
    public class ResourceM
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string OwnId { get; set; }

        public string Category { get; set; }

        public string FileName { get; set; }

        public string? Type { get; set; }

        public string? Suffix { get; set; }

        public long Size { get; set; }
    }
}
