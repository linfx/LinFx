using System;
using System.Threading;

namespace LinFx
{
	/// <summary>
	/// This class can be used to provide an action when
	/// Dipose method is called.
	/// </summary>
	public class DisposeAction : IDisposable
	{
		private Action _action;
		public static readonly DisposeAction Empty = new DisposeAction(null);

		/// <summary>
		/// Creates a new <see cref="DisposeAction"/> object.
		/// </summary>
		/// <param name="action">Action to be executed when this object is disposed.</param>
		public DisposeAction(Action action)
		{
			_action = action;
		}

		public void Dispose()
		{
			// Interlocked prevents multiple execution of the _action.
			var action = Interlocked.Exchange(ref _action, null);
			action?.Invoke();
		}
	}
}
