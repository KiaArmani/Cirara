using System;
using System.IO;
using Cirara.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace Cirara.Controllers
{
    [Route("r/{repoName}/{branchName?}/{commitHash?}")]
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
        /// <param name="branchName">Optional branch to pull data from</param>
        /// <param name="commitHash">Optional SHA-1 commit hash to pull data from</param>
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
        public ActionResult Get(string repoName, string? branchName, string? commitHash)
        {
            // Check if payload is present
            if (string.IsNullOrEmpty(repoName))
                return BadRequest("GitController (GET) - Missing repository name.");

            try
            {
                if (String.IsNullOrEmpty(branchName) && string.IsNullOrEmpty(commitHash))
                {
                    var repository = _gitService.GetRepository(repoName);
                    return Content(JsonConvert.SerializeObject(repository), MediaTypeHeaderValue.Parse("application/json"));
                }
                else if (!string.IsNullOrEmpty(branchName) && string.IsNullOrEmpty(commitHash))
                {
                    var branch = _gitService.GetBranch(repoName, branchName);
                    return Content(JsonConvert.SerializeObject(branch), MediaTypeHeaderValue.Parse("application/json"));
                }
                else if (!string.IsNullOrEmpty(branchName) && !string.IsNullOrEmpty(commitHash))
                {
                    var commit = _gitService.GetSlimCommit(repoName, branchName, commitHash);
                    return Content(JsonConvert.SerializeObject(commit), MediaTypeHeaderValue.Parse("application/json"));
                }

                return BadRequest("No valid parameters provided.");
            }
            catch (Exception e)
            {
                // Return 500 if any other exception occurred
                return Problem(e.Message, e.Source, 500, "GitController (GET)", e.GetType().ToString());
            }
        }
    }
}