namespace StarStuff.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    public static class TempDataDictionaryExtensions
    {
        public static void AddSuccessMessage(this ITempDataDictionary tempData, string message)
        {
            tempData[WebConstants.TempDataSuccessMessage] = message;
        }

        public static void AddErrorMessage(this ITempDataDictionary tempData, string message)
        {
            tempData[WebConstants.TempDataErrorMessage] = message;
        }
    }
}