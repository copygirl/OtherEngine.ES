
namespace OtherEngine.ES.Timeline
{
	/// <summary> Interface for components which support linear interpolation.
	///           Allows for smoothing out values in-between two keyframes of
	///           the component's timeline. </summary>
	public interface IInterpolatable<T>
	{
		/// <summary> Interpolates between the current and specified component values
		///           and returns the result. Alpha is in the range of [0.0 - 1.0].
		///           An alpha value of 0.0 would return the value of this component,
		///           while a value of 1.0 would return the other component. </summary>
		T Interpolate(T other, float alpha);
	}
}

