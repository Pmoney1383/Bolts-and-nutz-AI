using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject CurrentNut;
    public GameObject SelectedBolt;
    public List<NutSelectorAnim> nutSelectorAnims;
    public Boolean nutSelected = false;
    // Start is called before the first frame update
    void Start()
    {     
            NutSelectorAnim[] anims = GameObject.FindObjectsOfType<NutSelectorAnim>();
            foreach (NutSelectorAnim anim in anims)
            {
                nutSelectorAnims.Add(anim);
            }
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (!nutSelected)
                {
                    if (hit.collider.tag == "Nut")
                    {
                        CurrentNut = hit.collider.GetComponent<Boltz>().TopNutz;
                        nutSelected = true;
                        if (CurrentNut != null)
                        {
                            CurrentNut.GetComponent<NutSelectorAnim>().ToggleSelect();
                            
                        }
                    }

                    foreach (NutSelectorAnim anim in nutSelectorAnims)
                    {

                        if (anim != CurrentNut.GetComponent<NutSelectorAnim>())
                        {
                            if (anim.isSelected)
                            {
                                anim.ToggleSelect(); // Deselect any previously selected nut
                            }
                        }

                    }
                    return;
                }
                if (nutSelected)
                {
                    
                    if (hit.collider.tag == "Nut" && hit.collider.GetComponent<Boltz>().availableSpace > 0 && hit.collider.gameObject != CurrentNut.transform.parent.gameObject)
                    {
                        if (hit.collider.GetComponent<Boltz>().numOfNut >= 1)
                        {
                            if (CurrentNut.GetComponentInChildren<Renderer>().material.color != hit.collider.GetComponent<Boltz>().TopNutz.GetComponentInChildren<Renderer>().material.color)
                            {
                                return;
                            }
                        }
                        SelectedBolt = hit.collider.gameObject;
                        // Assuming CurrentNut is selected and valid
                        StartCoroutine(CurrentNut.GetComponent<NutMoverAnim>().MoveToBolt(CurrentNut, SelectedBolt));
                        CurrentNut.GetComponent <NutSelectorAnim>().isSelected = false;
                        CurrentNut = null;
                        nutSelected = false;

                    } else
                    {
                        nutSelected = false;
                        
                        
                        if (hit.collider.tag == "Nut")
                        {
                            CurrentNut = hit.collider.GetComponent<Boltz>().TopNutz;
                            nutSelected = true;
                            if (CurrentNut != null)
                            {
                                CurrentNut.GetComponent<NutSelectorAnim>().ToggleSelect();
                            }
                        }

                        foreach (NutSelectorAnim anim in nutSelectorAnims)
                        {

                            if (anim != CurrentNut.GetComponent<NutSelectorAnim>())
                            {
                                if (anim.isSelected)
                                {
                                    anim.ToggleSelect(); // Deselect any previously selected nut
                                }
                            }

                        }
                        return;
                    }
                }
            }
        }
    }
}
