using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.API.Dtos
{
    public class LinkDto
    {
        public string Href { get; set; } //链接
        public string Rel { get; set; } //描述
        public string Method { get; set; } //http方法
        public LinkDto(string href, string rel, string method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }
    }
}
