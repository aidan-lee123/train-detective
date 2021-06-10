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
    public float duration;
    public TimeSlot time;
    public Action action;

    public Activity(Vector2 _location, float _duration, TimeSlot _time, Action _action) {
        location = _location;
        duration = _duration;
        time = _time;
        action = _action;
    }
}

public class TimeSlot {

}
