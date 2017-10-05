using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel.Store;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.ApplicationInsights;
using Tetris.Common;
using Tetris.Controls;
using Tetris.Engines;
using Tetris.Enums;
using Tetris.Helpers;
using Tetris.Managers;
using Tetris.Models.Core;
using RelayCommand = GalaSoft.MvvmLight.Command.RelayCommand;
using Tetris.Models.Suspend;

namespace Tetris.ViewModel
{
    public class GameViewModelVnext : ViewModelBase
    {
        public BlockUI[,] BackgroundBlocks => gameEngine.BackgroundBlocks;
        public BlockUI[,] MainBlocks => gameEngine.MainBlocks;
        public ShadowBlockUI[,] ShadowBlocks => gameEngine.ShadowBlocks;
        public BlockUI[,] NextBlock => gameEngine.NextBlock;

        public int Score
        {
            get { return score; }
            set { Set(() => Score, ref score, value); }
        }

        public int Level
        {
            get { return level; }
            set { Set(() => Level, ref level, value); }
        }

        public string Time
        {
            get { return time; }
            set { Set(() => Time, ref time, value); }
        }

        public string PlayerName
        {
            get { return playerName; }
            set { Set(() => PlayerName, ref playerName, value); }
        }

        public GridLength BottomControlMargin
        {
            get { return bottomControlMargin; }
            set { Set(() => BottomControlMargin, ref bottomControlMargin, value); }
        }

        public GridLength MiddleControlMargin
        {
            get { return middleContolMargin; }
            set { Set(() => MiddleControlMargin, ref middleContolMargin, value); }
        }

        public string ContinueGameHeight
        {
            get { return continueGameHeight; }
            set { Set(() => ContinueGameHeight, ref continueGameHeight, value); }
        }

        public RecordsType RecordsType
        {
            get { return recordsType; }
            set { Set(() => RecordsType, ref recordsType, value); }
        }

        public RecordsMode RecordsMode
        {
            get { return recordsMode; }
            set { Set(() => RecordsMode, ref recordsMode, value); }
        }

        public ObservableCollection<GameResult> GameResults
        {
            get { return gameResults; }
            set { Set(() => GameResults, ref gameResults, value); }
        }

        public ObservableCollection<GameResult> ClassicResults
        {
            get { return classicResults; }
            set { Set(() => ClassicResults, ref classicResults, value); }
        }

        public ObservableCollection<GameResult> InfinityResults
        {
            get { return infinityResults; }
            set { Set(() => InfinityResults, ref infinityResults, value); }
        }

        public ObservableCollection<GameResult> TimeAttackResults
        {
            get { return timeAttackResults; }
            set { Set(() => TimeAttackResults, ref timeAttackResults, value); }
        }

        public int ClassicBest
        {
            get { return classicBest; }
            set { Set(() => ClassicBest, ref classicBest, value); }
        }

        public int TimeAttackBest
        {
            get { return timeAttackBest; }
            set { Set(() => TimeAttackBest, ref timeAttackBest, value); }
        }

        public int InfinityBest
        {
            get { return infinityBest; }
            set { Set(() => InfinityBest, ref infinityBest, value); }
        }

        private async void LoadSettingsAsync()
        {
            var gameSettings = await SettingsManager.LoadSettignsAsync().ConfigureAwait(false);
            SetGameSettings(gameSettings);
        }

        private async void SetGameSettings(GameSettings gameSettings)
        {
            ControlType = gameSettings.ControlType;
            ControlPosition = gameSettings.ControlPosition;
            BottomControlMargin = new GridLength(SettingsManager.GetPositionValue(ControlPosition), GridUnitType.Star);
            ControlMargin = gameSettings.ControlMargin;
            MiddleControlMargin = new GridLength(SettingsManager.GetMarginValue(ControlMargin));
            IsShadowAvailable = gameSettings.IsShadowAvailable;
            IsAudioAvailable = gameSettings.IsAudioAvailable;
            IsVibrateAvailable = gameSettings.IsVibrateAvailable;
            ThemeScheme = gameSettings.Theme;
            CurrentColorShemeIndex = gameSettings.ColorIndex;
            ColorsShemes = await ColorShemeManager.GetColorShemesAsync().ConfigureAwait(false);
            isHelpViewed = gameSettings.IsHelpViewed;
        }

