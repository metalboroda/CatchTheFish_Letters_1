using Assets.__Game.Scripts.EventBus;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.__Game.Scripts.Fish
{
  public class FishHandler : MonoBehaviour, IPointerClickHandler
  {
    private string _fishValue;

    public string FishValue
    {
      get => _fishValue;
      private set => _fishValue = value;
    }

    public void SetFishNumber(string fishValue, bool correct, bool tutorial = false)
    {
      _fishValue = fishValue;

      EventBus<EventStructs.FishUiEvent>.Raise(new EventStructs.FishUiEvent
      {
        FishId = transform.GetInstanceID(),
        FishValue = _fishValue,
        Correct = correct,
        Tutorial = tutorial
      });
    }

    public void OnPointerClick(PointerEventData eventData)
    {
      EventBus<EventStructs.FishClickEvent>.Raise(new EventStructs.FishClickEvent
      {
        FishHandler = this,
        FishValue = _fishValue
      });
    }

    public void DestroyFish(bool correct)
    {
      EventBus<EventStructs.FishDestroyEvent>.Raise(new EventStructs.FishDestroyEvent
      {
        FishId = transform.GetInstanceID(),
        Correct = correct
      });

      Destroy(gameObject);
    }
  }
}