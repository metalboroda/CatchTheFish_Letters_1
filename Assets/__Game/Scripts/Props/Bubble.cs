using Lean.Pool;
using UnityEngine;

namespace Assets.__Game.Scripts.Props
{
  public class Bubble : MonoBehaviour
  {
    private float _movementSpeed = 5f;
    private float _destroyTime = 10f;

    private void Update()
    {
      MoveUp();
    }

    public void SetupBubble(float movementSpeed, float destroyTime)
    {
      _movementSpeed = movementSpeed;
      _destroyTime = destroyTime;

      LeanPool.Despawn(gameObject, destroyTime);
    }

    private void MoveUp()
    {
      transform.Translate(Vector3.up * _movementSpeed * Time.deltaTime, Space.World);
    }
  }
}