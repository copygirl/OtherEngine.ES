using System;
using System.Linq;

namespace OtherEngine.ES.Interfaces
{
	/// <summary> Proxy class implementation of IComponentBag for
	///           IEntityComponentAccess classes that uses its
	///           IComponentGrouping implementation to manage access. </summary>
	public class ComponentBagProxy : IComponentBag
	{
		public IEntityComponentAccess Access { get; private set; }

		public Entity Entity { get; private set; }


		public ComponentBagProxy(IEntityComponentAccess access, Entity entity)
		{
			Access = access;
			Entity = entity;
		}


		#region IComponentBag implementation

		public int Count { get { return Access.ComponentTypes.Count(
			type => (Access.For(type).Count > 0)); } }


		public TComponent? Get<TComponent>()
			where TComponent : struct, IComponent
		{
			return Access.For<TComponent>().Get(Entity);
		}
		public TComponent? Set<TComponent>(TComponent? component)
			where TComponent : struct, IComponent
		{
			return Access.For<TComponent>().Set(Entity, component);
		}

		public IComponent Get(Type componentType)
		{
			return Access.For(componentType).Get(Entity);
		}
		public IComponent Set(Type componentType, IComponent component)
		{
			return Access.For(componentType).Set(Entity, component);
		}


		public System.Collections.Generic.IEnumerator<IComponent> GetEnumerator()
		{
			foreach (var type in Access.ComponentTypes) {
				var component = Access.For(type).Get(Entity);
				if (component != null)
					yield return component;
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}

