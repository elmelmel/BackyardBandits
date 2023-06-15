using System.Collections.Generic;

namespace GameEvents
{

	// Base class for all GameEvents
	public abstract class GameEvent
	{   /// <summary>
		/// each event will be able to test its data.
		/// this way, it isn't important if a sounddesigner or programmer implement/change an event,
		/// as both will be notified in case a misunderstanding / miscommunication occured.
		/// This is a convenient way to optimize maintainability, as the code will tell you what's wrong.
		/// </summary>
		public virtual bool isValid() { return true; }
	}	
	
/*
	To use this thing, first we declare a GameEvent subclass. This event can carry with it all of the parameters needed by the 	objects listening for the event.
	
	public class SomethingHappenedEvent : GameEvent
	{
		// Add event parameters here
	}
	
	Registering to listen for the event looks like this:
	
	public class SomeObject : MonoBehaviour
	{
		void OnEnable ()
		{
			GameEventManager.Instance.AddListener<SomethingHappenedEvent>(OnSomethingHappened);
		}
	
		void OnDisable ()
		{
			GameEventManager.Instance.RemoveListener<SomethingHappenedEvent>(OnSomethingHappened);
		}
	
		void OnSomethingHappened (SomethingHappenedEvent e)
		{
			// Handle event here
		}
	}
	
	And finally, to raise the event, do this:
	
	GameEventManager.Instance.Raise(new SomethingHappenedEvent());

*/

	public class GameEventManager
	{
		private static GameEventManager _instanceInternal = null;

		public static GameEventManager Instance {
			get {
				if (_instanceInternal == null) {
					_instanceInternal = new GameEventManager();
				}
				return _instanceInternal;
			}
		}

		public delegate void EventDelegate<in T>(T e) where T : GameEvent;

		private delegate void EventDelegate(GameEvent e);

		private readonly Dictionary<System.Type, EventDelegate> delegates = new Dictionary<System.Type, EventDelegate>();

		private readonly Dictionary<System.Delegate, EventDelegate> delegateLookup =
			new Dictionary<System.Delegate, EventDelegate>();

		public void AddListener<T>(EventDelegate<T> del) where T : GameEvent {
			// Early-out if we've already registered this delegate
			if (delegateLookup.ContainsKey(del))
				return;

			// Create a new non-generic delegate which calls our generic one.
			// This is the delegate we actually invoke.
			void InternalDelegate(GameEvent e) => del((T) e);
			delegateLookup[del] = InternalDelegate;

			EventDelegate tempDel;
			if (delegates.TryGetValue(typeof(T), out tempDel)) {
				delegates[typeof(T)] = tempDel += InternalDelegate;
			}
			else {
				delegates[typeof(T)] = InternalDelegate;
			}
		}

		public void RemoveListener<T>(EventDelegate<T> del) where T : GameEvent {
			EventDelegate internalDelegate;
			if (delegateLookup.TryGetValue(del, out internalDelegate)) {
				EventDelegate tempDel;
				if (delegates.TryGetValue(typeof(T), out tempDel)) {
					tempDel -= internalDelegate;
					if (tempDel == null) {
						delegates.Remove(typeof(T));
					}
					else {
						delegates[typeof(T)] = tempDel;
					}
				}

				delegateLookup.Remove(del);
			}
		}

		public void Raise(GameEvent e) {
			EventDelegate del;
			if (delegates.TryGetValue(e.GetType(), out del)) {
				del.Invoke(e);
			}
		}
	}

} // namespace