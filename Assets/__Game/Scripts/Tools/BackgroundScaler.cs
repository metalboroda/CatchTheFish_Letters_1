using UnityEngine;

namespace Assets.__Game.Scripts.Tools
{
  public class BackgroundScaler : MonoBehaviour
  {
    [SerializeField] private float _zPosition = 5f;

    private SpriteRenderer _spriteRenderer;
    private Camera _mainCamera;

    private void Awake()
    {
      _spriteRenderer = GetComponent<SpriteRenderer>();

      if (_spriteRenderer == null)
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

      _mainCamera = Camera.main;
    }

    void Start()
    {
      ScaleBackground();
    }

    private void ScaleBackground()
    {
      if (_spriteRenderer == null || _mainCamera == null) return;

      float spriteWidth = _spriteRenderer.sprite.bounds.size.x;
      float spriteHeight = _spriteRenderer.sprite.bounds.size.y;
      float cameraHeight = 2f * _mainCamera.orthographicSize;
      float cameraWidth = cameraHeight * _mainCamera.aspect;

      float scaleX = cameraWidth / spriteWidth;
      float scaleY = cameraHeight / spriteHeight;
      float scale = Mathf.Max(scaleX, scaleY);

      transform.localScale = new Vector3(scale, scale, 1f);
      transform.position = new Vector3(transform.position.x, transform.position.y, _zPosition);
    }
  }
}