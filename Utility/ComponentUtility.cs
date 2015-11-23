using System;

namespace OtherEngine.ES.Utility
{
	public static class ComponentUtility
	{
		/// <summary> Returns if the specified type is a valid component. </summary>
		public static bool IsValid(IComponent component)
		{
			return (component is ValueType);
		}

		/// <summary> Validates the specified component,
		///           throwing an exception if it's invalid. </summary>
		public static void Validate(IComponent component, string paramName = "component")
		{
			if (component == null)
				throw new ArgumentNullException(paramName);
			if (!(component is ValueType))
				throw new ArgumentException(string.Format(
					"{0} is not a struct", component.GetType()), paramName);
		}


		/// <summary> Returns if the specified type is a valid component type. </summary>
		public static bool IsValid(Type componentType)
		{
			return (typeof(IComponent).IsAssignableFrom(componentType) && componentType.IsValueType);
		}

		/// <summary> Validates the specified component type,
		///           throwing an exception if it's invalid. </summary>
		public static void Validate(Type componentType, string paramName = "componentType")
		{
			if (componentType == null)
				throw new ArgumentNullException(paramName);
			if (!typeof(IComponent).IsAssignableFrom(componentType))
				throw new ArgumentException(string.Format(
					"{0} is not an IComponent", componentType), paramName);
			if (!componentType.IsValueType)
				throw new ArgumentException(string.Format(
					"{0} is not a struct", componentType), paramName);
		}
	}
}

