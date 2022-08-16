using System.Reflection;

namespace ToDoApi.Moq.UtilityClasses
{
    public static class UtilityClass
    {
        public static string GetValueOfNonPublicField(object item, string nameOfField)
        {
            var field = item.GetType().GetField(nameOfField, BindingFlags.NonPublic | BindingFlags.Instance);
            var value = field.GetValue(item);
            return value.ToString();
        }
    }
}
