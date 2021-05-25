using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Link_Shortener.Config
{
    public class config
    {
        public bool IsUsingDynamo { get; set; }
        public bool IsUsingRedis { get; set; }
    }
}