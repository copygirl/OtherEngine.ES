using System;
using System.Collections.Generic;

namespace OtherEngine.ES.Interfaces
{
	/// <summary> Interface type that handles associating
	///           components of type TComponent to entities. </summary>
	public interface IComponentGrouping<TComponent> : IComponentGrouping,
	IReadOnlyCollection<EntityComponentPair<TComponent>>
		where TComponent : struct, IComponent
	{
		/// <summary> Gets the component value associated with
		///           the specified entity, or null if none. </summary>
		new TComponent? Get(Entity entity);

		/// <summary> Sets the component value associated with the specified
		///           entity, returning the old value, or null if none. </summary>
		TComponent? Set(Entity entity, TComponent? value);
	}

	/// <summary> Associates components of a single type to various entities. </summary>
	public interface IComponentGrouping : IReadOnlyCollection<IEntityComponentPair>
	{
		/// <summary> Gets the component type of this grouping. </summary>
		Type ComponentType { get; }


		/// <summary> Gets the component value associated with
		///           the specified entity, or null if none. </summary>
		IComponent Get(Entity entity);

		/// <summary> Sets the component value associated with the specified
		///           entity, returning the old value, or null if none. </summary>
		/// <exception cref="ArgumentException">
		///   Thrown if the specified component isn't of type ComponentType or null. </exception>
		IComponent Set(Entity entity, IComponent value);
	}
}

