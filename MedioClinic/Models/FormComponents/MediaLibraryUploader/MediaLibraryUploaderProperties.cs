using System.ComponentModel.DataAnnotations;

using CMS.DataEngine;
using Kentico.Forms.Web.Mvc;

namespace MedioClinic.Models.FormComponents
{
    public class MediaLibraryUploaderProperties : FormComponentProperties<string>
    {
        public MediaLibraryUploaderProperties() : base(FieldDataType.Text, 400)
        {
        }

        [DefaultValueEditingComponent(TextInputComponent.IDENTIFIER)]
        public override string DefaultValue { get; set; } = string.Empty;

        [EditingComponent(MediaLibrarySelectionComponent.Identifier, 
            Label = "{$FormComponent.MediaLibraryUploader.MediaLibraryId.Name$}", 
            Tooltip = "{$FormComponent.MediaLibraryUploader.MediaLibraryId.Tooltip$}", 
            ExplanationText = "{$FormComponent.MediaLibraryUploader.MediaLibraryId.ExplanationText$}",
            Order = 0)]
        [Required]
        public string MediaLibraryId { get; set; }
    }
}