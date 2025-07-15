using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UI : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public GameObject solveButton;
    public GameObject showPathButton;
    // Start is called before the first frame update
    void Start()
    {
        Text = this.GetComponentInChildren<TextMeshProUGUI>();
        SetShowButton(false);
        SetSolveButton(true);
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

    public void SetSolveButton(bool active)
    {

        solveButton.SetActive(active);

    }

    public void SetShowButton(bool active)
    {

        showPathButton.SetActive(active);

    }
}