        private ControlType controlType;
        private Position controlPosition;
        private Margin controlMargin;
        private bool isShadowAvailable;
        private Theme themeScheme;

        #region Settings properties
        public ControlType ControlType
        {
            get { return controlType; }
            set { Set(() => ControlType, ref controlType, value); }
        }

        public Position ControlPosition
        {
            get { return controlPosition; }
            set { Set(() => ControlPosition, ref controlPosition, value); }
        }

        public Margin ControlMargin
        {
            get { return controlMargin; }
            set { Set(() => ControlMargin, ref controlMargin, value); }
        }

        public bool IsShadowAvailable
        {
            get { return isShadowAvailable; }
            set { Set(() => IsShadowAvailable, ref isShadowAvailable, value); }
        }

        public bool IsAudioAvailable
        {
            get { return isAudioAvailable; }
            set { Set(() => IsAudioAvailable, ref isAudioAvailable, value); }
        }

        public bool IsVibrateAvailable
        {
            get { return isVibrateAvailable; }
            set { Set(() => IsVibrateAvailable, ref isVibrateAvailable, value); }
        }

        public Theme ThemeScheme
        {
            get { return themeScheme; }
            set { Set(() => ThemeScheme, ref themeScheme, value); }
        }

        public List<ColorSheme> ColorsShemes { get; set; }

        public int CurrentColorShemeIndex
        {
            get { return currentColorShemeIndex; }
            set { Set(() => CurrentColorShemeIndex, ref currentColorShemeIndex, value); }
        }
        #endregion

        private void SwitchControlTypeToSwipe()
        {
            ControlType = ControlType.Swipe;
            SettingsManager.SaveControlType((int)ControlType);
        }

        private void SwitchControlTypeToButtons()
        {
            ControlType = ControlType.Buttons;
            SettingsManager.SaveControlType((int)ControlType);
        }

        private void SwitchControlsPosition(string value)
        {
            var enumValue = (Position)Enum.Parse(typeof(Position), value);
            ControlPosition = enumValue;
            SettingsManager.SaveControlPositionIndex((int)enumValue);
            BottomControlMargin = new GridLength(SettingsManager.GetPositionValue(enumValue), GridUnitType.Star);
        }

        private void SwitchControlsMargin(string value)
        {
            var enumValue = (Margin)Enum.Parse(typeof(Margin), value);
            ControlMargin = enumValue;
            SettingsManager.SaveControlMarginIndex((int)enumValue);
            MiddleControlMargin = new GridLength(SettingsManager.GetMarginValue(enumValue));
        }

        private void SwitchShadowAvailable()
        {
            IsShadowAvailable = !IsShadowAvailable;
            SettingsManager.SaveShadowManager(IsShadowAvailable);
            gameEngine.SwitchShadow(IsShadowAvailable);
        }

        private void SwitchAudio()
        {
            IsAudioAvailable = !IsAudioAvailable;
            gameEngine.SwitchAduioAvailable(IsAudioAvailable);
            SettingsManager.SaveAudioSetting(IsAudioAvailable);
        }

        private void SwitchVibrate()
        {
            IsVibrateAvailable = !IsVibrateAvailable;
            SettingsManager.SaveVibrationSettings(IsVibrateAvailable);
        }

        private void SwitchThemeAndNotify(string value)
        {
            var enumValue = (Theme)Enum.Parse(typeof(Theme), value);
            ThemeScheme = enumValue;
            SettingsManager.SaveThemeIndex((int)enumValue);
            IsRestartNotificationAvailable = true;
        }

        private void SaveColorAndNotify(string index)
        {
            var newIndex = int.Parse(index);
            SettingsManager.SaveColorShemeIndex(newIndex);
            var colors = SettingsManager.GetNewColorsSheme(newIndex, CurrentColorShemeIndex);
            gameEngine.SwitchBlocksColors(colors, newIndex);
            CurrentColorShemeIndex = newIndex;
        }

        public RelayCommand SwitchControlTypeToButtonsCommand { get; set; }
        public RelayCommand SwitchControlTypeToSwipeCommand { get; set; }
        public RelayCommand<string> SwitchControlsPositionCommand { get; set; }
        public RelayCommand<string> SwitchControlsMarginCommand { get; set; }
        public RelayCommand SwitchShadowAvailableCommand { get; set; }
        public RelayCommand SwitchAudioCommand { get; set; }
        public RelayCommand SwitchVibrateCommand { get; set; }
        public RelayCommand<string> SwitchThemeAndNotifyCommand { get; set; }


