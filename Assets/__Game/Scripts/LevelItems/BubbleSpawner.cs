using System.Collections;
using Assets.__Game.Scripts.Props;
using Lean.Pool;
using UnityEngine;

namespace Assets.__Game.Scripts.LevelItems
{
  public class BubbleSpawner : MonoBehaviour
  {
    [SerializeField] private float _minSpawnInterval = 1.5f;
    [SerializeField] private float _maxSpawnInterval = 2.5f;

    [Space]
    [SerializeField] private float _minBubbleSpeed = 1f;
    [SerializeField] private float _maxBubbleSpeed = 2f;

    [Space]
    [SerializeField] private float _minBubbleScale = 0.05f;
    [SerializeField] private float _maxBubbleScale = 0.075f;

    [Space]
    [SerializeField] private float _bubbleDestroyTime = 10f;

    [Space]
    [SerializeField] private GameObject[] _bubblesToSpawn;

    private Camera _mainCamera;

    private void Awake()
    {
      _mainCamera = Camera.main;
    }

    private void Start()
    {
      StartCoroutine(SpawnBubblesPeriodically());
    }

    private IEnumerator SpawnBubblesPeriodically()
    {
      while (true)
      {
        float randSpawnTime = Random.Range(_minSpawnInterval, _maxSpawnInterval);

        yield return new WaitForSeconds(randSpawnTime);

        SpawnBubble();
      }
    }

    private void SpawnBubble()
    {
      GameObject bubblePrefab = GetRandomBubble();

      float randomX = Random.Range(
          -_mainCamera.orthographicSize * _mainCamera.aspect, _mainCamera.orthographicSize * _mainCamera.aspect);
      Vector3 spawnPosition = new Vector3(randomX, transform.position.y, transform.position.z);

      Bubble spawnedBubble = LeanPool.Spawn(bubblePrefab, spawnPosition, Quaternion.identity).GetComponent<Bubble>();

      float randScale = Random.Range(_minBubbleScale, _maxBubbleScale);
      Vector3 newScale = new Vector3(randScale, randScale, spawnedBubble.transform.localScale.z);
      float randSpeed = Random.Range(_minBubbleSpeed, _maxBubbleSpeed);

      spawnedBubble.transform.localScale = newScale;
      spawnedBubble.SetupBubble(randSpeed, _bubbleDestroyTime);
    }

    private GameObject GetRandomBubble()
    {
      return _bubblesToSpawn[Random.Range(0, _bubblesToSpawn.Length)];
    }
  }
}