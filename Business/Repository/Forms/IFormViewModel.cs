using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.Forms
{
    public interface IFormViewModel
    {
        IDictionary<string, object> Fields { get; }
    }
}
