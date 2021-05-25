using Common.Interfaces;
using Common.Models;
using FluentResults;
using Link_Shortener.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Infrastructure.Commands
{
    public class CreateLinkCommand : IRequest<Result<string>>
    {
        public string LinkToShorten { get; set; }
    }

    public class CreateLinkCommandHandler : AbstractBaseHandler<CreateLinkCommand, Result<string>>
    {
        public CreateLinkCommandHandler(IDatabaseService databaseService, ICacheService cacheService) : base(databaseService, cacheService)
        {
        }

        public override async Task<Result<string>> Handle(CreateLinkCommand request, CancellationToken cancellationToken)
        {
            //Needs this or else the dotnet redirect won't work
            if (!request.LinkToShorten.StartsWith("http://") && !request.LinkToShorten.StartsWith("https://"))
            {
                request.LinkToShorten = "https://" + request.LinkToShorten;
            }

            byte[] plainTextBytes = Encoding.ASCII.GetBytes(request.LinkToShorten);
            var hash = new MD5CryptoServiceProvider().ComputeHash(plainTextBytes);
            //64^10 = 1 quintillion combinations, probably won't have a collision
            string id = Convert.ToBase64String(hash).Substring(0, 10);
            //awful hack to stop there being slashes in the hash, there's definitely a better way to do this
            Link newLink = new Link(id.Replace("/", "0").Replace("\\", "1"), request.LinkToShorten);

            var createResult = await _databaseService.CreateLinkAsync(newLink);
            if (createResult.IsSuccess)
            {
                _cacheService.AddLinkToCache(newLink);
                return Result.Ok(newLink._id);
            }
            return Result.Fail(createResult.Errors[0]);
        }
    }
}