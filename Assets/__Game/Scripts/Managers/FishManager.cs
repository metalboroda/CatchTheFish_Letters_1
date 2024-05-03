using Assets.__Game.Scripts.EventBus;
using Assets.__Game.Scripts.Fish;
using System.Collections.Generic;
using Assets.__Game.Scripts.Game.States;
using Assets.__Game.Scripts.Infrastructure;
using Assets.__Game.Scripts.SOs;
using UnityEngine;

namespace Assets.__Game.Scripts.Managers
{
  public class FishManager : MonoBehaviour
  {
    [SerializeField] private CorrectValuesContainerSo _correctNumbersContainerSo;

    private List<FishHandler> _correctNumberFish = new();
    private List<FishHandler> _incorrectNumberFish = new();

    private EventBinding<EventStructs.FishSpawnerEvent> _fishSpawnerEvent;
    private EventBinding<EventStructs.FishClickEvent> _fishClickEvent;

    private GameBootstrapper _gameBootstrapper;

    private void Awake()
    {
      _gameBootstrapper = GameBootstrapper.Instance;
    }

    private void OnEnable()
    {
      _fishSpawnerEvent = new EventBinding<EventStructs.FishSpawnerEvent>(AddFishesToList);
      _fishClickEvent = new EventBinding<EventStructs.FishClickEvent>(ReceiveFish);
    }

    private void OnDisable()
    {
      _fishSpawnerEvent.Remove(AddFishesToList);
      _fishClickEvent.Remove(ReceiveFish);
    }

    private void Start()
    {
      EventBus<EventStructs.FishReceivedEvent>.Raise(new EventStructs.FishReceivedEvent
      {
        CorrectValues = _correctNumbersContainerSo.CorrectValues
      });
    }

    private void AddFishesToList(EventStructs.FishSpawnerEvent fishSpawnerEvent)
    {
      _correctNumberFish.AddRange(fishSpawnerEvent.CorrectFishHandlers);
      _incorrectNumberFish.AddRange(fishSpawnerEvent.IncorrectFinishHandlers);
    }

    private void ReceiveFish(EventStructs.FishClickEvent fishClickEvent)
    {
      foreach (var number in _correctNumbersContainerSo.CorrectValues)
      {
        if (_correctNumberFish.Contains(fishClickEvent.FishHandler))
        {
          _correctNumberFish.Remove(fishClickEvent.FishHandler);
          fishClickEvent.FishHandler.DestroyFish(true);

          EventBus<EventStructs.FishReceivedEvent>.Raise(new EventStructs.FishReceivedEvent
          {
            CorrectFish = true,
            CorrectFishIncrement = 1
          });

          break;
        }

        if (_incorrectNumberFish.Contains(fishClickEvent.FishHandler))
        {
          _incorrectNumberFish.Remove(fishClickEvent.FishHandler);
          fishClickEvent.FishHandler.DestroyFish(false);

          EventBus<EventStructs.FishReceivedEvent>.Raise(new EventStructs.FishReceivedEvent
          {
            CorrectFish = false,
            IncorrectFishIncrement = 1
          });

          break;
        }
      }

      CheckFishLists();
    }

    private void CheckFishLists()
    {
      if(_gameBootstrapper == null) return;

      if (_correctNumberFish.Count == 0)
        _gameBootstrapper.StateMachine.ChangeState(new GameWinState(_gameBootstrapper));

      if (_incorrectNumberFish.Count == 0)
        _gameBootstrapper.StateMachine.ChangeState(new GameLoseState(_gameBootstrapper));
    }
  }
}