        public bool IsContinueAvailable
        {
            get { return isContinueAvailable; }
            set
            {
                SwitchIsContinueAvailable(value);
                Set(() => IsContinueAvailable, ref isContinueAvailable, value);
            }
        }

        public bool IsMenuAvailable
        {
            get { return isMenuAvailable; }
            set { Set(() => IsMenuAvailable, ref isMenuAvailable, value); }
        }

        public bool IsAboutAvailable
        {
            get { return isAboutAvailable; }
            set { Set(() => IsAboutAvailable, ref isAboutAvailable, value); }
        }

        public bool IsGameResultAvailable
        {
            get { return isGameResultAvailable; }
            set
            {
                if (value && !IsGameResultAvailable) SaveGameResultAsync();
                Set(() => IsGameResultAvailable, ref isGameResultAvailable, value);
            }
        }

        public bool IsRecordsAvailable
        {
            get { return isRecordsAvailable; }
            set { Set(() => IsRecordsAvailable, ref isRecordsAvailable, value); }
        }

        public bool IsSettingsAvailable
        {
            get { return isSettingsAvailable; }
            set { Set(() => IsSettingsAvailable, ref isSettingsAvailable, value); }
        }

        public bool IsRestartNotificationAvailable
        {
            get { return isRestartNotificationAvailable; }
            set
            {
                if (IsRestartNotificationAvailable)
                {
                    SaveStateAndClose();
                }
                Set(() => IsRestartNotificationAvailable, ref isRestartNotificationAvailable, value);
            }
        }

        public bool IsChooseModeAvailable
        {
            get { return isChooseModeAvailable; }
            set { Set(() => IsChooseModeAvailable, ref isChooseModeAvailable, value); }
        }

        public bool IsShutDownRateAppAvailable
        {
            get { return isShutDownRateAppAvailable; }
            set { Set(() => IsShutDownRateAppAvailable, ref isShutDownRateAppAvailable, value); }
        }

        public bool IsDonateAvailable
        {
            get { return isDonateAvailable; }
            set { Set(() => IsDonateAvailable, ref isDonateAvailable, value); }
        }

        public bool IsHelpAvailable
        {
            get { return isHelpAvailable; }
            set
            {
                if (IsHelpAvailable && !value)
                    ResumeGame();
                if (!IsHelpAvailable && value)
                {
                    PauseGame();
                    gameDuration.Stop();
                }

                Set(() => IsHelpAvailable, ref isHelpAvailable, value);
            }
        }

        public RelayCommand MoveToLeftCommand { get; set; }
        public RelayCommand MoveToRightCommand { get; set; }
        public RelayCommand MoveToDownCommand { get; set; }
        public RelayCommand RotateCommand { get; set; }
        public RelayCommand PointerReleasedCommand { get; set; }
        public RelayCommand PauseResumeCommand { get; set; }
        public RelayCommand PauseRestartCommand { get; set; }
        public RelayCommand ChooseModeCommand { get; set; }
        public RelayCommand ShowPauseMenuCommand { get; set; }
        public RelayCommand SwitchAboutCommand { get; set; }
        public RelayCommand ShowRecordsCommand { get; set; }
        public RelayCommand HideRecordsCommand { get; set; }
        public RelayCommand HideGameResultCommand { get; set; }
        public RelayCommand SwitchSettingsCommand { get; set; }
        public RelayCommand RateAppCommand { get; set; }
        public RelayCommand ResumeOrRestartGameCommand { get; set; }
        public RelayCommand DisableRestartNotificationCommand { get; set; }
        public RelayCommand DisableRateNotificationCommand { get; set; }
        public RelayCommand BackPressCommand { get; set; }
        public RelayCommand StartClassicGameCommand { get; set; }
        public RelayCommand StartTimeGameCommand { get; set; }
        public RelayCommand StartGetLinesGameCommand { get; set; }
        public RelayCommand SwitchRecordsLeftCommand { get; set; }
        public RelayCommand SwitchRecordsRightCommand { get; set; }
        public RelayCommand SwitchRecordsTypeCommand { get; set; }
        public RelayCommand<string> SwitchColorCommand { get; set; }
        public RelayCommand NickNameChangedCommand { get; set; }
        public RelayCommand<string> OpenLinkCommand { get; set; }
        public RelayCommand SendMailCommand { get; set; }
        public RelayCommand SwitchHelpCommand { get; set; }
        public RelayCommand ShutdownRateAppCommand { get; set; }
        public RelayCommand SwitchDonateAvailableCommand { get; set; }
        public RelayCommand<string> DonateCommand { get; set; }

