using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using OtherEngine.ES.Utility;
using OtherEngine.Utility;

namespace OtherEngine.ES.Timeline
{
	// Heavily inspired by the "curves" approach from Planetary Annihilation:
	// http://forrestthewoods.com/the-tech-of-planetary-annihilation-chronocam/

	/// <summary> Entity container which stores game information in a timeline data structure.
	///           Using it, components may be associated with entities at specific times. </summary>
	public class GameTimeline
	{
		readonly ConcurrentDictionary<Type, IComponentData> _data =
			new ConcurrentDictionary<Type, IComponentData>();


		public IReadOnlyCollection<Type> ComponentTypes { get; private set; }


		public GameTimeline()
		{
			ComponentTypes = _data.Keys.AsReadOnly();
		}


		/// <summary> Returns an implementation of IEntityComponentAccess
		///           that allows access to components at the specified time. </summary>
		public GameTimelineFrame At(GameTime time)
		{
			return new GameTimelineFrame(this, time);
		}


		#region Getting ComponentData

		/// <summary> Returns the ComponentData for the specified component type, null if none. </summary>
		/// <exception cref="ArgumentException"> Thrown if componentType is not a valid component type. </exception>
		public IComponentData GetComponentData(Type componentType)
		{
			ComponentUtility.Validate(componentType);
			return GetComponentDataInternal(componentType);
		}

		/// <summary> Returns the ComponentData for type TComponent, null if none. </summary>
		public ComponentData<TComponent> GetComponentData<TComponent>()
			where TComponent : struct, IComponent
		{
			return (ComponentData<TComponent>)GetComponentDataInternal(typeof(TComponent));
		}

		/// <summary> Returns the ComponentData for type TComponent, creating it if necessary. </summary>
		public ComponentData<TComponent> GetOrCreateComponentData<TComponent>()
			where TComponent : struct, IComponent
		{
			return (ComponentData<TComponent>)_data.GetOrAdd(
				typeof(TComponent), type => new ComponentData<TComponent>());
		}


		IComponentData GetComponentDataInternal(Type componentType)
		{
			IComponentData data;
			return (_data.TryGetValue(componentType, out data) ? data : null);
		}

		#endregion

		#region Getting ComponentTimeline

		/// <summary> Returns the ComponentTimeline for the specified entity and component type, or null if none. </summary>
		/// <exception cref="ArgumentException"> Thrown if componentType is not a valid component type. </exception>
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


		/// <summary> Sets the component value for type TComponent
		///           on the specified entity at the specified time. </summary>
		public void Set<TComponent>(Entity entity, GameTime time, TComponent value)
			where TComponent : struct, IComponent
		{
			Set(entity, time, (TComponent?)value);
		}

		/// <summary> Removes the component value for type TComponent
		///           on the specified entity at the specified time. </summary>
		public void Remove<TComponent>(Entity entity, GameTime time)
			where TComponent : struct, IComponent
		{
			Set(entity, time, (TComponent?)null);
		}

		#endregion

		#region Entity related

		/// <summary> Returns all ComponentTimelines that contain
		///           information about the specified entity. </summary>
		public IEnumerable<IComponentTimeline> GetAllTimelines(Entity entity)
		{
			return _data.Keys
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

