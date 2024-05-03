using UnityEngine;

namespace Assets.__Game.Scripts.Tools
{
  public class AudioTool
  {
    private AudioSource _audioSource;

    public AudioTool(AudioSource audioSource)
    {
      _audioSource = audioSource;
    }

    public float RandomPitch(float min = 0.9f, float max = 1.1f)
    {
      return _audioSource.pitch = Random.Range(min, max);
    }
  }
}