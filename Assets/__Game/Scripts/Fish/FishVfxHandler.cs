using Assets.__Game.Scripts.Effects;
using Assets.__Game.Scripts.EventBus;
using Lean.Pool;
using UnityEngine;

namespace Assets.__Game.Scripts.Fish
{
  public class FishVfxHandler : MonoBehaviour
  {
    [SerializeField] private GameObject _bubblesParticlesPrefab;
    [SerializeField] private GameObject _starPrefab;
    [SerializeField] private GameObject _angryFaceParticlesPrefab;

    private EventBinding<EventStructs.FishDestroyEvent> _fishDestroyEvent;

    private void OnEnable()
    {
      _fishDestroyEvent = new EventBinding<EventStructs.FishDestroyEvent>(SpawnDestroyParticles);
    }

    private void OnDisable()
    {
      _fishDestroyEvent.Remove(SpawnDestroyParticles);
    }

    private void SpawnDestroyParticles(EventStructs.FishDestroyEvent fishDestroyEvent)
    {
      if (fishDestroyEvent.FishId != transform.GetInstanceID()) return;

      SpawnParticle(fishDestroyEvent.Correct ? _starPrefab : _angryFaceParticlesPrefab);
      SpawnParticle(_bubblesParticlesPrefab);
    }

    private void SpawnParticle(GameObject prefab)
    {
      GameObject spawnedParticle = LeanPool.Spawn(prefab, transform.position, Quaternion.identity);
      ParticleDestroyer particleDestroyer = spawnedParticle.GetComponent<ParticleDestroyer>();

      particleDestroyer.DestroyParticle();
    }
  }
}