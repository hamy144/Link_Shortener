using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Link_Shortener.Config
{
    public class RedisConfig
    {
        public string ConnectionString { get; set; }
        public string InstanceName { get; set; }
    }
}