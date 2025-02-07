
using Sigil;
using System.Reflection;


namespace EcoTech.Shared.Extensions;

public static class Extensions
{

    public static Func<object, object> CreateGetter(Type targetType, string propertyName, PropertyInfo propertyInfo)
    {
        // Emit IL for a method that gets the value of the property
        var emitter = Emit<Func<object, object>>.NewDynamicMethod($"{targetType.Name}{AppConstants.HyphenAsString}{propertyName}")
            .LoadArgument(0)
            // Cast it from object to the specific type
            .CastClass(targetType)
            // Call the property getter
            .Call(propertyInfo.GetGetMethod(true));
        if (propertyInfo.PropertyType.IsValueType)
        {
            emitter.Box(propertyInfo.PropertyType);
        }
        // Return the property value
        emitter.Return();
        // Compile the IL code into a delegate and return it
        return emitter.CreateDelegate();

    }

    /*public static T GetValueFromRequest<T>(this IHttpContextAccessor context, string name)
    {
        return (T)context?.HttpContext?.Items[name]!;

    }*/

}
