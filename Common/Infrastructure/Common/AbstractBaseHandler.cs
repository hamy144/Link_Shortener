using Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Link_Shortener.Common
{
    public abstract class AbstractBaseHandler<T, TR> : IRequestHandler<T, TR> where T : IRequest<TR>
    {
        protected IDatabaseService _databaseService;
        protected ICacheService _cacheService;

        protected AbstractBaseHandler(IDatabaseService databaseService, ICacheService cacheService)
        {
            _databaseService = databaseService;
            _cacheService = cacheService;
        }

        public abstract Task<TR> Handle(T request, CancellationToken cancellationToken);
    }
}