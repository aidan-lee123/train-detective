using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scheduler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class Schedule {


}

public class Activity {

    public Vector2 location;
    public TimeSlot time;
    public Action action;

    public Activity(Vector2 _location, float _startTime, float _duration, Action _action) {
        location = _location;
        time = new TimeSlot(_startTime, _duration);
        action = _action;
    }

    public void PerformAction() {
        action.Invoke();
    }
}

public class TimeSlot {


    public float startTime;
    public float duration;

    public TimeSlot(float _startTime, float _duration) {
        startTime = _startTime;
        duration = _duration;
    }
}
