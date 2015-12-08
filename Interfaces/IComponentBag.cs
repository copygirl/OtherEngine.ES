using System;
using System.Collections.Generic;

namespace OtherEngine.ES.Interfaces
{
	/// <summary> Handles access to components of different types. </summary>
	public interface IComponentBag : IReadOnlyCollection<IComponent>
	{
		/// <summary> Returns the component value of
		///           type TComponent, or null if none. </summary>
		TComponent? Get<TComponent>()
			where TComponent : struct, IComponent;

		/// <summary> Sets the component value of type TComponent
		///           and returns the old value, null if none. </summary>
		TComponent? Set<TComponent>(TComponent? component)
			where TComponent : struct, IComponent;


		/// <summary> Returns the component value of the
		///           specified component type, or null if none. </summary>
		/// <exception cref="ArgumentException">
		///   Thrown if the specified type is not a valid component type. </exception>
		IComponent Get(Type componentType);

		/// <summary> Sets the component value of the specified
		///           component type and returns the old value. </summary>
		/// <exception cref="ArgumentException">
		///   Thrown if the specified type is not a valid component type
		///    -OR-  if the specified component is not of the specified type or null. </exception>
		IComponent Set(Type componentType, IComponent component);
	}

	public static class ComponentBagExtensions
	{
		/// <summary> Returns whether the bag has a component of type TComponent. </summary>
		public static bool Has<TComponent>(this IComponentBag bag)
			where TComponent : struct, IComponent
		{
			if (bag == null)
				throw new ArgumentNullException("bag");
			return (bag.Get<TComponent>() != null);
		}

		/// <summary> Sets the component value of type TComponent in
		///           this bag and returns the old value, null if none. </summary>
		public static TComponent? Set<TComponent>(this IComponentBag bag, TComponent component)
			where TComponent : struct, IComponent
		{
			if (bag == null)
				throw new ArgumentNullException("bag");
			return bag.Set((TComponent?)component);
		}

		/// <summary> Removes the component value of type TComponent from
		///           this bag and returns the old value, null if none. </summary>
		public static TComponent? Remove<TComponent>(this IComponentBag bag)
			where TComponent : struct, IComponent
		{
			if (bag == null)
				throw new ArgumentNullException("bag");
			return bag.Set((TComponent?)null);
		}


		/// <summary> Returns whether the bag has a component
		///           of the specified component type. </summary>
		/// <exception cref="ArgumentException">
		///   Thrown if the specified type is not a valid component type. </exception>
		public static bool Has(this IComponentBag bag, Type componentType)
		{
			if (bag == null)
				throw new ArgumentNullException("bag");
			return (bag.Get(componentType) != null);
		}

		/// <summary> Removes the component value of the specified type from
		///           this bag and returns the old value, null if none. </summary>
		/// <exception cref="ArgumentException">
		///   Thrown if the specified type is not a valid component type. </exception>
		public static IComponent Remove(this IComponentBag bag, Type componentType)
		{
			if (bag == null)
				throw new ArgumentNullException("bag");
			return bag.Set(componentType, null);
		}
	}
}

