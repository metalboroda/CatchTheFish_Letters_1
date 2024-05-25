using Assets.__Game.Scripts.EventBus;
using TMPro;
using UnityEngine;

namespace Assets.__Game.Scripts.Fish
{
  public class FishUiHandler : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI[] _numberTexts;
    [Header("Tutorial")]
    [SerializeField] private SpriteRenderer _tutorialGlowingImage;
    [SerializeField] private Color _neutralColor;
    [SerializeField] private Color _correctGlowingColor;
    [SerializeField] private Color _incorrectGlowingColor;

    private EventBinding<EventStructs.FishUiEvent> _fishEventBinding;

    private void OnEnable()
    {
      _fishEventBinding = new EventBinding<EventStructs.FishUiEvent>(OnFishNumberReceived);
      _fishEventBinding = new EventBinding<EventStructs.FishUiEvent>(SetTutorialGlowingColor);
    }

    private void OnDisable()
    {
      _fishEventBinding.Remove(OnFishNumberReceived);
      _fishEventBinding.Remove(SetTutorialGlowingColor);
    }

    private void OnFishNumberReceived(EventStructs.FishUiEvent fishUiEvent)
    {
      if (this == null) return;
      if (fishUiEvent.FishId != transform.GetInstanceID()) return;

      foreach (var text in _numberTexts)
      {
        text.SetText(fishUiEvent.FishValue.ToString());
      }
    }

    private void SetTutorialGlowingColor(EventStructs.FishUiEvent fishUiEvent)
    {
      if (fishUiEvent.FishId != transform.GetInstanceID()) return;
      if (fishUiEvent.Tutorial == true)
        _tutorialGlowingImage.gameObject.SetActive(true);
      else
      {
        _tutorialGlowingImage.gameObject.SetActive(true);
        _tutorialGlowingImage.color = _neutralColor;

        return;
      }

      _tutorialGlowingImage.color = fishUiEvent.Correct == true ? _correctGlowingColor : _incorrectGlowingColor;
    }
  }
}