using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boltz : MonoBehaviour
{
    public GameObject TopNutz;
    public int maxCapacity = 4;
    public int availableSpace;
    public int numOfNut;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        numOfNut = 0;
        if (HasNutz())
        {
            TopNutz = this.gameObject.transform.GetChild(this.gameObject.transform.childCount - 1).gameObject;
        }
        foreach (Transform child in this.gameObject.transform)
        {
            if (child.name.Contains("Nut"))
            {
                numOfNut++;
            }
        }
        availableSpace = maxCapacity - numOfNut;
    }
    public bool HasNutz ()
    {

        foreach(Transform child in this.gameObject.transform)
        {
            if (child.name.Contains("Nut"))
            {
                return true;
            }
        }
        return false;
    }
}
