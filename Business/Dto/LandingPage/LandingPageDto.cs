using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Dto.LandingPage
{
    public class LandingPageDto : IDto
    {
        public int DocumentId { get; set; }
        public string Title { get; set; }
    }
}
