using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeKeeper : MonoBehaviour
{

    public float _inGameMinute;
    private int minute;
    public int Minute {
        get {
            return minute;
        }
    }
    

    public static TimeKeeper Instance { get; set; }

    private void Awake() {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        StartCoroutine(CountMinute());
    }

    public int AddMinutes(int amount) {
        minute += amount;
        return minute;
    }

    public int MinusMinutes(int amount) {
        minute += amount;
        return minute;
    }

    private IEnumerator CountMinute() {

        while (true) {
            yield return new WaitForSecondsRealtime(_inGameMinute);
            minute++;
        }
    }
}
