using Common.Models;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface ICacheService
    {
        Result<Link> GetLinkFromCache(string id);

        void AddLinkToCache(Link link);
    }
}