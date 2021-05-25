using Common.Interfaces;
using Common.Models;
using FluentResults;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Database
{
    public class MongoDatabaseService : IDatabaseService
    {
        private IMongoCollection<Link> _links;
        private ILogger<MongoDatabaseService> _logger;

        public MongoDatabaseService(IMongoCollection<Link> links, ILogger<MongoDatabaseService> logger)
        {
            _links = links;
            _logger = logger;
        }

        public async Task<Result<string>> CreateLinkAsync(Link link)
        {
            Link existingLink = _links.Find<Link>(l => l.ShortenedLink == link.ShortenedLink).FirstOrDefault();
            if (existingLink != null)
            {
                return Result.Ok(existingLink._id);
            }

            try
            {
                await _links.InsertOneAsync(link);
                return Result.Ok(link._id);
            }
            catch (AggregateException)
            {
                _logger.LogError("Collision occured on id: {0}", link._id);
                return Result.Fail("Collision occured");

                //Extra collision prevention, not really needed with MD5 64^10
                //Random random = new Random();
                //var bytes = new byte[5];
                //random.NextBytes(bytes);
                //link._id += Encoding.ASCII.GetString(bytes);
                //await _links.InsertOneAsync(link);
            }
        }

        public async Task DeleteLink(string id)
        {
            await _links.DeleteOneAsync(Builders<Link>.Filter.Eq("_id", id));
        }

        public async Task<Result<Link>> GetLink(string id)
        {
            Link link = _links.Find<Link>(l => l._id == id).FirstOrDefault();

            if (link != null)
            {
                link.LastAccessed = DateTime.Now;
                await UpdateLink(link);
                return Result.Ok(link);
            }
            else return Result.Fail("Not in DB"); ;
        }

        public async Task UpdateLink(Link link)
        {
            await _links.UpdateOneAsync(
                               Builders<Link>.Filter.Eq("_id", link._id),
                               Builders<Link>.Update.Set("LastAccessed", link.LastAccessed));
        }
    }
}