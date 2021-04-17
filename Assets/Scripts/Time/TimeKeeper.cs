using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeKeeper : MonoBehaviour
{
    //How many real seconds is an In Game Minute
    public float _inGameMinute = 5f;

    private TimeSpan time;
    public TimeSpan Time {
        get {
            return time;
        }
    }
    public int Minute {
        get {
            return time.Minutes;
        }
    }
    public int Hour {
        get {
            return time.Hours;
        }
    }

    public static TimeKeeper Instance { get; set; }

    private void Awake() {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
        time = TimeSpan.FromMinutes(0);
        StartCoroutine(CountMinute());
    }

    public TimeSpan SetTime(TimeSpan _time) {
        time = _time;
        return time;
    }

    #region Adding and Minusing from Time
    public TimeSpan AddMinutes(int amount) {
        time += TimeSpan.FromMinutes(amount);
        return time;
    }

    public TimeSpan AddHours(int amount) {
        time += TimeSpan.FromHours(amount);
        return time;
    }

    public TimeSpan MinusMinutes(int amount) {
        time -= TimeSpan.FromMinutes(amount);
        return time;
    }

    public TimeSpan MinusHours(int amount) {
        time -= TimeSpan.FromHours(amount);
        return time;
    }
    #endregion

    private IEnumerator CountMinute() {

        while (true) {
            yield return new WaitForSecondsRealtime(_inGameMinute);
            time += TimeSpan.FromMinutes(1);
            Debug.Log(time.ToString());
        }
    }

    public override string ToString() {

        return time.ToString();
    }
}
