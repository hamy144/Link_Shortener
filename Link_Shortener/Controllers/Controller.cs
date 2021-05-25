using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Common.Infrastructure.Commands;
using Common.Interfaces;
using Common.Models;
using Link_Shortener.Config;
using Link_Shortener.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Link_Shortener.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Controller : ControllerBase
    {
        private readonly ILogger<Controller> _logger;
        private IMediator _mediator;

        public Controller(ILogger<Controller> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("{index}")]
        public async Task<IActionResult> Get(string index)
        {
            var getLinkResult = await _mediator.Send(new GetLinkQuery { LinkId = index });
            if (getLinkResult.IsSuccess)
            {
                return Redirect(getLinkResult.Value);
            }
            else return BadRequest("Link does not exist/has been deleted");
        }

        [HttpPost]
        public async Task<IActionResult> Create(string urlToShorten)
        {
            if (!string.IsNullOrWhiteSpace(urlToShorten))
            {
                var linkResult = await _mediator.Send(new CreateLinkCommand { LinkToShorten = urlToShorten });
                if (linkResult.IsSuccess)
                {
                    return Ok("https://" + Request.Host.Value + "/" + linkResult.Value);
                }
            }
            return BadRequest("Failed to create");
        }

        [HttpDelete]
        public IActionResult Delete(string index)
        {
            return Ok();
        }
    }
}