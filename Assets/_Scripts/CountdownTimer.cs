using UnityEngine;

using System;

namespace game
{
    
    internal class CountdownTimer : MonoBehaviour
    {
        [SerializeField]
        private Animation Sparks;

        private static float COUNTDOWN_SECONDS = 120.0f;
        private float startTime;
    
	    void Start()
        {
            if( Sparks == null)
            {
                throw new UnassignedReferenceException();
            }
            Sparks.Stop();
            StartTimer();
        }
        
        void Update()
        {
            // Check for game-over condition
            if( Time.timeSinceLevelLoad - startTime > COUNTDOWN_SECONDS)
            {
                OnTimesUp(new TimesUpEventArgs());
            }
        }

        /// <summary>
        /// Start the countdown timer
        /// </summary>
        internal void StartTimer()
        {
            Sparks["CountdownSparks"].speed = 1 / COUNTDOWN_SECONDS;
            Sparks.Play();
            startTime = Time.timeSinceLevelLoad;
        }


        
        protected virtual void OnTimesUp(TimesUpEventArgs e)
        {
            TimesUpHandler handler = TimesUp;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        internal event TimesUpHandler TimesUp;
        internal delegate void TimesUpHandler(object sender, TimesUpEventArgs e);

    }

    internal class TimesUpEventArgs : EventArgs
    {
        internal TimesUpEventArgs()
        {
        }
    }
}