        private GameEngine gameEngine;
        private Mode gameMode;
        private int score;
        private int level;
        private string time;
        private int secondsCount;
        private string playerName;
        private DispatcherTimer gameDuration;
        private GridLength bottomControlMargin;
        private GridLength middleContolMargin;
        private string continueGameHeight;
        private RecordsType recordsType;
        private RecordsMode recordsMode;
        private RecordManager gameResultService;
        private ObservableCollection<GameResult> gameResults;
        private ObservableCollection<GameResult> classicResults;
        private ObservableCollection<GameResult> timeAttackResults;
        private ObservableCollection<GameResult> infinityResults;
        private int classicBest;
        private int timeAttackBest;
        private int infinityBest;
        private int currentColorShemeIndex;
        private bool isContinueAvailable;
        private bool isAudioAvailable;
        private bool isVibrateAvailable;
        private bool isMenuAvailable = true;
        private bool isAboutAvailable;
        private bool isGameResultAvailable;
        private bool isRecordsAvailable;
        private bool isSettingsAvailable;
        private bool isRestartNotificationAvailable;
        private bool isChooseModeAvailable;
        private bool isHelpAvailable;
        private bool isHelpViewed;
        private bool isShutDownRateAppAvailable;
        private bool isDonateAvailable;
        private int xStartIndex, yStartIndex;
        private bool isSwypeRotated;
#if WINDOWS_PHONE_APP
        private Windows.Phone.Devices.Notification.VibrationDevice vibrationDevice;
#endif

        public GameViewModelVnext()
        {
            LoadSettingsAsync();
            InitializeGameEngine();
            InitializeMangers();
            InitializeVibration();
            InitializeCommands();
            InitializeKeysInputActions();
        }

        private void InitializeGameEngine()
        {
            gameEngine = new GameEngine(isShadowAvailable, CurrentColorShemeIndex);
            gameEngine.OnScoreChanged += i => Score = i;
            gameEngine.OnLevelChanged += i => Level = i;
            gameEngine.OnGameOver += () =>
            {
                gameDuration.Stop();
                IsContinueAvailable = false;
                IsGameResultAvailable = true;
            };
            gameEngine.SwitchAduioAvailable(IsAudioAvailable);
            moveTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(GameConfig.DefaultMoveSpeed) };
            Time = secondsCount.GetTimeString();
        }

        private void InitializeMangers()
        {
            gameResultService = new RecordManager();
            LoadLocalGameResultsAsync();
        }

        private void InitializeVibration()
        {
#if WINDOWS_PHONE_APP
            vibrationDevice = Windows.Phone.Devices.Notification.VibrationDevice.GetDefault();
#endif
        }

