using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MultitenantAspApp.Controllers
{
    [Route("api/[controller]")]
    public class ValuesWithOptionsController : ControllerBase
    {
        private readonly IOptions<ValuesControllerOptions> _options;

        public ValuesWithOptionsController(IOptions<ValuesControllerOptions> options)
             => _options = options ?? throw new System.ArgumentNullException(nameof(options));

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get(string tenantid)
        {
            return new string[] {$"{tenantid}", $"{_options.Value.Value1Value}", _options.Value.Value2Value };
        }
   }
}