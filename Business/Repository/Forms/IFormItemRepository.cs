namespace Business.Repository.Forms
{
    /// <summary>
    /// A general repository, usable with multiple form view models.
    /// </summary>
    public interface IFormItemRepository : IRepository
    {
        /// <summary>
        /// Inserts data from a form submission.
        /// </summary>
        /// <param name="className">Kentico form class name.</param>
        /// <param name="viewModel">A view model of the form.</param>
        void InsertFormItem(string className, IFormViewModel viewModel);
    }
}
