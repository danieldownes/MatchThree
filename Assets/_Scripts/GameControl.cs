using UnityEngine;

using System;

namespace game
{
    /// <summary>
    /// Main game logic control flow
    /// </summary>
    internal class GameControl : MonoBehaviour
    {
        public RectTransform gameCanvas;

        // Game Grid
        internal Grid grid;

        internal Score score;

        internal CountdownTimer countDownTimer;

        internal bool gameOverFlag;


        public void Start()
        {
            grid = this.GetComponent<Grid>();
            grid.Init(this);

            score = this.GetComponent<Score>();
            score.Init();

            countDownTimer = this.GetComponent<CountdownTimer>();
            countDownTimer.Init();

            Reset();

            StartGame();
        }


        /// <summary>
        /// Reset all objects ready for a new game
        /// </summary>
        private void Reset()
        {
            grid.Reset();
            score.Init();
		}

        /// <summary>
        /// Start a new game
        /// </summary>
        private void StartGame()
        {
            gameOverFlag = false;
            countDownTimer.StartTimer();
            countDownTimer.TimesUp += CountDownTimer_TimesUp;
            grid.Populate(true);
        }

        /// <summary>
        /// Called to trigger gameover state
        /// </summary>
        /// <param name="fromCountdown">Gameover from countdown timeout condition</param>
        internal void GameOver(bool fromCountdown = false)
		{
			// If called from FuseCountdown, but is already game over (ie from no more matches), then ignore
			if( fromCountdown && gameOverFlag)
				return;
			
			// If gems are dropping/chaining, then just set game over flag and return
			// Check flag when dropping/chaining has finished
			gameOverFlag = true;
			if( !grid.canSelect)
				return;

            grid.DropAllGems();
            
            //TODO: setHintGem();

            grid.canSelect = false;
		}


        private void CountDownTimer_TimesUp(object sender, TimesUpEventArgs e)
        {
            GameOver(true);
        }

    }
}