using CMS.OnlineForms;

namespace Business.Repository.Forms
{
    class FormItemRepository : IFormItemRepository
    {
        public void InsertFormItem<TViewModel>(string className, TViewModel viewModel)
            where TViewModel : class, IFormViewModel, new()
        {
            var formItem = BizFormItem.New(className);

            foreach (var field in viewModel.Fields)
            {
                formItem.SetValue(field.Key, field.Value);
            }

            formItem.Insert();
        }
    }
}
