using System;
using System.Web.Mvc;

using Business.DependencyInjection;
using Business.Repository.Forms;
using MedioClinic.Models;
using MedioClinic.Models.Forms;

namespace MedioClinic.Controllers
{
    public class FormTestController : BaseController
    {
        protected const string PageName = "Form test";

        IFormItemRepository FormItemRepository { get; }

        public FormTestController(
            IBusinessDependencies businessDependencies,
            IFormItemRepository formItemRepository)
            : base(businessDependencies)
        {
            FormItemRepository = formItemRepository 
                ?? throw new ArgumentNullException(nameof(formItemRepository));
        }

        // GET: FormTest
        [HttpGet]
        public ActionResult Index()
        {
            var model = GetPageViewModel(new FloridaEventRegistrationViewModel(), 
                PageName);

            return View(model);
        }

        // POST: FormTest
        [HttpPost]
        public ActionResult Index(
            [System.Web.Http.FromBody] PageViewModel<FloridaEventRegistrationViewModel> uploadModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    FormItemRepository.InsertFormItem("BizForm.FloridaEventRegistration",
                                uploadModel.Data);
                }
                catch
                {
                    return Content("There was an error when submitting your form.");
                }

                return Content("Your form has been submitted.");
            }

            var model = GetPageViewModel(uploadModel.Data, PageName);

            return View(model);
        }
    }
}