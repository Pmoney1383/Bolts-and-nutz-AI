using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UI : MonoBehaviour
{
    public TextMeshProUGUI Text;

    // Start is called before the first frame update
    void Start()
    {
        Text = this.GetComponentInChildren<TextMeshProUGUI>();
        Text.text = "Click on bolt to edit nuts";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText (string text)
    {


        Text.text = text;

    }
}
