using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInspectItem : MonoBehaviour
{
    public Image itemImage;
    public TMP_Text itemName;
    public TMP_Text itemDesc;
    private Sprite itemSprite;

    // Start is called before the first frame update
    void Awake()
    {
        Hide(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItem(Item item) {
        itemSprite = item.icon;

        itemImage.sprite = itemSprite;
        itemName.text = item.title;

        Hide(false);
    }

    public void Hide(bool state) {

        RectTransform rect = GetComponent<RectTransform>();

        if (state) {
            rect.localScale = new Vector3(0, 0, 0);
        }
        else {
            rect.localScale = new Vector3(1, 1, 1);
        }
    }

}
