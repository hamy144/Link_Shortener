using Common.Interfaces;
using FluentResults;
using Link_Shortener.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Link_Shortener.Queries
{
    public class GetLinkQuery : IRequest<Result<string>>
    {
        public string LinkId { get; set; }
    }

    public class GetLinkQueryHandler : AbstractBaseHandler<GetLinkQuery, Result<string>>
    {
        public GetLinkQueryHandler(IDatabaseService databaseService, ICacheService cacheService) : base(databaseService, cacheService)
        {
        }

        public override async Task<Result<string>> Handle(GetLinkQuery request, CancellationToken cancellationToken)
        {
            var cacheResult = _cacheService.GetLinkFromCache(request.LinkId);
            if (cacheResult.IsSuccess)
            {
                await _databaseService.UpdateLink(cacheResult.Value);
                return Result.Ok(cacheResult.Value.ShortenedLink);
            }

            var dbResult = await _databaseService.GetLink(request.LinkId);
            if (dbResult.IsSuccess)
            {
                _cacheService.AddLinkToCache(dbResult.Value);
                return Result.Ok(dbResult.Value.ShortenedLink);
            }

            return Result.Fail("Link doesn't exist");
        }
    }
}