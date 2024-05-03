using Assets.__Game.Scripts.Tools;
using UnityEngine;

namespace Assets.__Game.Scripts.Audio
{
  [RequireComponent(typeof(AudioSource))]
  public class AudioEffect : MonoBehaviour
  {
    [SerializeField] private AudioClip[] _clips;

    private AudioSource _audioSource;

    private AudioTool _audioTool;

    private void Awake()
    {
      _audioSource = GetComponent<AudioSource>();

      _audioTool = new AudioTool(_audioSource);
    }

    private void OnEnable()
    {
      PlayRandomCLip();
    }

    private void PlayRandomCLip()
    {
      _audioTool.RandomPitch();
      _audioSource.PlayOneShot(GetRandomCLip());
    }

    private AudioClip GetRandomCLip()
    {
      return _clips[Random.Range(0, _clips.Length)];
    }
  }
}