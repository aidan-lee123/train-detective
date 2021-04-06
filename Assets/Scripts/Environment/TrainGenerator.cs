using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainGenerator : MonoBehaviour
{

    private int trainLength;
    public int TrainLength {
        get {
            return trainLength;
        }
        set {
            trainLength = value;
        }
    }

    public GameObject[] cabinList;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateTrain() {


        for(int i = 0; i < trainLength; i++) {

        }
    }
}
