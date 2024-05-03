using UnityEngine;

namespace Assets.__Game.Scripts.Fish
{
  public class FishController : MonoBehaviour
  {
    [SerializeField] private FishHandler _fishHandler;
    [SerializeField] private FishUiHandler _fishUiHandler;

    public FishHandler FishHandler
    {
      get => _fishHandler;
      private set => _fishHandler = value;
    }

    public FishUiHandler FishUiHandler
    {
      get => _fishUiHandler;
      private set => _fishUiHandler = value;
    }
  }
}