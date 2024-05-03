using UnityEngine;

namespace Assets.__Game.Scripts.Tools
{
  public class RandomPointInCamera
  {
    private float _minDistance;
    private Camera _mainCamera;

    public RandomPointInCamera(Camera camera, float minDistance = 10f)
    {
      _minDistance = minDistance;
      _mainCamera = camera;
    }

    public Vector3 GetRandomPointInCamera()
    {
      Vector3 randomPoint;
      Vector3 prevPoint = Vector3.zero;
      float distance;

      do
      {
        randomPoint = GetRandomPoint();
        distance = Vector3.Distance(randomPoint, prevPoint);
      } while (distance < _minDistance);

      return randomPoint;
    }

    private Vector3 GetRandomPoint()
    {
      float randomX = Random.Range(0.1f, 0.9f);
      float randomY = Random.Range(0.1f, 0.9f);
      Vector3 viewportPoint = new Vector3(randomX, randomY, 0f);

      return _mainCamera.ViewportToWorldPoint(viewportPoint);
    }
  }
}