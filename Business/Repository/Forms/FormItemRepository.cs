using CMS.OnlineForms;

namespace Business.Repository.Forms
{
    class FormItemRepository : IFormItemRepository
    {
        public void InsertFormItem(string className, IFormViewModel viewModel)
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
