using System;
using System.Collections.Generic;

namespace OtherEngine.ES
{
	/// <summary> Holds information about the value of a component on an entity
	///           at different moments in time, including past and future. This
	///           allows for interpolation and should work well with threading. </summary>
	public class ComponentTimeline<TComponent> : IComponentTimeline
		where TComponent : struct, IComponent
	{
		readonly object _syncWriteLock = new object();


		/// <summary> Gets the first (earliest) keyframe in this timeline. </summary>
		public Keyframe First { get; private set; }

		/// <summary> Gets the last (latest) keyframe in this timeline. </summary>
		public Keyframe Last { get; private set; }

		/// <summary> Gets the total number of keyframes in this timeline. </summary>
		public int Count { get; private set; }


		/// <summary> Gets an enumerable of keyframes in this timeline. </summary>
		public IEnumerable<Keyframe> Keyframes { get {
				for (var frame = First; (frame != null); frame = frame.Next)
					yield return frame;
			} }


		public void Set(GameTime time, TComponent? value)
		{
			lock (_syncWriteLock) {
				var newFrame = new Keyframe(time, value);

				for (var frame = Last; (frame != null); frame = frame.Previous) {
					// If currently checked frame is before new frame,
					// add the new frame in after the current one.
					if (frame.Time < time) {
						newFrame.Next = frame.Next;
						newFrame.Previous = frame;

						if (frame.Next == null)
							Last = newFrame;
						else
							frame.Next.Previous = newFrame;

						frame.Next = newFrame;

						Count++;
						return;
					}

					// If currently checked frame is at the
					// same time as the new frame, replace it.
					if (frame.Time == time) {
						newFrame.Next = frame.Next;
						newFrame.Previous = frame.Previous;

						if (frame.Next == null)
							Last = newFrame;
						else
							frame.Next.Previous = newFrame;

						if (frame.Previous == null)
							First = newFrame;
						else
							frame.Previous.Next = newFrame;
					
						return;
					}
				}

				newFrame.Next = First;

				if (First == null)
					Last = newFrame;
				else
					First.Previous = newFrame;

				First = newFrame;

				Count++;
			}
		}

		public TComponent? Get(GameTime time)
		{
			for (var frame = Last; (frame != null); frame = frame.Previous) {
				if (frame.Time <= time) {
					// If there's a next frame, time is not exactly the time of the
					// current frame, the current and next frames have values and
					// the value is interpolatable, interpolate between the frames!
					if ((frame.Next != null) && (time > frame.Time) &&
					    (frame.Value.HasValue) && (frame.Next.Value.HasValue) &&
					    (frame.Value.Value is IInterpolatable<TComponent>)) {
						var foo = ((IInterpolatable<TComponent>)frame.Value.Value);
						var alpha = (float)(time - frame.Time).Ticks / (frame.Next.Time - frame.Time).Ticks;
						return foo.Interpolate(frame.Next.Value.Value, alpha);
					}
					return frame.Value;
				}
			}
			return null;
		}

		/// <summary> Cleans up all keyframes before the specified time.
		///           Doesn't removes the latest keyframe to make
		///           sure at least one keyframe always remains. </summary>
		public void Cleanup(GameTime until)
		{
			var i = 0;
			for (var frame = First; (frame != null); frame = frame.Next, i++)
				if (frame.Time > until) {
					frame.Previous = null;
					First = frame;
					Count -= i;
					return;
				}
		}


		public class Keyframe : IKeyframe
		{
			public GameTime Time { get; private set; }

			public TComponent? Value { get; private set; }

			IComponent IKeyframe.Value { get { return Value; } }


			public Keyframe Previous { get; internal set; }

			public Keyframe Next { get; internal set; }


			internal Keyframe(GameTime time, TComponent? value)
			{
				Time = time;
				Value = value;
			}
		}


		#region IComponentTimeline implementation

		IEnumerable<IKeyframe> IComponentTimeline.Keyframes { get { return Keyframes; } }

		IComponent IComponentTimeline.Get(GameTime time) { return Get(time); }

		#endregion
	}

	public interface IComponentTimeline
	{
		IEnumerable<IKeyframe> Keyframes { get; }

		IComponent Get(GameTime time);
	}

	public interface IKeyframe
	{
		GameTime Time { get; }

		IComponent Value { get; }
	}
}