        private void InitializeCommands()
        {
            MoveToLeftCommand = new RelayCommand(() => TouchMove(gameEngine.MoveToLeft));
            MoveToRightCommand = new RelayCommand(() => TouchMove(gameEngine.MoveToRight));
            MoveToDownCommand = new RelayCommand(() => TouchMove(gameEngine.MoveToDown, GameConfig.VerticalMoveSpeed));
            RotateCommand = new RelayCommand(RotateTouch);
            PointerReleasedCommand = new RelayCommand(PointerReleased);
            PauseResumeCommand = new RelayCommand(ResumeGame);
            PauseRestartCommand = new RelayCommand(() => NewGame(Mode.Classic));
            ChooseModeCommand = new RelayCommand(ChooseMode);
            ShowPauseMenuCommand = new RelayCommand(() => IsMenuAvailable = true);
            SwitchAboutCommand = new RelayCommand(() => IsAboutAvailable = true);
            ShowRecordsCommand = new RelayCommand(() => IsRecordsAvailable = true);
            HideRecordsCommand = new RelayCommand(() => IsRecordsAvailable = false);
            HideGameResultCommand = new RelayCommand(() => IsGameResultAvailable = false);
            SwitchSettingsCommand = new RelayCommand(() => IsSettingsAvailable = true);
            SwitchAudioCommand = new RelayCommand(SwitchAudio);
            SwitchVibrateCommand = new RelayCommand(SwitchVibrate);
            RateAppCommand = new RelayCommand(RateApp);
            ResumeOrRestartGameCommand = new RelayCommand(ResumeOrRestart);
            DisableRestartNotificationCommand = new RelayCommand(() => IsRestartNotificationAvailable = false);
            BackPressCommand = new RelayCommand(BackNavigate);
            StartClassicGameCommand = new RelayCommand(() => NewGame(Mode.Classic));
            StartTimeGameCommand = new RelayCommand(() => NewGame(Mode.TimeAttack, false, 120));
            StartGetLinesGameCommand = new RelayCommand(() => NewGame(Mode.Infinity, true));
            SwitchRecordsLeftCommand = new RelayCommand(SwitchRecordsLeft);
            SwitchRecordsRightCommand = new RelayCommand(SwitchRecordsRight);
            SwitchRecordsTypeCommand = new RelayCommand(SwitchRecordsType);
            SwitchColorCommand = new RelayCommand<string>(SaveColorAndNotify);
            SwitchControlsPositionCommand = new RelayCommand<string>(SwitchControlsPosition);
            SwitchControlsMarginCommand = new RelayCommand<string>(SwitchControlsMargin);
            SwitchControlTypeToButtonsCommand = new RelayCommand(SwitchControlTypeToButtons);
            SwitchControlTypeToSwipeCommand = new RelayCommand(SwitchControlTypeToSwipe);
            SwitchShadowAvailableCommand = new RelayCommand(SwitchShadowAvailable);
            SwitchThemeAndNotifyCommand = new RelayCommand<string>(SwitchThemeAndNotify);
            NickNameChangedCommand = new RelayCommand(UpdateLastNickName);
            OpenLinkCommand = new RelayCommand<string>(async (link) => { await Launcher.LaunchUriAsync(new Uri(link)); });
            SendMailCommand = new RelayCommand(async () => { await Launcher.LaunchUriAsync(new Uri("mailto:?to=d2funlife@gmail.com")); });
            SwitchHelpCommand = new RelayCommand(() => { IsHelpAvailable = !IsHelpAvailable; });
            SwitchDonateAvailableCommand = new RelayCommand(() => IsDonateAvailable = true);
            DonateCommand = new RelayCommand<string>(Donate);
        }

        private void InitializeKeysInputActions()
        {
#if WINDOWS_PHONE_APP
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += BackPressed;
#endif
#if WINDOWS_APP

#endif
        }

        private void GameDurationIncrement()
        {
            secondsCount++;
            Time = secondsCount.GetTimeString();
        }

        private void GameDurationDecrement()
        {
            if (secondsCount == 0)
            {
                gameEngine.GameOver();
                return;
            }
            secondsCount--;
            Time = secondsCount.GetTimeString();
        }

        private void GameDurationInitialize(int initializeSeconds = 0)
        {
            secondsCount = initializeSeconds;
            Time = secondsCount.GetTimeString();
            gameDuration = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            if (initializeSeconds == 0)
                gameDuration.Tick += (sender, o) => GameDurationIncrement();
            else
                gameDuration.Tick += (sender, o) => GameDurationDecrement();

        }

        private void GameDurationRecreate()
        {
            gameDuration = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            if (gameMode == Mode.TimeAttack)
            {
                gameDuration.Tick += (sender, o) => GameDurationDecrement();
                return;
            }
            gameDuration.Tick += (sender, o) => GameDurationIncrement();
        }

        public void PauseGame()
        {
            gameEngine.Pause();
        }

        private void ResumeGame()
        {
            IsMenuAvailable = false;
            if (gameDuration == null)
                GameDurationRecreate();
            gameDuration.Start();
            gameEngine.Play();
        }

        private void ResumeOrRestart()
        {
            if (IsContinueAvailable)
                ResumeGame();
            else
                ChooseMode();
        }

