using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace YiSha.Common
{
    /// <summary>
    /// 控制器模型绑定
    /// </summary>
    public class ModelBindingMetadataProvider : IMetadataDetailsProvider, IDisplayMetadataProvider
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
