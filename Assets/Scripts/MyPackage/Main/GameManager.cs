using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using ZPackage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
#if ANALYTICS_SDKS
using SupersonicWisdomSDK;
#endif

namespace ZPackage
{
    public enum GameState { Starting, Playing, Pause, LevelCompleted, GameOver, Settings, Flying }
    public class GameManager : GenericSingleton<GameManager>
    {
        // [SerializeField] GroundSpawner groundSpawner;
        // [SerializeField] ProgressBar progressBar;
        #region Events
        public event EventHandler<GameState> StateChanged;
        public event EventHandler GameStart;
        public event EventHandler GamePlay;
        public event EventHandler GamePaused;
        public event EventHandler GameResumed;
        public event EventHandler<LevelCompletedEventArgs> LevelCompleted;
        public event EventHandler GameOverEvent;
        public event EventHandler Settings;
        public event EventHandler Flying;
        #endregion
        #region Properties
        private GameState _state;

        public GameState State
        {
            get { return _state; }
            set
            {
                _state = value;
                StateChanged?.Invoke(this, value);
            }
        }

        private bool _isPaused;

        public bool IsPaused
        {
            get { return _isPaused; }
            set
            {
                _isPaused = value;
                if (value)
                {
                    //Time.timeScale = 0;
                    GamePaused?.Invoke(this, EventArgs.Empty);
                    State = GameState.Pause;

                }
                else
                {
                    //Time.timeScale = 1;
                    GameResumed?.Invoke(this, EventArgs.Empty);
                    PlayGame();
                    //State = GameState.Playing;
                }
            }
        }
        int level;
        public int Level
        {
            get { return level; }
            set
            {
                level = value;
                PlayerPrefs.SetInt("level", value);
                Z.CanM.HudLevel(value.ToString());
                // if (progressBar)
                // {
                //     progressBar.UpdateLevel(value);
                // }
            }
        }
        private int coin;
        public int Coin
        {
            get { return coin; }
            set
            {

                coin = value;
                PlayerPrefs.SetInt("coin", value);
                Z.CanM.HudCoin(value.ToString());
            }
        }
        private int score;
        public int Score
        {
            get { return score; }
            set
            {

                score = value;
                // PlayerPrefs.SetInt("score", value);
                Z.CanM.HudScore(value.ToString());
            }
        }

        private int throwCount;
        public int ThrowCount
        {
            get { return throwCount; }
            set
            {

                throwCount = value;
                // PlayerPrefs.SetInt("throwCount", value);
                Z.CanM.HudThrowCount(value.ToString());
            }
        }
        private int nextLvlScore;
        public int NextLvlScore
        {
            get { return nextLvlScore; }
            set
            {
                nextLvlScore = value;
            }
        }

        #endregion

        #region Methods
        public void Awake()
        {

            // groundSpawner = FindObjectOfType<GroundSpawner>();
            // //canvasManager = FindObjectOfType<CanvasManager>();
            // progressBar = FindObjectOfType<ProgressBar>();

        }

        private void Start()
        {
            PopulatePlayerPrefs();
            StartGame();
            QualitySettings.vSyncCount = 0;  // VSync must be disabled
            Application.targetFrameRate = 60;
            CreateGitIgnore();
        }
        void PopulatePlayerPrefs()
        {
            Coin = PlayerPrefs.GetInt("coin", 0);
            Level = PlayerPrefs.GetInt("level", 1);
            // Score = 0;
            ThrowCount = 3;
            NextLvlScore = PlayerPrefs.GetInt("nextLevelScore", 20);
            // Score = PlayerPrefs.GetInt("score",0);
        }
        private void CreateGitIgnore()
        {
            if (!File.Exists(".gitignore"))
            {
                print(Directory.GetCurrentDirectory() + ".gitignore File Created");
                File.WriteAllText(".gitignore", File.ReadAllText(Directory.GetCurrentDirectory() + "/Assets/Scripts/Data/gitignore"));
            }
        }

        private void NextLevel()
        {
            print(Level);
            Level++;
            if (Level < 10)
            {
                PlayerPrefs.SetInt("nextLevelScore", (int)(Level * 5 + Score * 1.2f + 50));
            }
            else
            {
                PlayerPrefs.SetInt("nextLevelScore", (int)(Mathf.Sqrt(Level * Score) * 0.3 + Score));
            }
        }
        private void StartGame()
        {
            GAStartEvent();
            State = GameState.Starting;
            GameStart?.Invoke(this, EventArgs.Empty);
        }
        public void Fly()
        {
            State = GameState.Flying;
            Flying?.Invoke(this, EventArgs.Empty);
        }
        public async void PlayGame()
        {
            GAPlayEvent();
            GamePlay?.Invoke(this, EventArgs.Empty);
            await Task.Yield();
            State = GameState.Playing;
        }

        public void PauseGame(bool value)
        {
            IsPaused = value;
        }
        public void LevelComplete(object sender, float counter)
        {
            if (State != GameState.LevelCompleted)
            {
                LevelCompletedEventArgs eventArgs = new LevelCompletedEventArgs { level = Level, score = Score };
                LevelCompleted?.Invoke(sender, eventArgs);
                State = GameState.LevelCompleted;
                NextLevel();
                GALevelCompleteEvent();
            }
        }
        public void GameOver(object sender, EventArgs e)
        {
            GameOverEvent?.Invoke(sender, e);
            State = GameState.GameOver;
            GAGameOverEvent();
        }
        public void Setting(object sender, EventArgs e)
        {
            Settings?.Invoke(sender, e);
            State = GameState.Settings;
            PauseGame(true);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void ReloadGround()
        {
            Floor[] tiles = FindObjectsOfType<Floor>();
            foreach (var tile in tiles)
            {
                Destroy(tile.gameObject);
            }
            //event subscribe hiij bolno
            // groundSpawner.setNextSpawnPoint(Vector3.zero);
            // groundSpawner.SpawnTiles();
            StartGame();
        }
        void GAStartEvent()
        {
#if ANALYTICS_SDKS
        if (PlayerPrefs.GetInt("level_zero", 0) == 0)
        {
            SupersonicWisdom.Api.NotifyLevelStarted(0, null);
            SupersonicWisdom.Api.NotifyLevelCompleted(0, null);
            PlayerPrefs.SetInt("level_zero", 1);
        }
#endif
        }
        void GAPlayEvent()
        {
#if ANALYTICS_SDKS
            SupersonicWisdom.Api.NotifyLevelStarted(GameController.Level, null);
#endif
        }
        void GAGameOverEvent()
        {
#if ANALYTICS_SDKS
            SupersonicWisdom.Api.NotifyLevelFailed(GameController.Level, null);
#endif
        }
        void GALevelCompleteEvent()
        {
#if ANALYTICS_SDKS
            SupersonicWisdom.Api.NotifyLevelCompleted(GameController.Level, null);
#endif
        }
        #endregion

    }
}
