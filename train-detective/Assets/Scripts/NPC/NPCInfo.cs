using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInfo : MonoBehaviour
{
    public string _characterName = ""; 
    public LayerMask _player;

    public void Awake(){
        _player = LayerMask.GetMask("Player");
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
