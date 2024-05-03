using System.Collections;
using System.Collections.Generic;
using Assets.__Game.Scripts.Enums;
using Assets.__Game.Scripts.EventBus;
using Assets.__Game.Scripts.Game.States;
using Assets.__Game.Scripts.Infrastructure;
using Assets.__Game.Scripts.LevelItems;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.__Game.Scripts.Managers
{
  public class GameUiManager : MonoBehaviour
  {
    [Header("Quest Canvas")]
    [SerializeField] private GameObject _questCanvas;
    [Space]
    [SerializeField] private Text _questLevelCounterText;
    [SerializeField] private TextMeshProUGUI _questCorrectNumbersTxt;
    [SerializeField] private Button _questPlayButton;

    [Header("Game Canvas")]
    [SerializeField] private GameObject _gameCanvas;
    [Space]
    [SerializeField] private TextMeshProUGUI _gameScoreCounterTxt;
    [SerializeField] private GameObject _gameStarIcon;
    [Space]
    [SerializeField] private TextMeshProUGUI _gameLoseCounterTxt;
    [SerializeField] private GameObject _gameAngryFaceIcon;
    [Space]
    [SerializeField] private Button _gamePauseButton;
    [Header("Game Canvas Animation")]
    [SerializeField] private float _gameIconScaleIn = 1.3f;
    [SerializeField] private float _gameIconAnimDuration = 0.15f;

    [Header("Win Canvas")]
    [SerializeField] private GameObject _winCanvas;
    [Space]
    [SerializeField] private Button _winNextLevelBtn;
    [SerializeField] private Button _winRewardButton;

    [Header("Lose Canvas")]
    [SerializeField] private GameObject _loseCanvas;
    [Space]
    [SerializeField] private Button _loseRestartBtn;

    [Header("Pause Canvas")]
    [SerializeField] private GameObject _pauseCanvas;
    [Space]
    [SerializeField] private Text _pauseLevelCounterText;
    [SerializeField] private TextMeshProUGUI _pauseCorrectNumbersTxt;
    [SerializeField] private Button _pauseContinueBtn;
    [SerializeField] private Button _pauseRestartButton;
    [Space]
    [SerializeField] private Button _pauseAudioBtn;
    [SerializeField] private GameObject _pauseAudioOnIcon;
    [SerializeField] private GameObject _pauseAudioOffIcon;

    private readonly List<GameObject> _canvases = new();
    private int _currentScore;
    private int _overallScore;
    private int _currentLoses;

    private GameBootstrapper _gameBootstrapper;
    private Reward _reward;
    private GameSettings _gameSettings;

    private EventBinding<EventStructs.SendComponentEvent<GameBootstrapper>> _componentEvent;
    private EventBinding<EventStructs.StateChanged> _stateChanged;
    private EventBinding<EventStructs.FishSpawnerEvent> _fishSpawnerEvent;
    private EventBinding<EventStructs.FishReceivedEvent> _fishReceivedEvent;

    private void Awake()
    {
      _reward = new Reward();
      _gameSettings = new GameSettings();

      LoadSettings();
    }

    private void OnEnable()
    {
      _componentEvent = new EventBinding<EventStructs.SendComponentEvent<GameBootstrapper>>(SetBootstrapper);
      _stateChanged = new EventBinding<EventStructs.StateChanged>(SwitchCanvasesDependsOnState);
      _fishSpawnerEvent = new EventBinding<EventStructs.FishSpawnerEvent>(SetOverallScore);
      _fishReceivedEvent = new EventBinding<EventStructs.FishReceivedEvent>(DisplayScore);
      _fishReceivedEvent = new EventBinding<EventStructs.FishReceivedEvent>(DisplayCorrectNumbersArray);
      _fishReceivedEvent = new EventBinding<EventStructs.FishReceivedEvent>(IconScaleAnimation);
    }

    private void OnDisable()
    {
      _componentEvent.Remove(SetBootstrapper);
      _stateChanged.Remove(SwitchCanvasesDependsOnState);
      _fishSpawnerEvent.Remove(SetOverallScore);
      _fishReceivedEvent.Remove(DisplayScore);
      _fishReceivedEvent.Remove(DisplayCorrectNumbersArray);
      _fishReceivedEvent.Remove(IconScaleAnimation);
    }

    private void Start()
    {
      SubscribeButtons();
      AddCanvasesToList();
      UpdateAudioButtonVisuals();
    }

    private void LoadSettings()
    {
      _gameSettings = SettingsManager.LoadSettings<GameSettings>();

      if (_gameSettings == null)
        _gameSettings = new GameSettings();
    }

    private void SubscribeButtons()
    {
      // Quest
      _questPlayButton.onClick.AddListener(() =>
      {
        EventBus<EventStructs.UiButtonEvent>.Raise(new EventStructs.UiButtonEvent
        {
          UiEnums = UiEnums.QuestPlayButton
        });
      });

      // Game
      _gamePauseButton.onClick.AddListener(() =>
      {
        EventBus<EventStructs.UiButtonEvent>.Raise(new EventStructs.UiButtonEvent
        {
          UiEnums = UiEnums.GamePauseButton
        });
      });

      // Win
      _winNextLevelBtn.onClick.AddListener(() =>
      {
        EventBus<EventStructs.UiButtonEvent>.Raise(new EventStructs.UiButtonEvent
        {
          UiEnums = UiEnums.WinNextLevelButton
        });

        _gameBootstrapper.RestartLevel();
      });
      _winRewardButton.onClick.AddListener(() =>
      {
        _reward.OpenRandomWikipediaFishLink();
        _winRewardButton.gameObject.SetActive(false);
      });

      // Lose
      _loseRestartBtn.onClick.AddListener(() =>
      {
        EventBus<EventStructs.UiButtonEvent>.Raise(new EventStructs.UiButtonEvent
        {
          UiEnums = UiEnums.LoseRestartLevelButton
        });

        _gameBootstrapper.RestartLevel();
      });

      // Pause
      _pauseContinueBtn.onClick.AddListener(() =>
      {
        EventBus<EventStructs.UiButtonEvent>.Raise(new EventStructs.UiButtonEvent
        {
          UiEnums = UiEnums.PauseContinueButton
        });
      });
      _pauseRestartButton.onClick.AddListener(() =>
      {
        _gameBootstrapper.RestartLevel();
      });
      _pauseAudioBtn.onClick.AddListener(SwitchAudioVolumeButton);
    }

    private void AddCanvasesToList()
    {
      _canvases.Add(_questCanvas);
      _canvases.Add(_gameCanvas);
      _canvases.Add(_winCanvas);
      _canvases.Add(_loseCanvas);
      _canvases.Add(_pauseCanvas);
    }

    private void SetBootstrapper(EventStructs.SendComponentEvent<GameBootstrapper> componentEvent)
    {
      _gameBootstrapper = componentEvent.Data;
    }

    private void SetOverallScore(EventStructs.FishSpawnerEvent fishSpawnerEvent)
    {
      _overallScore = fishSpawnerEvent.CorrectFishCount;
      _gameScoreCounterTxt.text = $"{_currentScore} / {_overallScore}";
    }

    private void DisplayScore(EventStructs.FishReceivedEvent fishReceivedEvent)
    {
      if (fishReceivedEvent.CorrectFish == true)
      {
        _currentScore += fishReceivedEvent.CorrectFishIncrement;
        _gameScoreCounterTxt.text = $"{_currentScore} / {_overallScore}";
      }
      else
      {
        _currentLoses += fishReceivedEvent.IncorrectFishIncrement;
        _gameLoseCounterTxt.text = $"{_currentLoses}";
      }
    }

    private void DisplayLevelCounter()
    {
      if (_gameSettings.OverallLevelIndex == 0)
        _questLevelCounterText.text = $"НАВЧАЛЬНИЙ РІВЕНЬ";
      else
        _questLevelCounterText.text = $"РІВЕНЬ {_gameSettings.OverallLevelIndex}";

      if (_gameSettings.OverallLevelIndex == 0)
        _pauseLevelCounterText.text = $"НАВЧАЛЬНИЙ РІВЕНЬ";
      else
        _pauseLevelCounterText.text = $"РІВЕНЬ {_gameSettings.OverallLevelIndex}";
    }

    private void DisplayCorrectNumbersArray(EventStructs.FishReceivedEvent fishReceivedEvent)
    {
      if (fishReceivedEvent.CorrectValues == null) return;

      string arrayString = "";

      for (int i = 0; i < fishReceivedEvent.CorrectValues.Length; i++)
      {
        arrayString += fishReceivedEvent.CorrectValues[i].ToString();

        if (i < fishReceivedEvent.CorrectValues.Length - 1)
          arrayString += " ";
      }

      DisplayLevelCounter();

      _questCorrectNumbersTxt.text = arrayString;
      _pauseCorrectNumbersTxt.text = arrayString;
    }

    private void IconScaleAnimation(EventStructs.FishReceivedEvent fishReceivedEvent)
    {
      Sequence seq = DOTween.Sequence();
      Transform icon = null;

      if (fishReceivedEvent.CorrectFish == true)
        icon = _gameStarIcon.transform;
      else
        icon = _gameAngryFaceIcon.transform;

      seq.Append(icon.DOScale(_gameIconScaleIn, _gameIconAnimDuration));
      seq.Append(icon.DOScale(1f, _gameIconAnimDuration));
    }

    private void SwitchCanvasesDependsOnState(EventStructs.StateChanged state)
    {
      switch (state.State)
      {
        case GameQuestState:
          SwitchCanvas(_questCanvas);
          break;
        case GameplayState:
          SwitchCanvas(_gameCanvas);
          break;
        case GameWinState:
          SwitchCanvas(_winCanvas);
          TryToEnableReward();
          break;
        case GameLoseState:
          SwitchCanvas(_loseCanvas);
          break;
        case GamePauseState:
          SwitchCanvas(_pauseCanvas);
          break;
      }
    }

    private void SwitchCanvas(GameObject canvas, float delay = 0)
    {
      StartCoroutine(DoSwitchCanvas(canvas, delay));
    }

    private IEnumerator DoSwitchCanvas(GameObject canvas, float delay)
    {
      yield return new WaitForSeconds(delay);

      foreach (var canvasItem in _canvases)
      {
        if (canvasItem == canvas)
          canvas.SetActive(true);
        else
          canvasItem.SetActive(false);
      }
    }

    private void TryToEnableReward()
    {
      if (_currentLoses > 0) return;

      _winRewardButton.gameObject.SetActive(true);
    }

    private void SwitchAudioVolumeButton()
    {
      _gameSettings.IsMusicOn = !_gameSettings.IsMusicOn;

      UpdateAudioButtonVisuals();
      EventBus<EventStructs.AudioSwitchedEvent>.Raise();
      SettingsManager.SaveSettings(_gameSettings);
    }

    private void UpdateAudioButtonVisuals()
    {
      _pauseAudioOnIcon.SetActive(_gameSettings.IsMusicOn);
      _pauseAudioOffIcon.SetActive(!_gameSettings.IsMusicOn);
    }
  }
}