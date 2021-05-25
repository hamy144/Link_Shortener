using System;
using System.Collections.Generic;
using System.Text;
using Common.Interfaces;
using Common.Models;
using FluentResults;

namespace Services.Cache
{
    public class RedisCacheService : ICacheService
    {
        //Use distributed caching if it's to be deployed in a scalable cloud environment
        public void AddLinkToCache(Link link)
        {
            throw new NotImplementedException();
        }

        Result<Link> ICacheService.GetLinkFromCache(string id)
        {
            throw new NotImplementedException();
        }
    }
}