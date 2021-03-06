﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronos;

public class TimeManager : MonoBehaviour
{
    //How many real seconds is an In Game Minute
    public float _inGameMinute = 5f;
    private Clock clock;

    private TimeSpan time;
    public TimeSpan Time {
        get {
            return Instance.time;
        }
    }
    public int Minute {
        get {
            return Instance.time.Minutes;
        }
    }
    public int Hour {
        get {
            return Instance.time.Hours;
        }
    }
    public int Day {
        get {
            return Instance.time.Days;
        }
    }

    public static TimeManager Instance { get; set; }

    private void Awake() {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
        Instance.clock = Timekeeper.instance.Clock("Root");
        Instance.time = TimeSpan.FromMinutes(0);
        StartCoroutine(CountMinute());
    }

    public TimeSpan SetTime(TimeSpan _time) {
        Instance.time = _time;
        return Instance.time;
    }

    #region Adding, Minusing and Setting Time
    public TimeSpan AddMinutes(int amount) {
        Instance.time += TimeSpan.FromMinutes(amount);
        return time;
    }

    public TimeSpan AddHours(int amount) {
        Instance.time += TimeSpan.FromHours(amount);
        return time;
    }

    public TimeSpan MinusMinutes(int amount) {
        Instance.time -= TimeSpan.FromMinutes(amount);
        return time;
    }

    public TimeSpan MinusHours(int amount) {
        Instance.time -= TimeSpan.FromHours(amount);
        return time;
    }

    public TimeSpan SetDay() {
        Instance.time = TimeSpan.FromHours(6);
        return Instance.time;
    }

    public TimeSpan SetNight() {
        Instance.time = TimeSpan.FromHours(18);
        return Instance.time;
    }
    #endregion



    private int CheckTime() {
        int hour = Instance.time.Hours;

        return hour;
    }



    #region Coroutines
    private IEnumerator CountMinute() {
        // Run First In Game Minute without Timescale 
        yield return new WaitForSecondsRealtime(_inGameMinute);
        Instance.time += TimeSpan.FromMinutes(1);
        PlayerHUD.Instance.SetTime();
        CheckTime();

        //Continue with TimeScale
        while (true) {
            var waitTime = (_inGameMinute / clock.timeScale);

            //Debug.Log("In Game Minute : " + _inGameMinute + "\nTime Scale: " + clock.timeScale + "\nWait Time: " + waitTime);
            yield return new WaitForSecondsRealtime(waitTime);
            Instance.time += TimeSpan.FromMinutes(1);
            PlayerHUD.Instance.SetTime();
            CheckTime();
        }
    }
    #endregion

    public override string ToString() {

        return Instance.time.ToString();
    }
}
