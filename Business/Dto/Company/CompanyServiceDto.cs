using CMS.DocumentEngine;

namespace Business.Dto.Company
{
    public class CompanyServiceDto : IDto
    {
        public string Text { get; set; }
        public DocumentAttachment Icon { get; set; }
        public string Header { get; set; }
    }
}
