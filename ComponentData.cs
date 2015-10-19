using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace OtherEngine.ES
{
	/// <summary> Holds information on a specific component type in the GameTimeline.
	///           This includes the ComponentTimelines for entities that own this component. </summary>
	public class ComponentData<TComponent> : IComponentData
		where TComponent : struct, IComponent
	{
		readonly ConcurrentDictionary<Entity, ComponentTimeline<TComponent>> _data =
			new ConcurrentDictionary<Entity, ComponentTimeline<TComponent>>();


		/// <summary> Gets an enumerable of entries containing the
		///           entity and its timeline for type TComponent. </summary>
		public IEnumerable<Entry> Entries { get {
				return _data.Select(kvp => new Entry(kvp.Key, kvp.Value)); } }


		internal ComponentData() {  }


		/// <summary> Gets the ComponentTimeline for type TComponent
		///           on the specified entity, or null if none. </summary>
		public ComponentTimeline<TComponent> Get(Entity entity)
		{
			ComponentTimeline<TComponent> timeline;
			return (_data.TryGetValue(entity, out timeline) ? timeline : null);
		}

		/// <summary> Gets the ComponentTimeline for type TComponent
		///           on the specified entity, creating it if necessary. </summary>
		public ComponentTimeline<TComponent> GetOrCreate(Entity entity)
		{
			return _data.GetOrAdd(entity, _ => new ComponentTimeline<TComponent>());
		}


		#region IComponentData implementation

		IEnumerable<IComponentDataEntry> IComponentData.Entries { get {
				return Entries.Cast<IComponentDataEntry>(); } }

		IComponentTimeline IComponentData.Get(Entity entity) { return Get(entity); }

		#endregion

		#region Entry class definition

		public struct Entry : IComponentDataEntry
		{
			public Entity Entity { get; private set; }

			public ComponentTimeline<TComponent> Timeline { get; private set; }

			IComponentTimeline IComponentDataEntry.Timeline { get { return Timeline; } }


			public Entry(Entity entity, ComponentTimeline<TComponent> timeline)
			{
				Entity = entity;
				Timeline = timeline;
			}
		}

		#endregion
	}

	public interface IComponentData
	{
		IEnumerable<IComponentDataEntry> Entries { get; }

		IComponentTimeline Get(Entity entity);
	}

	public interface IComponentDataEntry
	{
		Entity Entity { get; }

		IComponentTimeline Timeline { get; }
	}
}

