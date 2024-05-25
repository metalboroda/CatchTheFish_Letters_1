using Assets.__Game.Scripts.EventBus;
using Assets.__Game.Scripts.Game.States;
using Assets.__Game.Scripts.Infrastructure;
using UnityEngine;

namespace Assets.__Game.Scripts.LevelItems
{
  public class LevelTimer : MonoBehaviour
  {
    [SerializeField] private int _maxTime;

    private bool _allowTimer = true;
    private float _currentTime;

    private GameBootstrapper _gameBootstrapper;

    private EventBinding<EventStructs.StateChanged> _stateChangedEvent;

    private void Awake()
    {
      _gameBootstrapper = GameBootstrapper.Instance;
    }

    private void OnEnable()
    {
      _stateChangedEvent = new EventBinding<EventStructs.StateChanged>(StopTimer);
    }

    private void OnDisable()
    {
      _stateChangedEvent.Remove(StopTimer);
    }

    void Start()
    {
      _currentTime = _maxTime;
    }

    void Update()
    {
      if (_allowTimer == false) return;
      if (_currentTime > 0)
      {
        _currentTime -= Time.deltaTime;
      }
      else
      {
        _currentTime = 0;

        TimerFinished();
      }

      EventBus<EventStructs.TimerEvent>.Raise(new EventStructs.TimerEvent { Time = (int)_currentTime });
    }

    private void TimerFinished()
    {
      _gameBootstrapper.StateMachine.ChangeState(new GameLoseState(_gameBootstrapper));
    }

    private void StopTimer(EventStructs.StateChanged stateChanged)
    {
      if (stateChanged.State is GameplayState)
        _allowTimer = true;
      else
        _allowTimer = false;
    }
  }
}