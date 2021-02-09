using System;
using System.IO;
using Cirara.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Cirara.Controllers
{
    [Route("r/{repoName}/{branch=main}/{commit=-1}")]
    [ApiController]
    public class GitController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly GitService _gitService;

        public GitController(IConfiguration configuration, GitService gitService)
        {
            _configuration = configuration;
            _gitService = gitService;
        }

        // GET api/<UserController>/5
        /// <summary>
        ///     Returns information regarding a user.
        /// </summary>
        /// <param name="repoName">Name of the repository</param>
        /// <param name="branch">Optional branch to pull data from</param>
        /// <param name="commit">Optional commit hash to pull data from</param>
        /// <response code="200">When the user was found</response>
        /// <response code="400">When the payload is missing or the user identifier is ambiguous</response>
        /// <response code="404">When the user couldn't be found</response>
        /// <response code="422">When the user identifier couldn't be parsed as GUID</response>
        /// <response code="500">When any other unhandled exception</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Get(string repoName, string branch, string commit)
        {
            // Check if payload is present
            if (string.IsNullOrEmpty(repoName))
                return BadRequest("GitController (GET) - Missing repository name.");

            if (string.IsNullOrEmpty(branch))
                branch = _configuration["Config:DefaultBranch"];

            try
            {
                // Get user from user service and return it                
                var repository = _gitService.GetRepository(repoName);
                return Content(JsonConvert.SerializeObject(repository));
            }
            catch (InvalidCastException e)
            {
                // Return 422 if we can't parse the user identifier as GUID
                return UnprocessableEntity(e.Message);
            }
            catch (InvalidDataException e)
            {
                // Return 400 if the user identifier is ambiguous
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                // Return 500 if any other exception occurred
                return Problem(e.Message, e.Source, 500, "GitController (GET)", e.GetType().ToString());
            }
        }
    }
}