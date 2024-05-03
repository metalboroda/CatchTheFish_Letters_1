using UnityEngine;

namespace Assets.__Game.Scripts.SOs
{
  [CreateAssetMenu(fileName = "FishContainer", menuName = "Fish/FishContainer")]
  public class FishContainerSo : ScriptableObject
  {
    [SerializeField] private GameObject[] _fishes;

    public GameObject[] Fishes
    {
      get => _fishes;
      private set => _fishes = value;
    }

    public GameObject GetRandomFish()
    {
      if (_fishes == null || _fishes.Length == 0) return null;

      int randomIndex = Random.Range(0, _fishes.Length);

      return _fishes[randomIndex];
    }
  }
}