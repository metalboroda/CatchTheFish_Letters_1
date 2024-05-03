using UnityEngine;

namespace Assets.__Game.Scripts.Tools
{
  public class LockRotation : MonoBehaviour
  {
    void LateUpdate()
    {
      transform.rotation = Quaternion.Euler(0, 0, 0);
    }
  }
}