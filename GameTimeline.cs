using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace OtherEngine.ES
{
	// Heavily inspired by the "curves" approach from Planetary Annihilation:
	// http://forrestthewoods.com/the-tech-of-planetary-annihilation-chronocam/

	/// <summary> Main class which stores game information in a timeline data structure.
	///           Using it, components may be associated with entities at specified times. </summary>
	public class GameTimeline
	{
		readonly ConcurrentDictionary<Type, IComponentData> _data =
			new ConcurrentDictionary<Type, IComponentData>();

		readonly ConcurrentBag<Type> _componentTypes = new ConcurrentBag<Type>();


		#region Getting ComponentData

		/// <summary> Returns the ComponentData for the specified component type, null if none. </summary>
		public IComponentData GetComponentData(Type componentType)
		{
			IComponentData data;
			return (_data.TryGetValue(componentType, out data) ? data : null);
		}

		/// <summary> Returns the ComponentData for type TComponent, null if none. </summary>
		public ComponentData<TComponent> GetComponentData<TComponent>()
			where TComponent : struct, IComponent
		{
			return (ComponentData<TComponent>)GetComponentData(typeof(TComponent));
		}

		/// <summary> Returns the ComponentData for type TComponent, creating it if necessary. </summary>
		public ComponentData<TComponent> GetOrCreateComponentData<TComponent>()
			where TComponent : struct, IComponent
		{
			return (ComponentData<TComponent>)_data.GetOrAdd(
				typeof(TComponent), CreateComponentData<TComponent>);
		}


		/// <summary> Creates a ComponentData for type TComponent and
		///           adds the type to the collection of component types. </summary>
		ComponentData<TComponent> CreateComponentData<TComponent>(Type componentType)
			where TComponent : struct, IComponent
		{
			_componentTypes.Add(componentType);
			return new ComponentData<TComponent>();
		}

		#endregion

		#region Getting ComponentTimeline

		/// <summary> Returns the ComponentTimeline for the specified
		///           entity and component type, or null if none. </summary>
		public IComponentTimeline GetTimeline(Entity entity, Type componentType)
		{
			return GetComponentData(componentType)?.Get(entity);
		}

		/// <summary> Returns the ComponentTimeline for the specified
		///           entity and type TComponent, or null if none. </summary>
		public ComponentTimeline<TComponent> GetTimeline<TComponent>(Entity entity)
			where TComponent : struct, IComponent
		{
			return GetComponentData<TComponent>()?.Get(entity);
		}

		/// <summary> Returns the ComponentTimeline for the specified
		///           entity and type TComponent, creating it if necessary. </summary>
		public ComponentTimeline<TComponent> GetOrCreateTimeline<TComponent>(Entity entity)
			where TComponent : struct, IComponent
		{
			return GetOrCreateComponentData<TComponent>().GetOrCreate(entity);
		}

		#endregion

		#region Getting / setting components

		/// <summary> Returns the component value for type TComponent on the
		///           specified entity at the specified time, null if none. </summary>
		public TComponent? Get<TComponent>(Entity entity, GameTime time)
			where TComponent : struct, IComponent
		{
			return GetTimeline<TComponent>(entity)?.Get(time);
		}

		/// <summary> Sets the component value for type TComponent
		///           on the specified entity at the specified time. </summary>
		public void Set<TComponent>(Entity entity, GameTime time, TComponent? value)
			where TComponent : struct, IComponent
		{
			GetOrCreateTimeline<TComponent>(entity)?.Set(time, value);
		}

		#endregion

		#region Entity related

		/// <summary> Returns all ComponentTimelines that contain
		///           information about the specified entity. </summary>
		public IEnumerable<IComponentTimeline> GetAllTimelines(Entity entity)
		{
			return _componentTypes
				.Select(type => GetTimeline(entity, type))
				.Where(timeline => (timeline != null));
		}

		/// <summary> Returns all components on the entity at the specified time. </summary>
		public IEnumerable<IComponent> GetAllComponents(Entity entity, GameTime time)
		{
			return GetAllTimelines(entity)
				.Select(timeline => timeline.Get(time))
				.Where(component => (component != null));
		}

		#endregion
	}
}

