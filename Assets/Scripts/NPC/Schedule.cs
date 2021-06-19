using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schedule : MonoBehaviour
{

    public List<Activity> ActivityList { get; set; }
    public Activity currentActivity { get; set; }

    void Awake()
    {
        ActivityList = new List<Activity>();
    }

    void Update()
    {
        if(ActivityList.Count > 0) {
            ProcessQueue();
        }
    }

    public void AddToQueue(Activity activity) {
        ActivityList.Add(activity);
    }

    void ProcessQueue() {

        if (!ActivityList[0].Finished()) {
            ActivityList[0].PerformAction();
        } else {
            ActivityList.RemoveAt(0);
        }
    }
}
