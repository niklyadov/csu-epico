using Epico.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epico.Services
{
    interface IMetricService
    {
        public Task<Metric> AddMetric(string name, string description, int? parentMetricId);
    }
}
