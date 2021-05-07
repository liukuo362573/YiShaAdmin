using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace YiSha.Util
{
    /// <summary>
    /// Controller Model Binding 处理
    /// </summary>
    public class ModelBindingMetadataProvider : IDisplayMetadataProvider
    {
        public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
        {
            if (context.Key.MetadataKind == ModelMetadataKind.Property)
            {
                context.DisplayMetadata.ConvertEmptyStringToNull = false;
            }
        }
    }
}