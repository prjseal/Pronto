using System.Collections.Generic;

namespace Pronto.Models
{
    public class DashboardData
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public List<DashboardLink> UsefulLinks { get; set; }
    }
}
