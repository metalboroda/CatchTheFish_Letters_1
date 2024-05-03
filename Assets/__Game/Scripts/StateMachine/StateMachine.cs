using Assets.__Game.Scripts.EventBus;
using Assets.__Game.Scripts.StateMachine;
using System.Collections;
using UnityEngine;
using static Assets.__Game.Scripts.EventBus.EventStructs;

namespace Assets.__Game.Scripts.Infrastructure
{
  public class StateMachine
  {
    public State CurrentState { get; private set; }
    public State PreviousState { get; private set; }

    public void Init(State initState)
    {
      CurrentState = initState;
      CurrentState.Enter();

      EventBus<StateChanged>.Raise(new StateChanged { State = CurrentState });
    }

    public void ChangeState(State newState)
    {
      if (newState == CurrentState) return;

      PreviousState = CurrentState;
      CurrentState.Exit();
      CurrentState = newState;
      CurrentState.Enter();

      EventBus<StateChanged>.Raise(new StateChanged { State = CurrentState });
    }

    public void ChangeStateWithDelay(State newState, float delay, MonoBehaviour monoBehaviour)
    {
      monoBehaviour.StartCoroutine(DoChangeStateWithDelay(newState, delay));
    }

    private IEnumerator DoChangeStateWithDelay(State newState, float delay)
    {
      yield return new WaitForSeconds(delay);

      ChangeState(newState);
    }
  }
}