using Microsoft.AspNetCore.Authorization;
using System;

namespace Epico.Controllers
{
    [Authorize]
    public class FeatureController : FeatureControllerBase
    {
        public FeatureController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
