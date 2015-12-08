
namespace OtherEngine.ES.Interfaces
{
	public interface IEntityComponentPair
	{
		Entity Entity { get; }

		IComponent Component { get; }
	}

	public struct EntityComponentPair<TComponent> : IEntityComponentPair
		where TComponent : struct, IComponent
	{
		public Entity Entity { get; private set; }

		public TComponent Component { get; private set; }

		IComponent IEntityComponentPair.Component { get { return Component; } }


		public EntityComponentPair(Entity entity, TComponent component)
		{
			Entity = entity;
			Component = component;
		}
	}
}

