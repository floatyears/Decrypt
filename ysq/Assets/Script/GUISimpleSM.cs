using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class GUISimpleSM<TState, TTrigger>
{
	internal abstract class TriggerBehaviour
	{
		private TTrigger mTrigger;

		private Func<bool> mGuard;

		public TTrigger Trigger
		{
			get
			{
				return this.mTrigger;
			}
		}

		public bool IsGuardConditionOk
		{
			get
			{
				return this.mGuard();
			}
		}

		protected TriggerBehaviour(TTrigger trigger, Func<bool> guard)
		{
			this.mTrigger = trigger;
			this.mGuard = guard;
		}

		public abstract bool ResultsInTransitionFrom(TState source, object[] args, out TState destination);
	}

	internal class TransitioningTriggerBehaviour : GUISimpleSM<TState, TTrigger>.TriggerBehaviour
	{
		private TState mDestination;

		public TransitioningTriggerBehaviour(TTrigger trigger, TState destination, Func<bool> guard) : base(trigger, guard)
		{
			this.mDestination = destination;
		}

		public override bool ResultsInTransitionFrom(TState source, object[] args, out TState destination)
		{
			destination = this.mDestination;
			return true;
		}
	}

	internal class IgnoredTriggerBehaviour : GUISimpleSM<TState, TTrigger>.TriggerBehaviour
	{
		public IgnoredTriggerBehaviour(TTrigger trigger, Func<bool> guard) : base(trigger, guard)
		{
		}

		public override bool ResultsInTransitionFrom(TState source, object[] args, out TState destination)
		{
			destination = default(TState);
			return false;
		}
	}

	internal class DynamicTriggerBehaviour : GUISimpleSM<TState, TTrigger>.TriggerBehaviour
	{
		private Func<object[], TState> mDestination;

		public DynamicTriggerBehaviour(TTrigger trigger, Func<object[], TState> destination, Func<bool> guard) : base(trigger, guard)
		{
			this.mDestination = destination;
		}

		public override bool ResultsInTransitionFrom(TState source, object[] args, out TState destination)
		{
			destination = this.mDestination(args);
			return true;
		}
	}

	public class Transition
	{
		private TState mSource;

		private TState mDestination;

		private TTrigger mTrigger;

		public TState Source
		{
			get
			{
				return this.mSource;
			}
		}

		public TState Destination
		{
			get
			{
				return this.mDestination;
			}
		}

		public TTrigger Trigger
		{
			get
			{
				return this.mTrigger;
			}
		}

		public bool IsReentry
		{
			get
			{
				TState source = this.Source;
				return source.Equals(this.Destination);
			}
		}

		public Transition(TState source, TState destination, TTrigger trigger)
		{
			this.mSource = source;
			this.mDestination = destination;
			this.mTrigger = trigger;
		}
	}

	internal class StateRepresentation
	{
		private TState mState;

		private IDictionary<TTrigger, ICollection<GUISimpleSM<TState, TTrigger>.TriggerBehaviour>> mTriggerBehaviours = new Dictionary<TTrigger, ICollection<GUISimpleSM<TState, TTrigger>.TriggerBehaviour>>();

		private ICollection<Action<GUISimpleSM<TState, TTrigger>.Transition, object[]>> mEntryActions = new List<Action<GUISimpleSM<TState, TTrigger>.Transition, object[]>>();

		private ICollection<Action<GUISimpleSM<TState, TTrigger>.Transition>> mExitActions = new List<Action<GUISimpleSM<TState, TTrigger>.Transition>>();

		private GUISimpleSM<TState, TTrigger>.StateRepresentation mSuperState;

		private ICollection<GUISimpleSM<TState, TTrigger>.StateRepresentation> mSubStates = new List<GUISimpleSM<TState, TTrigger>.StateRepresentation>();

		public IEnumerable<TTrigger> PermittedTriggers
		{
			get
			{
				List<TTrigger> list = new List<TTrigger>();
				foreach (KeyValuePair<TTrigger, ICollection<GUISimpleSM<TState, TTrigger>.TriggerBehaviour>> current in this.mTriggerBehaviours)
				{
					foreach (GUISimpleSM<TState, TTrigger>.TriggerBehaviour current2 in current.Value)
					{
						if (current2.IsGuardConditionOk)
						{
							list.Add(current.Key);
						}
					}
				}
				if (this.mSuperState != null)
				{
					foreach (TTrigger current3 in this.mSuperState.PermittedTriggers)
					{
						if (!list.Contains(current3))
						{
							list.Add(current3);
						}
					}
				}
				return list;
			}
		}

		public TState UnderlyingState
		{
			get
			{
				return this.mState;
			}
		}

		public GUISimpleSM<TState, TTrigger>.StateRepresentation Superstate
		{
			get
			{
				return this.mSuperState;
			}
			set
			{
				this.mSuperState = value;
			}
		}

		public StateRepresentation(TState state)
		{
			this.mState = state;
		}

		public bool CanHandle(TTrigger trigger)
		{
			GUISimpleSM<TState, TTrigger>.TriggerBehaviour triggerBehaviour;
			return this.TryFindHandler(trigger, out triggerBehaviour);
		}

		public bool TryFindHandler(TTrigger trigger, out GUISimpleSM<TState, TTrigger>.TriggerBehaviour handler)
		{
			return this.TryFindLocalHandler(trigger, out handler) || (this.mSuperState != null && this.mSuperState.TryFindHandler(trigger, out handler));
		}

		private bool TryFindLocalHandler(TTrigger trigger, out GUISimpleSM<TState, TTrigger>.TriggerBehaviour handler)
		{
			ICollection<GUISimpleSM<TState, TTrigger>.TriggerBehaviour> collection;
			if (!this.mTriggerBehaviours.TryGetValue(trigger, out collection))
			{
				handler = null;
				return false;
			}
			List<GUISimpleSM<TState, TTrigger>.TriggerBehaviour> list = new List<GUISimpleSM<TState, TTrigger>.TriggerBehaviour>();
			foreach (GUISimpleSM<TState, TTrigger>.TriggerBehaviour current in collection)
			{
				if (current.IsGuardConditionOk)
				{
					list.Add(current);
				}
			}
			if (list != null && list.Count > 1)
			{
				Debug.Log(new object[]
				{
					"cant a trigger have muti behaviours."
				});
				handler = null;
				return false;
			}
			handler = ((list.Count != 1) ? null : list[0]);
			return handler != null;
		}

		public void AddEntryAction(TTrigger trigger, Action<GUISimpleSM<TState, TTrigger>.Transition, object[]> action)
		{
			if (action != null)
			{
				this.mEntryActions.Add(delegate(GUISimpleSM<TState, TTrigger>.Transition t, object[] args)
				{
					TTrigger trigger2 = t.Trigger;
					if (trigger2.Equals(trigger))
					{
						action(t, args);
					}
				});
			}
		}

		public void AddEntryAction(Action<GUISimpleSM<TState, TTrigger>.Transition, object[]> action)
		{
			if (action != null)
			{
				this.mEntryActions.Add(action);
			}
		}

		public void AddExitAction(Action<GUISimpleSM<TState, TTrigger>.Transition> action)
		{
			if (action != null)
			{
				this.mExitActions.Add(action);
			}
		}

		public bool IncludeState(TState state)
		{
			if (this.mState.Equals(state))
			{
				return true;
			}
			foreach (GUISimpleSM<TState, TTrigger>.StateRepresentation current in this.mSubStates)
			{
				if (current.IncludeState(state))
				{
					return true;
				}
			}
			return false;
		}

		public bool IsIncludedIn(TState state)
		{
			return this.mState.Equals(state) || (this.mSuperState != null && this.mSuperState.IsIncludedIn(state));
		}

		public void AddSubstate(GUISimpleSM<TState, TTrigger>.StateRepresentation substate)
		{
			if (substate != null)
			{
				this.mSubStates.Add(substate);
			}
		}

		public void AddTriggerBehaviour(GUISimpleSM<TState, TTrigger>.TriggerBehaviour triggerBehaviour)
		{
			ICollection<GUISimpleSM<TState, TTrigger>.TriggerBehaviour> collection;
			if (!this.mTriggerBehaviours.TryGetValue(triggerBehaviour.Trigger, out collection))
			{
				collection = new List<GUISimpleSM<TState, TTrigger>.TriggerBehaviour>();
				this.mTriggerBehaviours.Add(triggerBehaviour.Trigger, collection);
			}
			collection.Add(triggerBehaviour);
		}

		private void ExecuteEntryActions(GUISimpleSM<TState, TTrigger>.Transition transition, object[] entryArgs)
		{
			if (transition != null && entryArgs != null)
			{
				foreach (Action<GUISimpleSM<TState, TTrigger>.Transition, object[]> current in this.mEntryActions)
				{
					current(transition, entryArgs);
				}
			}
		}

		public void Enter(GUISimpleSM<TState, TTrigger>.Transition transition, params object[] entryArgs)
		{
			if (transition != null)
			{
				if (transition.IsReentry)
				{
					this.ExecuteEntryActions(transition, entryArgs);
				}
				else if (!this.IncludeState(transition.Source))
				{
					if (this.mSuperState != null)
					{
						this.mSuperState.Enter(transition, entryArgs);
					}
					this.ExecuteEntryActions(transition, entryArgs);
				}
			}
		}

		private void ExecuteExitActions(GUISimpleSM<TState, TTrigger>.Transition transition)
		{
			if (transition != null)
			{
				foreach (Action<GUISimpleSM<TState, TTrigger>.Transition> current in this.mExitActions)
				{
					current(transition);
				}
			}
		}

		public void Exit(GUISimpleSM<TState, TTrigger>.Transition transition)
		{
			if (transition != null)
			{
				if (transition.IsReentry)
				{
					this.ExecuteExitActions(transition);
				}
				else if (!this.IncludeState(transition.Destination))
				{
					this.ExecuteExitActions(transition);
					if (this.mSuperState != null)
					{
						this.mSuperState.Exit(transition);
					}
				}
			}
		}
	}

	private static class ParameterConversion
	{
		public static object Unpack(object[] args, Type argType, int index)
		{
			if (args != null && index < args.Length)
			{
				object obj = args[index];
				if (obj != null)
				{
					if (argType.IsAssignableFrom(obj.GetType()))
					{
						return obj;
					}
					Debug.Log(new object[]
					{
						"index={0}, arg={1}, argType={2}",
						index,
						obj.GetType(),
						argType
					});
				}
			}
			return null;
		}

		public static TArg Unpack<TArg>(object[] args, int index)
		{
			return (TArg)((object)GUISimpleSM<TState, TTrigger>.ParameterConversion.Unpack(args, typeof(TArg), index));
		}

		public static void Validate(object[] args, Type[] expected)
		{
			if (args.Length <= expected.Length)
			{
				for (int i = 0; i < args.Length; i++)
				{
					GUISimpleSM<TState, TTrigger>.ParameterConversion.Unpack(args, expected[i], i);
				}
			}
		}
	}

	public abstract class TriggerWithParameters
	{
		private TTrigger mUnderlyingTrigger;

		private Type[] mArgumentTypes;

		public TTrigger Trigger
		{
			get
			{
				return this.mUnderlyingTrigger;
			}
		}

		public TriggerWithParameters(TTrigger underlyingTrigger, params Type[] argumentTypes)
		{
			if (argumentTypes != null)
			{
				this.mUnderlyingTrigger = underlyingTrigger;
				this.mArgumentTypes = argumentTypes;
			}
		}

		public void ValidateParameters(object[] args)
		{
			if (args != null)
			{
				GUISimpleSM<TState, TTrigger>.ParameterConversion.Validate(args, this.mArgumentTypes);
			}
		}
	}

	public class TriggerWithParameters<TArg0> : GUISimpleSM<TState, TTrigger>.TriggerWithParameters
	{
		public TriggerWithParameters(TTrigger underlyingTrigger) : base(underlyingTrigger, new Type[]
		{
			typeof(TArg0)
		})
		{
		}
	}

	public class TriggerWithParameters<TArg0, TArg1> : GUISimpleSM<TState, TTrigger>.TriggerWithParameters
	{
		public TriggerWithParameters(TTrigger underlyingTrigger) : base(underlyingTrigger, new Type[]
		{
			typeof(TArg0),
			typeof(TArg1)
		})
		{
		}
	}

	public class TriggerWithParameters<TArg0, TArg1, TArg2> : GUISimpleSM<TState, TTrigger>.TriggerWithParameters
	{
		public TriggerWithParameters(TTrigger underlyingTrigger) : base(underlyingTrigger, new Type[]
		{
			typeof(TArg0),
			typeof(TArg1),
			typeof(TArg2)
		})
		{
		}
	}

	public class StateConfiguration
	{
		private GUISimpleSM<TState, TTrigger>.StateRepresentation mRepresentation;

		private Func<TState, GUISimpleSM<TState, TTrigger>.StateRepresentation> mLookup;

		private static Func<bool> NoGuard = () => true;

		internal StateConfiguration(GUISimpleSM<TState, TTrigger>.StateRepresentation representation, Func<TState, GUISimpleSM<TState, TTrigger>.StateRepresentation> lookup)
		{
			this.mRepresentation = representation;
			this.mLookup = lookup;
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration Permit(TTrigger trigger, TState destinationState)
		{
			this.EnforceNotIdentityTransition(destinationState);
			return this.InternalPermit(trigger, destinationState);
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration PermitIf(TTrigger trigger, TState destinationState, Func<bool> guard)
		{
			this.EnforceNotIdentityTransition(destinationState);
			return this.InternalPermitIf(trigger, destinationState, guard);
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration PermitReentry(TTrigger trigger)
		{
			return this.InternalPermit(trigger, this.mRepresentation.UnderlyingState);
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration PermitReentryIf(TTrigger trigger, Func<bool> guard)
		{
			return this.InternalPermitIf(trigger, this.mRepresentation.UnderlyingState, guard);
		}

		private GUISimpleSM<TState, TTrigger>.StateConfiguration InternalPermit(TTrigger trigger, TState destinationState)
		{
			return this.InternalPermitIf(trigger, destinationState, GUISimpleSM<TState, TTrigger>.StateConfiguration.NoGuard);
		}

		private GUISimpleSM<TState, TTrigger>.StateConfiguration InternalPermitIf(TTrigger trigger, TState destinationState, Func<bool> guard)
		{
			this.mRepresentation.AddTriggerBehaviour(new GUISimpleSM<TState, TTrigger>.TransitioningTriggerBehaviour(trigger, destinationState, guard));
			return this;
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration Ignore(TTrigger trigger)
		{
			return this.IgnoreIf(trigger, GUISimpleSM<TState, TTrigger>.StateConfiguration.NoGuard);
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration IgnoreIf(TTrigger trigger, Func<bool> guard)
		{
			this.mRepresentation.AddTriggerBehaviour(new GUISimpleSM<TState, TTrigger>.IgnoredTriggerBehaviour(trigger, guard));
			return this;
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration OnEntry(Action entryAction)
		{
			return this.OnEntry(delegate(GUISimpleSM<TState, TTrigger>.Transition t)
			{
				entryAction();
			});
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration OnEntry(Action<GUISimpleSM<TState, TTrigger>.Transition> entryAction)
		{
			this.mRepresentation.AddEntryAction(delegate(GUISimpleSM<TState, TTrigger>.Transition t, object[] args)
			{
				entryAction(t);
			});
			return this;
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration OnEntry<TArg0>(Action<TArg0> entryAction)
		{
			return this.OnEntry<TArg0>(delegate(TArg0 a0, GUISimpleSM<TState, TTrigger>.Transition t)
			{
				entryAction(a0);
			});
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration OnEntry<TArg0>(Action<TArg0, GUISimpleSM<TState, TTrigger>.Transition> entryAction)
		{
			this.mRepresentation.AddEntryAction(delegate(GUISimpleSM<TState, TTrigger>.Transition t, object[] args)
			{
				entryAction(GUISimpleSM<TState, TTrigger>.ParameterConversion.Unpack<TArg0>(args, 0), t);
			});
			return this;
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration OnEntry<TArg0, TArg1>(Action<TArg0, TArg1> entryAction)
		{
			return this.OnEntry<TArg0, TArg1>(delegate(TArg0 a0, TArg1 a1, GUISimpleSM<TState, TTrigger>.Transition t)
			{
				entryAction(a0, a1);
			});
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration OnEntry<TArg0, TArg1>(Action<TArg0, TArg1, GUISimpleSM<TState, TTrigger>.Transition> entryAction)
		{
			this.mRepresentation.AddEntryAction(delegate(GUISimpleSM<TState, TTrigger>.Transition t, object[] args)
			{
				entryAction(GUISimpleSM<TState, TTrigger>.ParameterConversion.Unpack<TArg0>(args, 0), GUISimpleSM<TState, TTrigger>.ParameterConversion.Unpack<TArg1>(args, 1), t);
			});
			return this;
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration OnEntry<TArg0, TArg1, TArg2>(Action<TArg0, TArg1, TArg2> entryAction)
		{
			return this.OnEntry<TArg0, TArg1, TArg2>(delegate(TArg0 a0, TArg1 a1, TArg2 a2, GUISimpleSM<TState, TTrigger>.Transition t)
			{
				entryAction(a0, a1, a2);
			});
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration OnEntry<TArg0, TArg1, TArg2>(Action<TArg0, TArg1, TArg2, GUISimpleSM<TState, TTrigger>.Transition> entryAction)
		{
			this.mRepresentation.AddEntryAction(delegate(GUISimpleSM<TState, TTrigger>.Transition t, object[] args)
			{
				entryAction(GUISimpleSM<TState, TTrigger>.ParameterConversion.Unpack<TArg0>(args, 0), GUISimpleSM<TState, TTrigger>.ParameterConversion.Unpack<TArg1>(args, 1), GUISimpleSM<TState, TTrigger>.ParameterConversion.Unpack<TArg2>(args, 2), t);
			});
			return this;
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration OnEntryFrom(TTrigger trigger, Action entryAction)
		{
			return this.OnEntryFrom(trigger, delegate(GUISimpleSM<TState, TTrigger>.Transition t)
			{
				entryAction();
			});
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration OnEntryFrom(TTrigger trigger, Action<GUISimpleSM<TState, TTrigger>.Transition> entryAction)
		{
			this.mRepresentation.AddEntryAction(trigger, delegate(GUISimpleSM<TState, TTrigger>.Transition t, object[] args)
			{
				entryAction(t);
			});
			return this;
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration OnEntryFrom<TArg0>(GUISimpleSM<TState, TTrigger>.TriggerWithParameters<TArg0> trigger, Action<TArg0> entryAction)
		{
			return this.OnEntryFrom<TArg0>(trigger, delegate(TArg0 a0, GUISimpleSM<TState, TTrigger>.Transition t)
			{
				entryAction(a0);
			});
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration OnEntryFrom<TArg0>(GUISimpleSM<TState, TTrigger>.TriggerWithParameters<TArg0> trigger, Action<TArg0, GUISimpleSM<TState, TTrigger>.Transition> entryAction)
		{
			this.mRepresentation.AddEntryAction(trigger.Trigger, delegate(GUISimpleSM<TState, TTrigger>.Transition t, object[] args)
			{
				entryAction(GUISimpleSM<TState, TTrigger>.ParameterConversion.Unpack<TArg0>(args, 0), t);
			});
			return this;
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration OnEntryFrom<TArg0, TArg1>(GUISimpleSM<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1> trigger, Action<TArg0, TArg1> entryAction)
		{
			return this.OnEntryFrom<TArg0, TArg1>(trigger, delegate(TArg0 a0, TArg1 a1, GUISimpleSM<TState, TTrigger>.Transition t)
			{
				entryAction(a0, a1);
			});
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration OnEntryFrom<TArg0, TArg1>(GUISimpleSM<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1> trigger, Action<TArg0, TArg1, GUISimpleSM<TState, TTrigger>.Transition> entryAction)
		{
			this.mRepresentation.AddEntryAction(trigger.Trigger, delegate(GUISimpleSM<TState, TTrigger>.Transition t, object[] args)
			{
				entryAction(GUISimpleSM<TState, TTrigger>.ParameterConversion.Unpack<TArg0>(args, 0), GUISimpleSM<TState, TTrigger>.ParameterConversion.Unpack<TArg1>(args, 1), t);
			});
			return this;
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration OnEntryFrom<TArg0, TArg1, TArg2>(GUISimpleSM<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1, TArg2> trigger, Action<TArg0, TArg1, TArg2> entryAction)
		{
			return this.OnEntryFrom<TArg0, TArg1, TArg2>(trigger, delegate(TArg0 a0, TArg1 a1, TArg2 a2, GUISimpleSM<TState, TTrigger>.Transition t)
			{
				entryAction(a0, a1, a2);
			});
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration OnEntryFrom<TArg0, TArg1, TArg2>(GUISimpleSM<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1, TArg2> trigger, Action<TArg0, TArg1, TArg2, GUISimpleSM<TState, TTrigger>.Transition> entryAction)
		{
			this.mRepresentation.AddEntryAction(trigger.Trigger, delegate(GUISimpleSM<TState, TTrigger>.Transition t, object[] args)
			{
				entryAction(GUISimpleSM<TState, TTrigger>.ParameterConversion.Unpack<TArg0>(args, 0), GUISimpleSM<TState, TTrigger>.ParameterConversion.Unpack<TArg1>(args, 1), GUISimpleSM<TState, TTrigger>.ParameterConversion.Unpack<TArg2>(args, 2), t);
			});
			return this;
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration OnExit(Action exitAction)
		{
			return this.OnExit(delegate(GUISimpleSM<TState, TTrigger>.Transition t)
			{
				exitAction();
			});
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration OnExit(Action<GUISimpleSM<TState, TTrigger>.Transition> exitAction)
		{
			this.mRepresentation.AddExitAction(exitAction);
			return this;
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration SubstateOf(TState superstate)
		{
			GUISimpleSM<TState, TTrigger>.StateRepresentation stateRepresentation = this.mLookup(superstate);
			this.mRepresentation.Superstate = stateRepresentation;
			stateRepresentation.AddSubstate(this.mRepresentation);
			return this;
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration PermitDynamic(TTrigger trigger, Func<TState> destinationStateSelector)
		{
			return this.PermitDynamicIf(trigger, destinationStateSelector, GUISimpleSM<TState, TTrigger>.StateConfiguration.NoGuard);
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration PermitDynamic<TArg0>(GUISimpleSM<TState, TTrigger>.TriggerWithParameters<TArg0> trigger, Func<TArg0, TState> destinationStateSelector)
		{
			return this.PermitDynamicIf<TArg0>(trigger, destinationStateSelector, GUISimpleSM<TState, TTrigger>.StateConfiguration.NoGuard);
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration PermitDynamic<TArg0, TArg1>(GUISimpleSM<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1> trigger, Func<TArg0, TArg1, TState> destinationStateSelector)
		{
			return this.PermitDynamicIf<TArg0, TArg1>(trigger, destinationStateSelector, GUISimpleSM<TState, TTrigger>.StateConfiguration.NoGuard);
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration PermitDynamic<TArg0, TArg1, TArg2>(GUISimpleSM<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1, TArg2> trigger, Func<TArg0, TArg1, TArg2, TState> destinationStateSelector)
		{
			return this.PermitDynamicIf<TArg0, TArg1, TArg2>(trigger, destinationStateSelector, GUISimpleSM<TState, TTrigger>.StateConfiguration.NoGuard);
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration PermitDynamicIf(TTrigger trigger, Func<TState> destinationStateSelector, Func<bool> guard)
		{
			return this.InternalPermitDynamicIf(trigger, (object[] args) => destinationStateSelector(), guard);
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration PermitDynamicIf<TArg0>(GUISimpleSM<TState, TTrigger>.TriggerWithParameters<TArg0> trigger, Func<TArg0, TState> destinationStateSelector, Func<bool> guard)
		{
			return this.InternalPermitDynamicIf(trigger.Trigger, (object[] args) => destinationStateSelector(GUISimpleSM<TState, TTrigger>.ParameterConversion.Unpack<TArg0>(args, 0)), guard);
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration PermitDynamicIf<TArg0, TArg1>(GUISimpleSM<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1> trigger, Func<TArg0, TArg1, TState> destinationStateSelector, Func<bool> guard)
		{
			return this.InternalPermitDynamicIf(trigger.Trigger, (object[] args) => destinationStateSelector(GUISimpleSM<TState, TTrigger>.ParameterConversion.Unpack<TArg0>(args, 0), GUISimpleSM<TState, TTrigger>.ParameterConversion.Unpack<TArg1>(args, 1)), guard);
		}

		public GUISimpleSM<TState, TTrigger>.StateConfiguration PermitDynamicIf<TArg0, TArg1, TArg2>(GUISimpleSM<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1, TArg2> trigger, Func<TArg0, TArg1, TArg2, TState> destinationStateSelector, Func<bool> guard)
		{
			return this.InternalPermitDynamicIf(trigger.Trigger, (object[] args) => destinationStateSelector(GUISimpleSM<TState, TTrigger>.ParameterConversion.Unpack<TArg0>(args, 0), GUISimpleSM<TState, TTrigger>.ParameterConversion.Unpack<TArg1>(args, 1), GUISimpleSM<TState, TTrigger>.ParameterConversion.Unpack<TArg2>(args, 2)), guard);
		}

		private GUISimpleSM<TState, TTrigger>.StateConfiguration InternalPermitDynamic(TTrigger trigger, Func<object[], TState> destinationStateSelector)
		{
			return this.InternalPermitDynamicIf(trigger, destinationStateSelector, GUISimpleSM<TState, TTrigger>.StateConfiguration.NoGuard);
		}

		private GUISimpleSM<TState, TTrigger>.StateConfiguration InternalPermitDynamicIf(TTrigger trigger, Func<object[], TState> destinationStateSelector, Func<bool> guard)
		{
			this.mRepresentation.AddTriggerBehaviour(new GUISimpleSM<TState, TTrigger>.DynamicTriggerBehaviour(trigger, destinationStateSelector, guard));
			return this;
		}

		private void EnforceNotIdentityTransition(TState destination)
		{
			if (destination.Equals(this.mRepresentation.UnderlyingState))
			{
				Debug.LogError(new object[]
				{
					"{0} = {1}",
					destination,
					this.mRepresentation.UnderlyingState
				});
			}
		}
	}

	internal class StateReference
	{
		public TState State
		{
			get;
			set;
		}
	}

	private IDictionary<TState, GUISimpleSM<TState, TTrigger>.StateRepresentation> mStateConfiguration = new Dictionary<TState, GUISimpleSM<TState, TTrigger>.StateRepresentation>();

	private IDictionary<TTrigger, GUISimpleSM<TState, TTrigger>.TriggerWithParameters> mTriggerConfiguration = new Dictionary<TTrigger, GUISimpleSM<TState, TTrigger>.TriggerWithParameters>();

	private Func<TState> mStateAccessor;

	private Action<TState> mStateMutator;

	private Action<TState, TTrigger> mUnhandledTriggerAction = new Action<TState, TTrigger>(GUISimpleSM<TState, TTrigger>.DefaultUnhandledTriggerAction);

    private event Action<GUISimpleSM<TState, TTrigger>.Transition> OnTransitionedEvent;

	public TState State
	{
		get
		{
			return this.mStateAccessor();
		}
		private set
		{
			this.mStateMutator(value);
		}
	}

	public IEnumerable<TTrigger> PermittedTriggers
	{
		get
		{
			return this.CurrentRepresentation.PermittedTriggers;
		}
	}

	private GUISimpleSM<TState, TTrigger>.StateRepresentation CurrentRepresentation
	{
		get
		{
			return this.GetRepresentation(this.State);
		}
	}

	public GUISimpleSM(Func<TState> stateAccessor, Action<TState> stateMutator)
	{
		this.mStateAccessor = stateAccessor;
		this.mStateMutator = stateMutator;
	}

	public GUISimpleSM(TState initialState)
	{
		GUISimpleSM<TState, TTrigger>.StateReference reference = new GUISimpleSM<TState, TTrigger>.StateReference
		{
			State = initialState
		};
		this.mStateAccessor = (() => reference.State);
		this.mStateMutator = delegate(TState s)
		{
			reference.State = s;
		};
	}

	private static void DefaultUnhandledTriggerAction(TState state, TTrigger trigger)
	{
		Debug.Log(new object[]
		{
			"{0} cant trigger {1}",
			state,
			trigger
		});
	}

	private GUISimpleSM<TState, TTrigger>.StateRepresentation GetRepresentation(TState state)
	{
		GUISimpleSM<TState, TTrigger>.StateRepresentation stateRepresentation;
		if (!this.mStateConfiguration.TryGetValue(state, out stateRepresentation))
		{
			stateRepresentation = new GUISimpleSM<TState, TTrigger>.StateRepresentation(state);
			this.mStateConfiguration.Add(state, stateRepresentation);
		}
		return stateRepresentation;
	}

	public GUISimpleSM<TState, TTrigger>.StateConfiguration Configure(TState state)
	{
		return new GUISimpleSM<TState, TTrigger>.StateConfiguration(this.GetRepresentation(state), new Func<TState, GUISimpleSM<TState, TTrigger>.StateRepresentation>(this.GetRepresentation));
	}

	public void Fire(TTrigger trigger)
	{
		this.InternalFire(trigger, new object[0]);
	}

	public void Fire<TArg0>(GUISimpleSM<TState, TTrigger>.TriggerWithParameters<TArg0> trigger, TArg0 arg0)
	{
		this.InternalFire(trigger.Trigger, new object[]
		{
			arg0
		});
	}

	public void Fire<TArg0, TArg1>(GUISimpleSM<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1> trigger, TArg0 arg0, TArg1 arg1)
	{
		this.InternalFire(trigger.Trigger, new object[]
		{
			arg0,
			arg1
		});
	}

	public void Fire<TArg0, TArg1, TArg2>(GUISimpleSM<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1, TArg2> trigger, TArg0 arg0, TArg1 arg1, TArg2 arg2)
	{
		this.InternalFire(trigger.Trigger, new object[]
		{
			arg0,
			arg1,
			arg2
		});
	}

	private void InternalFire(TTrigger trigger, params object[] args)
	{
		GUISimpleSM<TState, TTrigger>.TriggerWithParameters triggerWithParameters;
		if (this.mTriggerConfiguration.TryGetValue(trigger, out triggerWithParameters))
		{
			triggerWithParameters.ValidateParameters(args);
		}
		GUISimpleSM<TState, TTrigger>.TriggerBehaviour triggerBehaviour;
		if (!this.CurrentRepresentation.TryFindHandler(trigger, out triggerBehaviour))
		{
			this.mUnhandledTriggerAction(this.CurrentRepresentation.UnderlyingState, trigger);
			return;
		}
		TState state = this.State;
		TState destination;
		if (triggerBehaviour.ResultsInTransitionFrom(state, args, out destination))
		{
			GUISimpleSM<TState, TTrigger>.Transition transition = new GUISimpleSM<TState, TTrigger>.Transition(state, destination, trigger);
			this.CurrentRepresentation.Exit(transition);
			this.State = transition.Destination;
			Action<GUISimpleSM<TState, TTrigger>.Transition> onTransitionedEvent = this.OnTransitionedEvent;
			if (onTransitionedEvent != null)
			{
				onTransitionedEvent(transition);
			}
			this.CurrentRepresentation.Enter(transition, args);
		}
	}

	public void OnUnhandledTrigger(Action<TState, TTrigger> unhandledTriggerAction)
	{
		if (unhandledTriggerAction != null)
		{
			this.mUnhandledTriggerAction = unhandledTriggerAction;
		}
	}

	public bool IsInState(TState state)
	{
		return this.CurrentRepresentation.IsIncludedIn(state);
	}

	public bool CanFire(TTrigger trigger)
	{
		return this.CurrentRepresentation.CanHandle(trigger);
	}

	public override string ToString()
	{
		List<string> list = new List<string>();
		foreach (TTrigger current in this.PermittedTriggers)
		{
			list.Add(current.ToString());
		}
		return string.Format("StateMachine {{ State = {0}, PermittedTriggers = {{ {1} }}}}", this.State, string.Join(", ", list.ToArray()));
	}

	public GUISimpleSM<TState, TTrigger>.TriggerWithParameters<TArg0> SetTriggerParameters<TArg0>(TTrigger trigger)
	{
		GUISimpleSM<TState, TTrigger>.TriggerWithParameters<TArg0> triggerWithParameters = new GUISimpleSM<TState, TTrigger>.TriggerWithParameters<TArg0>(trigger);
		this.SaveTriggerConfiguration(triggerWithParameters);
		return triggerWithParameters;
	}

	public GUISimpleSM<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1> SetTriggerParameters<TArg0, TArg1>(TTrigger trigger)
	{
		GUISimpleSM<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1> triggerWithParameters = new GUISimpleSM<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1>(trigger);
		this.SaveTriggerConfiguration(triggerWithParameters);
		return triggerWithParameters;
	}

	public GUISimpleSM<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1, TArg2> SetTriggerParameters<TArg0, TArg1, TArg2>(TTrigger trigger)
	{
		GUISimpleSM<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1, TArg2> triggerWithParameters = new GUISimpleSM<TState, TTrigger>.TriggerWithParameters<TArg0, TArg1, TArg2>(trigger);
		this.SaveTriggerConfiguration(triggerWithParameters);
		return triggerWithParameters;
	}

	private void SaveTriggerConfiguration(GUISimpleSM<TState, TTrigger>.TriggerWithParameters trigger)
	{
		if (!this.mTriggerConfiguration.ContainsKey(trigger.Trigger))
		{
			this.mTriggerConfiguration.Add(trigger.Trigger, trigger);
		}
	}

	public void OnTransitioned(Action<GUISimpleSM<TState, TTrigger>.Transition> onTransitionAction)
	{
		if (onTransitionAction == null)
		{
			throw new ArgumentNullException("onTransitionAction");
		}
		this.OnTransitionedEvent = (Action<GUISimpleSM<TState, TTrigger>.Transition>)Delegate.Combine(this.OnTransitionedEvent, onTransitionAction);
	}
}
