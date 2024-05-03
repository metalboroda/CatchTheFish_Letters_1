using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Assets.__Game.Scripts.Tools
{
  public class LightIntensityHandler : MonoBehaviour
  {
    [SerializeField] private float _minInterval = 0.25f;
    [SerializeField] private float _maxInterval = 0.75f;
    [SerializeField] private float _minIntensity = 0.99f;
    [SerializeField] private float _maxIntensity = 1.1f;

    private Light2D _light2D;

    private void Awake()
    {
      _light2D = GetComponent<Light2D>();
    }

    private void Start()
    {
      StartCoroutine(ChangeIntensityRoutine());
    }

    private IEnumerator ChangeIntensityRoutine()
    {
      while (true)
      {
        float randomInterval = Random.Range(_minInterval, _maxInterval);
        float randomIntensity = Random.Range(_minIntensity, _maxIntensity);
        float currentIntensity = _light2D.intensity;

        float elapsedTime = 0f;

        while (elapsedTime < randomInterval)
        {
          _light2D.intensity = Mathf.Lerp(currentIntensity, randomIntensity, elapsedTime / randomInterval);
          elapsedTime += Time.deltaTime;

          yield return null;
        }
      }
    }
  }
}