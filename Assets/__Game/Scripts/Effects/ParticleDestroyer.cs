using Lean.Pool;
using UnityEngine;

namespace Assets.__Game.Scripts.Effects
{
  public class ParticleDestroyer : MonoBehaviour
  {
    [SerializeField] private float _destroyTime = 10;

    public void DestroyParticle()
    {
      LeanPool.Despawn(gameObject, _destroyTime);
    }
  }
}