using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MultitenantAspApp.Controllers
{
    [Route("api/[controller]")]
    public class ValuesWithDependenciesController : ControllerBase
    {
        private readonly IHelloWorldService _helloWorldService;

        public ValuesWithDependenciesController(IHelloWorldService helloWorldService)
             => _helloWorldService = helloWorldService ?? throw new System.ArgumentNullException(nameof(helloWorldService));

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get(string tenantid)
        {
            return new string[] {$"{tenantid}", _helloWorldService.GetHelloWorld() };
        }
   }
}