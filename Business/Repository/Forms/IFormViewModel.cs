using System.Collections.Generic;

namespace Business.Repository.Forms
{
    public interface IFormViewModel
    {
        IDictionary<string, object> Fields { get; }
    }
}
