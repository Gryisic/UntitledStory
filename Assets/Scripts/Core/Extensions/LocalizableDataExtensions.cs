using Core.Data.Interfaces;

namespace Core.Extensions
{
    public static class LocalizableDataExtensions
    {
        public static T As<T>(this ILocalizableTextData data) where T: class, ILocalizableTextData => data as T;
    }
}