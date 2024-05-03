using UnityEngine;

namespace Assets.__Game.Scripts.Setup
{
  public class ScreenSetup : MonoBehaviour
  {
    void Awake()
    {
      Application.targetFrameRate = 120;
    }
  }
}