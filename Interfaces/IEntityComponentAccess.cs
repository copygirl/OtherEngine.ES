using System;
using System.Collections.Generic;

namespace OtherEngine.ES.Interfaces
{
	public interface IEntityComponentAccess
	{
		/// <summary> Gets a collection of entities handled by this data structure. </summary>
		IReadOnlyCollection<Entity> Entities { get; }

		/// <summary> Gets a collection of component types handled by this data structure. </summary>
		IReadOnlyCollection<Type> ComponentTypes { get; }

		/// <summary> Gets the preferred way of accessing entity-component relationships. </summary>
		PreferredAccess PreferredAccess { get; }


		/// <summary> Returns an interface handling access to components
		///           associated with the specified entity in this data structure. </summary>
		IComponentBag For(Entity entity);

		/// <summary> Returns an interface handling access component associations
		///           of type TComponent with entities in this data structure. </summary>
		IComponentGrouping<TComponent> For<TComponent>()
			where TComponent : struct, IComponent;

		/// <summary> Returns an interface handling access component associations of
		///           the specified component type with entities in this data structure. </summary>
		IComponentGrouping For(Type componentType);
	}

	public enum PreferredAccess
	{
		ByEntity,
		ByComponent
	}

	public static class EntityComponentAccessExtensions
	{
		#region Generic entity-component access methods (preferred)

		/// <summary> Returns the component of type TComponent associated with the
		///           specified entity in this data structure, or null if none. </summary>
		public static TComponent? Get<TComponent>(
			this IEntityComponentAccess access, Entity entity)
			where TComponent : struct, IComponent
		{
			if (access == null)
				throw new ArgumentNullException("access");
			switch (access.PreferredAccess) {
				case PreferredAccess.ByEntity:
					return access.For(entity).Get<TComponent>();
				case PreferredAccess.ByComponent:
					return access.For<TComponent>().Get(entity);
				default:
					throw new InvalidOperationException(
						"Unknown PreferredAccess value");
			}
		}

		/// <summary> Sets the component value of type TComponent associated
		///           with the specified entity in this data structure,
		///           returning the old value, or null if none. </summary>
		public static TComponent? Set<TComponent>(
			this IEntityComponentAccess access,
			Entity entity, TComponent? component)
			where TComponent : struct, IComponent
		{
			if (access == null)
				throw new ArgumentNullException("access");
			switch (access.PreferredAccess) {
				case PreferredAccess.ByEntity:
					return access.For(entity).Set<TComponent>(component);
				case PreferredAccess.ByComponent:
					return access.For<TComponent>().Set(entity, component);
				default:
					throw new InvalidOperationException(
						"Unknown PreferredAccess value");
			}
		}


		/// <summary> Returns if there's a component of type TComponent
		///           associated with the specified entity in this data structure. </summary>
		public static bool Has<TComponent>(
			this IEntityComponentAccess access, Entity entity)
			where TComponent : struct, IComponent
		{
			return (access.Get<TComponent>(entity) != null);
		}

		/// <summary> Sets the component value of type TComponent associated
		///           with the specified entity in this data structure,
		///           returning the old value, or null if none. </summary>
		public static TComponent? Set<TComponent>(
			this IEntityComponentAccess access,
			Entity entity, TComponent component)
			where TComponent : struct, IComponent
		{
			return access.Set<TComponent>(entity, (TComponent?)component);
		}

		/// <summary> Removes the component of type TComponent associated
		///           with the specified entity in this data structure,
		///           returning the old value, or null if none. </summary>
		public static TComponent? Remove<TComponent>(
			this IEntityComponentAccess access, Entity entity)
			where TComponent : struct, IComponent
		{
			return access.Set<TComponent>(entity, (TComponent?)null);
		}

		#endregion

		#region General entity-component access methods

		/// <summary> Returns the component of the specified component type associated
		///           with the specified entity in this data structure, or null if none. </summary>
		public static IComponent Get(
			this IEntityComponentAccess access,
		    Entity entity, Type componentType)
		{
			if (access == null)
				throw new ArgumentNullException("access");
			switch (access.PreferredAccess) {
				case PreferredAccess.ByEntity:
					return access.For(entity).Get(componentType);
				case PreferredAccess.ByComponent:
					return access.For(componentType).Get(entity);
				default:
					throw new InvalidOperationException(
						"Unknown PreferredAccess value");
			}
		}

		/// <summary> Sets the component value of the specified component
		///           type associated with the specified entity in this data
		///           structure, returning the old value, or null if none. </summary>
		public static IComponent Set(
			this IEntityComponentAccess access,
			Entity entity, Type componentType, IComponent component)
		{
			if (access == null)
				throw new ArgumentNullException("access");
			switch (access.PreferredAccess) {
				case PreferredAccess.ByEntity:
					return access.For(entity).Set(componentType, component);
				case PreferredAccess.ByComponent:
					return access.For(componentType).Set(entity, component);
				default:
					throw new InvalidOperationException(
						"Unknown PreferredAccess value");
			}
		}


		/// <summary> Returns if there's a component of the specified componen type
		///           associated with the specified entity in this data structure. </summary>
		public static bool Has(
			this IEntityComponentAccess access, Entity entity, Type componentType)
		{
			return (access.Get(entity, componentType) != null);
		}

		/// <summary> Removes the component of the specified component type
		///           associated with the specified entity in this data
		///           structure, returning the old value, or null if none. </summary>
		public static IComponent Remove(
			this IEntityComponentAccess access,
			Entity entity, Type componentType)
		{
			return access.Set(entity, componentType, null);
		}

		#endregion

		#region Entity related methods

		/// <summary> Returns if the specified entity has any
		///           associated components in this data structure. </summary>
		public static bool Has(this IEntityComponentAccess access, Entity entity)
		{
			return (access.For(entity).Count > 0);
		}

		#endregion
	}
}

