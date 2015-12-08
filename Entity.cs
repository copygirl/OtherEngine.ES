using System;

namespace OtherEngine.ES
{
	/// <summary> Unique identifier for an entity, which
	///           components may be associated with. </summary>
	public struct Entity : IEquatable<Entity>
	{
		public Guid ID { get; private set; }


		public Entity(Guid id) { ID = id; }


		public static Entity New()
		{
			return new Entity(Guid.NewGuid());
		}


		#region Equality operators

		public static bool operator ==(Entity left, Entity right)
		{
			return (left.ID == right.ID);
		}

		public static bool operator !=(Entity left, Entity right)
		{
			return (left.ID != right.ID);
		}

		#endregion


		#region ToString, Equals and GetHashCode

		public override string ToString()
		{
			return string.Format("[Entity {0}]", ID);
		}

		public override bool Equals(object obj)
		{
			return ((obj is Entity) && Equals((Entity)obj));
		}

		public override int GetHashCode()
		{
			return ID.GetHashCode();
		}

		#endregion

		#region IEquatable<Entity> implementation

		public bool Equals(Entity other)
		{
			return (other.ID == ID);
		}

		#endregion
	}
}

