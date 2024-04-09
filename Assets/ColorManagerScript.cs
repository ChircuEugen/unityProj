using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ColorManagerScript : MonoBehaviour
{
    public Button button;
    private float red = 120f;
    private float green = 210f;
    private float blue = 90f;
    private float alpha = 200f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        ChangeButtonColor();
    }

    public void ChangeButtonColor()
    {
        Color newColor = new Color(red, green, blue, alpha);
        Image buttonImage = button.GetComponent<Image>();
        buttonImage.color = newColor;
    }

    
}
