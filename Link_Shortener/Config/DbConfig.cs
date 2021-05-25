using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Link_Shortener.Config
{
    public class DbConfig
    {
        public string LinksCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}