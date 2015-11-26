
namespace OtherEngine.ES
{
	/// <summary>
	///   Interface for component types. Components are structs which
	///   are associated with an entity and hold a small amount of data.
	///   Empty components may be used as "flags".
	///   Implementing types' names must end with "Component".
	///   
	///   Example component types:
	///     Position, Name, Target, Inventory, AI, Input,
	///     Player, Item, World, Chunk, Game Window, ...
	/// </summary>
	public interface IComponent
	{
	}
}