        private void NewGame(Mode mode, bool stableSpeed = false, int initializeSeconds = 0)
        {
            gameMode = mode;
            IsContinueAvailable = true;
            IsChooseModeAvailable = false;
            IsMenuAvailable = false;
            IsGameResultAvailable = false;
            IsRecordsAvailable = false;
            GameDurationInitialize(initializeSeconds);
            gameEngine.SetIsSpeedStable(stableSpeed);
            moveTimer.Stop();
            moveTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(GameConfig.DefaultMoveSpeed) };
            isTouchReleased = true;
            gameEngine.Restart();
            gameDuration.Start();
            if (!isHelpViewed)
            {
                IsHelpAvailable = true;
                SettingsManager.SetIsHelpViewedSetting();
                isHelpViewed = true;
            }
            var tc = new TelemetryClient();
            tc.TrackEvent("New game " + mode);
        }

        private void ChooseMode()
        {
            IsChooseModeAvailable = true;
            IsMenuAvailable = true;
            IsGameResultAvailable = false;
        }

        private void SwitchIsContinueAvailable(bool value)
        {
            ContinueGameHeight = value ? "5*" : "0";
        }

        private async void RateApp()
        {
#if WINDOWS_PHONE_APP

            await Launcher.LaunchUriAsync(new Uri(GameConfig.WindowsStoreUrl));
#endif
#if WINDOWS_APP
            await Launcher.LaunchUriAsync(new Uri(GameConfig.WindowsPhoneUrl));
#endif
        }

        private DispatcherTimer moveTimer;
        private bool isTouchReleased = true;

        private void Vibrate()
        {
#if WINDOWS_PHONE_APP
            if (!IsVibrateAvailable) return;
            vibrationDevice.Vibrate(TimeSpan.FromMilliseconds(GameConfig.VibrateDuration));
#endif
        }

        private void PointerReleased()
        {
            moveTimer.Stop();
            moveTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(GameConfig.DefaultMoveSpeed) };
            isTouchReleased = true;
        }

        private void TouchMove(Action action, int interval = GameConfig.DefaultMoveSpeedConst)
        {
            if (!isTouchReleased) return;
            Vibrate();
            action();
            moveTimer.Interval = TimeSpan.FromMilliseconds(interval);
            moveTimer.Tick += (sender, o) => action();
            moveTimer.Start();
            isTouchReleased = false;
        }

        private void RotateTouch()
        {
            Vibrate();
            gameEngine.RotateMove();
        }
       
        public void LoadState()
        {
            var session = SuspensionManager.GetSuspendingSession();
            if (session == null) return;

            gameEngine.SetSuspending(session.Game);
            if (session.Game != null)
                gameMode = session.Game.GameMode;
            if (gameMode == Mode.TimeAttack)
                GameDurationInitialize(120);
            else
                GameDurationInitialize();
            IsContinueAvailable = session.ContinueAvailable;
            Time = session.Time;
            secondsCount = session.SecondsCount;
        }

        public void SaveState()
        {
            var session = new SuspendingSession
            {
                Game = gameEngine.GetSuspending(),
                ContinueAvailable = IsContinueAvailable,
                Time = Time,
                SecondsCount = secondsCount
            };
            if (session.Game != null)
                session.Game.GameMode = gameMode;
            SuspensionManager.SaveSession(session);
        }

        public async void SaveStateOnClose()
        {
            await SuspensionManager.SaveAsync().ConfigureAwait(true);
        }

        private async void SaveStateAndClose()
        {
            await SuspensionManager.SaveAsync().ConfigureAwait(true);
            Application.Current.Exit();
        }
        
        private void SwitchRecordsType()
        {
            RecordsType = RecordsType == RecordsType.Local ? RecordsType.Global : RecordsType.Local;
            SwitchRecords(RecordsMode.Classic);
        }

        public void SwitchRecordsLeft()
        {
            if (RecordsMode == RecordsMode.Classic)
            {
                SwitchRecords(RecordsMode.Infinity);
                return;
            }
            var mode = (int)RecordsMode;
            SwitchRecords((RecordsMode)(--mode));
        }

        public void SwitchRecordsRight()
        {
            if (RecordsMode == RecordsMode.Infinity)
            {
                SwitchRecords(RecordsMode.Classic);
                return;
            }
            var mode = (int)RecordsMode;
            SwitchRecords((RecordsMode)(++mode));
        }

        private void SwitchRecords(RecordsMode mode)
        {
            if (recordsType == RecordsType.Local)
                SwitchLocalRecords(mode);
            else
                SwitchGlobalRecords(mode);
        }

        private void SwitchLocalRecords(RecordsMode mode)
        {
            RecordsMode = mode;
            if (mode == RecordsMode.Classic)
                GameResults = ClassicResults;
            if (mode == RecordsMode.TwoMinutes)
                GameResults = TimeAttackResults;
            if (mode == RecordsMode.Infinity)
                GameResults = InfinityResults;
        }

        private void SwitchGlobalRecords(RecordsMode mode)
        {
            GameResults = new ObservableCollection<GameResult>();
        }

        private async void SaveGameResultAsync()
        {
            var result = GetGameResult();

            if (gameMode == Mode.Classic)
            {
                ClassicResults = await gameResultService.UpdateResultsAsync(ClassicResults, result, gameMode).ConfigureAwait(true);
                ClassicBest = ClassicResults.First().Score;
                RecordsMode = RecordsMode.Classic;
            }
            if (gameMode == Mode.TimeAttack)
            {
                TimeAttackResults = await gameResultService.UpdateResultsAsync(TimeAttackResults, result, gameMode).ConfigureAwait(true);
                TimeAttackBest = TimeAttackResults.First().Score;
                RecordsMode = RecordsMode.TwoMinutes;
            }
            if (gameMode == Mode.Infinity)
            {
                InfinityResults = await gameResultService.UpdateResultsAsync(InfinityResults, result, gameMode).ConfigureAwait(true);
                InfinityBest = InfinityResults.First().Score;
                RecordsMode = RecordsMode.Infinity;
            }

            SwitchLocalRecords(RecordsMode);

            var tc = new TelemetryClient();
            var properties = new Dictionary<string, string> { { "Mode", gameMode.ToString() } };
            var measurements = new Dictionary<string, double> { { "Scores", result.Score }, { "Level", result.Level } };
            tc.TrackEvent("Game over", properties, measurements);
        }

        private async void UpdateLastNickName()
        {
            var result = GetGameResult();

            if (gameMode == Mode.Classic)
            {
                ClassicResults = await gameResultService.UpdateLastNickNameAsync(ClassicResults, result.PlayerName, gameMode).ConfigureAwait(false);
            }
            if (gameMode == Mode.TimeAttack)
            {
                TimeAttackResults = await gameResultService.UpdateLastNickNameAsync(TimeAttackResults, result.PlayerName, gameMode).ConfigureAwait(false);
            }
            if (gameMode == Mode.Infinity)
            {
                InfinityResults = await gameResultService.UpdateLastNickNameAsync(InfinityResults, result.PlayerName, gameMode).ConfigureAwait(false);
            }
        }

        private async void LoadLocalGameResultsAsync()
        {
            PlayerName =  await SettingsManager.GetPlayerNameAsync().ConfigureAwait(true);
            ClassicResults = await gameResultService.GetResults(Mode.Classic).ConfigureAwait(true);
            TimeAttackResults = await gameResultService.GetResults(Mode.TimeAttack).ConfigureAwait(true);
            InfinityResults = await gameResultService.GetResults(Mode.Infinity).ConfigureAwait(true);
            ClassicBest = (ClassicResults.Count > 0) ? ClassicResults.First().Score : 0;
            TimeAttackBest = (TimeAttackResults.Count > 0) ? TimeAttackResults.First().Score : 0;
            InfinityBest = (InfinityResults.Count > 0) ? InfinityResults.First().Score : 0;
            SwitchRecords(RecordsMode.Classic);
        }

        private GameResult GetGameResult()
        {
            return new GameResult
            {
                CreateTime = DateTime.Now,
                Level = Level,
                Score = Score + 1,
                Time = Time,
                PlayerName = PlayerName,
                IsLastResult = true
            };
        }

        private void BackNavigate()
        {
            BackNavigateHandle();
        }

        private bool BackNavigateHandle()
        {
            if (IsDonateAvailable)
            {
                IsDonateAvailable = false;
                return true;
            }
            if (IsHelpAvailable)
            {
                IsHelpAvailable = false;
                return true;
            }
            if (IsChooseModeAvailable)
            {
                IsChooseModeAvailable = false;
                return true;
            }
            if (IsRestartNotificationAvailable)
            {
                IsRestartNotificationAvailable = false;
                return true;
            }
            if (IsAboutAvailable)
            {
                IsAboutAvailable = false;
                return true;
            }
            if (IsRecordsAvailable)
            {
                IsRecordsAvailable = false;
                IsMenuAvailable = true;
                return true;
            }
            if (IsGameResultAvailable)
            {
                IsGameResultAvailable = false;
                IsMenuAvailable = true;
                return true;
            }
            if (IsSettingsAvailable)
            {
                IsSettingsAvailable = false;
                return true;
            }
            if (!IsMenuAvailable)
            {
                IsMenuAvailable = true;
                gameDuration.Stop();
                gameEngine.Pause();
                return true;
            }

            return false;
        }

        private int horizontalDelta = 1;
        private int verticalDelta = 2;

        public void SwipeStart(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            if (ControlType != ControlType.Swipe) return;
            var pointerOnBoard = pointerRoutedEventArgs.GetCurrentPoint((UIElement)sender).Position;
            xStartIndex = (int)Math.Ceiling(pointerOnBoard.X / gameEngine.Config.BlockSize);
            yStartIndex = (int)Math.Ceiling(pointerOnBoard.Y / gameEngine.Config.BlockSize);
        }

        public void SwipeMove(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            if (ControlType != ControlType.Swipe) return;
            var pointerOnBoard = pointerRoutedEventArgs.GetCurrentPoint((UIElement)sender).Position;
            var xIndex = (int)Math.Ceiling(pointerOnBoard.X / gameEngine.Config.BlockSize);
            var yIndex = (int)Math.Ceiling(pointerOnBoard.Y / gameEngine.Config.BlockSize);
            var xDelta = xStartIndex - xIndex;
            var yDelta = yStartIndex - yIndex;

            if (!isSwypeRotated && yDelta > verticalDelta)
            {
                gameEngine.RotateMove();
                isSwypeRotated = true;
                return;
            }

            if (yDelta < -verticalDelta)
            {
                gameEngine.MoveToDown();
                xStartIndex = xIndex;
                yStartIndex = yIndex;
                verticalDelta = 1;
                return;
            }

            if (xDelta > horizontalDelta)
            {
                gameEngine.MoveToLeft();
                xStartIndex = xIndex;
                yStartIndex = yIndex;
                horizontalDelta = 0;
                return;
            }

            if (xDelta < -horizontalDelta)
            {
                gameEngine.MoveToRight();
                xStartIndex = xIndex;
                yStartIndex = yIndex;
                horizontalDelta = 0;
            }
        }

        public void SwipeEnd(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            if (ControlType != ControlType.Swipe) return;
            Vibrate();
            isSwypeRotated = false;
            xStartIndex = 0;
            yStartIndex = 0;
            verticalDelta = 2;
            horizontalDelta = 1;
        }

        public void SwipeTap(object sender, TappedRoutedEventArgs tappedRoutedEventArgs)
        {
            Vibrate();
            gameEngine.MoveDropDown();
        }

