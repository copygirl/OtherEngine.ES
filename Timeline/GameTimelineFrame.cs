using System;
using System.Collections.Generic;
using System.Linq;
using OtherEngine.ES.Interfaces;
using OtherEngine.ES.Utility;

namespace OtherEngine.ES.Timeline
{
	public class GameTimelineFrame : IEntityComponentAccess
	{
		public GameTimeline Timeline { get; private set; }

		public GameTime Time { get; private set; }


		public GameTimelineFrame(GameTimeline timeline, GameTime time)
		{
			Timeline = timeline;
			Time = time;
		}


		#region IEntityComponentAccess implementation

		public IReadOnlyCollection<Entity> Entities { get { return null; } }

		public IReadOnlyCollection<Type> ComponentTypes { get { return Timeline.ComponentTypes; } }

		public PreferredAccess PreferredAccess { get { return PreferredAccess.ByComponent; } }


		public IComponentBag For(Entity entity)
		{
			return new ComponentBagProxy(this, entity);
		}

		public IComponentGrouping<TComponent> For<TComponent>() where TComponent : struct, IComponent
		{
			return new ComponentGrouping<TComponent>(this);
		}

		public IComponentGrouping For(Type componentType)
		{
			ComponentUtility.Validate(componentType);
			var type = typeof(ComponentGrouping<>).MakeGenericType(componentType);
			return (IComponentGrouping)Activator.CreateInstance(type, componentType);
		}

		#endregion

		#region ComponentGrouping class

		class ComponentGrouping<TComponent> : IComponentGrouping<TComponent>
			where TComponent : struct, IComponent
		{
			public GameTimelineFrame Frame { get; private set; }

			public ComponentData<TComponent> Data { get; private set; }


			public GameTime Time { get { return Frame.Time; } }

			public Type ComponentType { get { return typeof(TComponent); } }

			public int Count { get { return Enumerable.Count<EntityComponentPair<TComponent>>(this); } }


			public ComponentGrouping(GameTimelineFrame frame)
			{
				Frame = frame;
				Data = frame.Timeline.GetComponentData<TComponent>();
			}


			public ComponentData<TComponent> GetOrCreateData()
			{
				return (Data ?? (Data = Frame.Timeline.GetComponentData<TComponent>()));
			}

			public TComponent? Get(Entity entity)
			{
				return Data?.Get(entity)?.Get(Time);
			}

			public TComponent? Set(Entity entity, TComponent? value)
			{
				var timeline = GetOrCreateData().GetOrCreate(entity);
				var previous = timeline.Get(Time);
				timeline.Set(Time, value);
				return previous;
			}

			IComponent IComponentGrouping.Get(Entity entity)
			{
				return Get(entity);
			}

			public IComponent Set(Entity entity, IComponent value)
			{
				ComponentUtility.CheckType<TComponent>(value, "value");
				return Set(entity, (TComponent?)value);
			}


			public IEnumerator<EntityComponentPair<TComponent>> GetEnumerator()
			{
				if (Data != null)
					foreach (var entry in Data.Entries) {
						var component = entry.Timeline.Get(Time);
						if (component != null)
							yield return new EntityComponentPair<TComponent>(entry.Entity, component);
					}
			}

			IEnumerator<IEntityComponentPair> IEnumerable<IEntityComponentPair>.GetEnumerator()
			{
				foreach (var pair in this)
					yield return pair;
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

		#endregion
	}
}

