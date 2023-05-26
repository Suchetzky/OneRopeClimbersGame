using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{

  [SerializeField] private TextMeshProUGUI timerText;
  [SerializeField] private float timeInSeconds;
  
  private float _currTime;
  private bool _timerIsOn;

  public static UnityEvent OnTimerEnd;

  private void Awake()
  {
    
    if (OnTimerEnd == null)
      OnTimerEnd = new UnityEvent();
  }

  private void Start()
  {
    RestartTimer();
    StartTimer();
  }

  private void Update()
  {
    if (_timerIsOn)
      _currTime -= Time.deltaTime;

    if (_currTime <= 0)
    {
      _currTime = 0;
      PauseTimer();
      OnTimerEnd.Invoke();
    }

    TimeSpan time = GetTime();
    
    timerText.text = time.Minutes == 0 ?
      $"{time.Seconds / 10 % 10}{time.Seconds % 10}.{time.Milliseconds / 10 % 10}{time.Milliseconds % 10}" :
      $"{(time.Minutes / 10 % 10)}{ (time.Minutes % 10)}:{time.Seconds / 10 % 10}{time.Seconds % 10}";
  }

  public void StartTimer()
  {
    _timerIsOn = true;
  }

  public void PauseTimer()
  {
    _timerIsOn = false;
  }

  public void RestartTimer()
  {
    _currTime = timeInSeconds;
    _timerIsOn = false;
  }

  public TimeSpan GetTime()
  {
    return TimeSpan.FromSeconds(_currTime);
  }
}
