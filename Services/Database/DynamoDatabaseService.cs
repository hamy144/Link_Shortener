using Common.Interfaces;
using Common.Models;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Database
{
    public class DynamoDatabaseService : IDatabaseService
    {
        //Use Dynamo DB when hosted on a dockerised scalable cloud environment
        public Task<Result<string>> CreateLinkAsync(Link link)
        {
            throw new NotImplementedException();
        }

        public Task DeleteLink(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Link>> GetLink(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateLink(Link link)
        {
            throw new NotImplementedException();
        }
    }
}