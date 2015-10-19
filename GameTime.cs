using System;
using System.Text;

namespace OtherEngine.ES
{
	/// <summary> Represents a specific absolute or
	///           relative time in the GameTimeline. </summary>
	public struct GameTime : IEquatable<GameTime>, IComparable<GameTime>
	{
		public const long TicksPerSecond = 1000;

		public static readonly GameTime Zero = new GameTime(0);
		public static readonly GameTime MinValue = new GameTime(long.MinValue);
		public static readonly GameTime MaxValue = new GameTime(long.MaxValue);


		/// <summary> Gets the number of ticks in this GameTime. </summary>
		public long Ticks { get; private set; }

		/// <summary> Gets the total number of seconds this GameTime represents. </summary>
		public double Seconds { get { return (Ticks / (double)TicksPerSecond); } }


		#region Constructor and static constructor methods

		public GameTime(long ticks) { Ticks = ticks; }


		public static GameTime FromSeconds(double seconds) {
			return new GameTime((long)(seconds * TicksPerSecond)); }

		public static GameTime FromMinutes(double minutes) {
			return FromSeconds(minutes / 60); }

		public static GameTime FromHours(double hours) {
			return FromSeconds(hours / 3600); }

		#endregion


		#region Comparison / equality operators

		public static bool operator >(GameTime left, GameTime right)
		{
			return (left.Ticks > right.Ticks);
		}

		public static bool operator <(GameTime left, GameTime right)
		{
			return (left.Ticks < right.Ticks);
		}


		public static bool operator >=(GameTime left, GameTime right)
		{
			return (left.Ticks >= right.Ticks);
		}

		public static bool operator <=(GameTime left, GameTime right)
		{
			return (left.Ticks <= right.Ticks);
		}


		public static bool operator ==(GameTime left, GameTime right)
		{
			return (left.Ticks == right.Ticks);
		}

		public static bool operator !=(GameTime left, GameTime right)
		{
			return (left.Ticks != right.Ticks);
		}

		#endregion

		#region Arithmetic operators

		public static GameTime operator +(GameTime left, GameTime right)
		{
			return new GameTime(left.Ticks + right.Ticks);
		}

		public static GameTime operator -(GameTime left, GameTime right)
		{
			return new GameTime(left.Ticks - right.Ticks);
		}

		#endregion

		#region Casting to / from TimeSpan

		public static explicit operator GameTime(TimeSpan time)
		{
			return new GameTime(time.Ticks * TicksPerSecond / TimeSpan.TicksPerSecond);
		}

		public static explicit operator TimeSpan(GameTime time)
		{
			return new TimeSpan(time.Ticks * TimeSpan.TicksPerSecond / TicksPerSecond);
		}

		#endregion


		#region ToString, Equals and GetHashCode

		public override string ToString()
		{
			var h = Ticks / (TicksPerSecond * 60 * 60);
			var m = (Ticks / (TicksPerSecond * 60)) % (24 * 60);
			var s = (Ticks / TicksPerSecond) % (24 * 60 * 60);
			var ms = (Ticks * 1000 / TicksPerSecond) % 1000;

			var sb = new StringBuilder('[');
			sb.AppendFormat("{0}:{1:00}:{2:00}", h, m, s);
			if (ms != 0) sb.AppendFormat(".{0:000}", ms);
			return sb.Append(']').ToString();
		}

		public override bool Equals(object obj)
		{
			return ((obj is GameTime) && Equals((GameTime)obj));
		}

		public override int GetHashCode()
		{
			return Ticks.GetHashCode();
		}

		#endregion

		#region IEquatable<GameTime> implementation

		public bool Equals(GameTime other)
		{
			return (other.Ticks == Ticks);
		}

		#endregion

		#region IComparable<GameTime> implementation

		public int CompareTo(GameTime other)
		{
			return (int)(Ticks - other.Ticks);
		}

		#endregion
	}
}

