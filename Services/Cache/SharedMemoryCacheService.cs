using Common.Interfaces;
using Common.Models;
using FluentResults;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Cache
{
    public class SharedMemoryCacheService : ICacheService
    {
        private IMemoryCache _cache;

        public SharedMemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void AddLinkToCache(Link link)
        {
            _cache.Set(link._id, link);
        }

        Result<Link> ICacheService.GetLinkFromCache(string id)
        {
            Link cacheEntry = _cache.Get<Link>(id);
            if (cacheEntry != null)
            {
                cacheEntry.LastAccessed = DateTime.Now;
                _cache.Set(cacheEntry._id, cacheEntry);
                return Result.Ok(cacheEntry);
            }
            return Result.Fail("Not in Cache");
        }
    }
}