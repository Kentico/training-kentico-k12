using System.Collections.Generic;

namespace Business.Repository.Forms
{
    /// <summary>
    /// A view model that provides its data through a dictionary.
    /// </summary>
    public interface IFormViewModel
    {
        /// <summary>
        /// Dictionary that provides unified acces to strongly-typed property members.
        /// </summary>
        IDictionary<string, object> Fields { get; }
    }
}
