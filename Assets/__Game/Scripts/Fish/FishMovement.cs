using Assets.__Game.Scripts.EventBus;
using Assets.__Game.Scripts.Tools;
using DG.Tweening;
using UnityEngine;

namespace Assets.__Game.Scripts.Fish
{
  public class FishMovement : MonoBehaviour
  {
    [Range(0, 10)]
    [SerializeField] private float _minMovementDistance = 10f;
    [SerializeField] private float _rotationDuration = 1f;

    private float _movementSpeed;

    private RandomPointInCamera _randomPointInCamera;

    private EventBinding<EventStructs.FishDestroyEvent> _fishDestroyEvent;

    private void Awake()
    {
      _randomPointInCamera = new RandomPointInCamera(Camera.main, _minMovementDistance);
    }

    private void OnEnable()
    {
      _fishDestroyEvent = new EventBinding<EventStructs.FishDestroyEvent>(StopTweeners);
    }

    private void OnDisable()
    {
      _fishDestroyEvent.Remove(StopTweeners);
    }

    private void Start()
    {
      MoveToPoint();
    }

    public void SetParameters(float movementSpeed)
    {
      _movementSpeed = movementSpeed;
    }

    private void MoveToPoint()
    {
      Vector3 point = _randomPointInCamera.GetRandomPointInCamera();

      point.z = 0f;

      float targetYRotation = Mathf.Clamp(Mathf.Atan2(
        point.x - transform.position.x, point.z - transform.position.z) * Mathf.Rad2Deg, -90f, 90f);

      transform.DORotate(new Vector3(
       transform.rotation.x, targetYRotation, transform.rotation.z), _rotationDuration);
      transform.DOMove(point, _movementSpeed)
       .SetEase(Ease.InOutQuad)
       .SetSpeedBased(true)
       .OnComplete(MoveToPoint);
    }

    private void StopTweeners(EventStructs.FishDestroyEvent fishDestroyEvent)
    {
      if (fishDestroyEvent.FishId != transform.GetInstanceID()) return;

      DOTween.Kill(transform);
    }
  }
}