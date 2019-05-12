using System.Collections.Generic;
using Umbraco.Core.Models;

namespace Pronto.Models
{
    public class DashboardData
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public List<DashboardLink> UsefulLinks { get; set; }
    }
}
