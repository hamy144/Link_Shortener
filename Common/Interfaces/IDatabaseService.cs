using Common.Models;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IDatabaseService
    {
        Task<Result<Link>> GetLink(string id);

        Task UpdateLink(Link link);

        Task<Result<string>> CreateLinkAsync(Link link);

        Task DeleteLink(string id);
    }
}