using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PollyRetry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IGithubService _githubService;
        public UserProfileController(IGithubService githubService)
        {
            _githubService = githubService;
        }
        [HttpGet("{userName}")]
        public async Task<IActionResult> GetProfileAsync(string userName)
        {
            var profile = await _githubService.GetProfileByUserName(userName);
            return profile is not null ? Ok(profile) : NotFound();
        }
        
        [HttpGet("orgs/{orgsName}")]
        public async Task<IActionResult> GetOrgAsync(string orgsName)
        {
            var profile = await _githubService.GetOrgByName(orgsName);
            return profile is not null ? Ok(profile) : NotFound();
        }
    }
}