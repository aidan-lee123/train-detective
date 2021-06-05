using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHUD : MonoBehaviour
{

    public static PlayerHUD Instance { get; set; }

    public TMP_Text clock;
    public TMP_Text money;

    private void Awake() {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void SetTime() {
        clock.text = TimeManager.Instance.ToString();
    }

    public void SetMoney() {
        money.text = "$" + Player.Instance.Money.ToString();
    }

    void Update()
    {
        
    }
}
