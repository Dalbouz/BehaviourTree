using KrampStudio.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace KrampStudio.BT.General
{
    [Serializable]
    public struct WorkingHours
    {
        public float OpeningTime;
        public float ClosingTime;

        public void StoreWorkingHours(float opening, float closing)
        {
            OpeningTime = opening;
            ClosingTime = closing;
        }
    }

    public class GameManager : SingletonRoot<GameManager>
    {
        [SerializeField] private TMP_Text _currentTimeVisual = default;

        private WaitForSeconds _waitForSecounds = default;
        private Coroutine _dayCycleCoroutine = default;

        public float CurrentTime = 0;
        public float DayInMinutes = 5f;
        public bool IsMuseumOpened = default;
        public WorkingHours MuseumWorkingHours;

        private void Start()
        {
            _waitForSecounds = new WaitForSeconds(CalculateLenghtOfASecondsInADay());

            if(_dayCycleCoroutine != null)
            {
                StopCoroutine(_dayCycleCoroutine);
                _dayCycleCoroutine = null;
            }
            _currentTimeVisual.text = CurrentTime + "h";
            _dayCycleCoroutine = StartCoroutine(DayCycleCoroutine());
        }

        private float CalculateOneDayInSecounds()
        {
            return DayInMinutes * 60f;
        }

        private float CalculateLenghtOfASecondsInADay()
        {
            return CalculateOneDayInSecounds() / 24f;
        }

        private IEnumerator DayCycleCoroutine()
        {
            while (true)
            {
                Debug.Log($"Store opens at: {MuseumWorkingHours.OpeningTime} and closes at: {MuseumWorkingHours.ClosingTime}.\n The current Time is: {CurrentTime} \n The Museum is currently {IsMuseumOpened}.");

                if ((CurrentTime < MuseumWorkingHours.OpeningTime || CurrentTime >= MuseumWorkingHours.ClosingTime)
                    && IsMuseumOpened == true)
                {
                    IsMuseumOpened = false;
                    DEventSystem.Raise(new WorkingHourEvent(IsMuseumOpened));
                }
                else if((CurrentTime >= MuseumWorkingHours.OpeningTime && CurrentTime < MuseumWorkingHours.ClosingTime)
                    && IsMuseumOpened == false)
                {
                    IsMuseumOpened = true;
                    DEventSystem.Raise(new WorkingHourEvent(IsMuseumOpened));
                }

                if (CurrentTime >= 24f)
                {
                    CurrentTime = 0;
                }

                yield return _waitForSecounds;

                CurrentTime += 1; //one hour passed.

                _currentTimeVisual.text = CurrentTime + "h";
            }

        }
    }
}