#if WINDOWS_PHONE_APP
        private void BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            var isHandled = BackNavigateHandle();
            if (isHandled) e.Handled = true;
        }
#endif

        private async void Donate(string number)
        {
            var tc = new TelemetryClient();
            tc.TrackEvent("Donate for " + number + " started");
            var listing = await CurrentApp.LoadListingInformationAsync();
            var prod = listing.ProductListings.FirstOrDefault(x => x.Value.ProductId == "Donate" + number);
            var purchaseResults = await CurrentApp.RequestProductPurchaseAsync(prod.Value.ProductId);
            switch (purchaseResults.Status)
            {
                case ProductPurchaseStatus.Succeeded:
                    //product1TempTransactionId = purchaseResults.TransactionId;

                    // Grant the user their purchase here, and then pass the product ID and transaction ID to currentAppSimulator.reportConsumableFulfillment
                    // To indicate local fulfillment to the Windows Store.
                    tc.TrackEvent("Donate for " + number + " done");
                    await CurrentApp.ReportConsumableFulfillmentAsync(prod.Value.ProductId, purchaseResults.TransactionId);
                    break;

                case ProductPurchaseStatus.NotFulfilled:
                    //product1TempTransactionId = purchaseResults.TransactionId;

                    // First check for unfulfilled purchases and grant any unfulfilled purchases from an earlier transaction.
                    // Once products are fulfilled pass the product ID and transaction ID to currentAppSimulator.reportConsumableFulfillment
                    // To indicate local fulfillment to the Windows Store.
                    tc.TrackEvent("Donate for " + number + " unfulfilled");
                    break;
                case ProductPurchaseStatus.NotPurchased:
                    tc.TrackEvent("Donate for " + number + " not purchased");
                    break;
                case ProductPurchaseStatus.AlreadyPurchased:
                    tc.TrackEvent("Donate for " + number + " already purchased");
                    break;
            }
        } 
    }
}