using CMS.OnlineForms;

namespace Business.Repository.Forms
{
    public interface IFormItemRepository : IRepository
    {
        void InsertFormItem<TViewModel>(string className, TViewModel viewModel)
            where TViewModel : class, IFormViewModel, new();
    }
}
