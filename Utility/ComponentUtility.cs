using System;
using System.Text.RegularExpressions;

namespace OtherEngine.ES.Utility
{
	public static class ComponentUtility
	{
		static readonly Regex _componentSuffix = new Regex("Component$");


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
					"{0} is not a struct", ToStringInternal(component.GetType())), paramName);
		}

		/// <summary> Returns a string representation of the component's type. </summary>
		public static string ToString(IComponent component)
		{
			Validate(component);
			return ToStringInternal(component.GetType());
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
					"{0} is not an IComponent", ToStringInternal(componentType)), paramName);
			if (!componentType.IsValueType)
				throw new ArgumentException(string.Format(
					"{0} is not a struct", ToStringInternal(componentType)), paramName);
		}

		/// <summary> Returns a string representation of the component type. </summary>
		public static string ToString(Type componentType)
		{
			Validate(componentType);
			return ToStringInternal(componentType);
		}

		/// <summary> Returns if the specified component type's
		///           name is valid (ends with "Component"). </summary>
		public static bool IsValidName(Type componentType)
		{
			Validate(componentType);
			return ((componentType.Name.Length > 9) &&
				_componentSuffix.IsMatch(componentType.Name));
		}


		static string ToStringInternal(Type componentType)
		{
			return string.Format("[Component {0}]",
				_componentSuffix.Replace(componentType.ToString(), ""));
		}
	}
}

