using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameSetup : MonoBehaviour
{
    public GameObject Boltz;
    public GameObject Nutz;
    public GameObject NutTab;
    public List<Color> NutzColor;
    public List<int> colorCounter;
    public List<GameObject> BoltzList;
    public List<GameObject> NutzList;
    public List<Vector3> BoltzPositionList;
    public float nutzHeight = 2.62f;
    public int numOfNutz = 4;
    public int NutzCount = 0;
    public int BoltIndex = 0;
    public int nutIndex = 0;
    public bool SetupFinished = false;
    // Start is called before the first frame update
    void Start()
    {
       CustomSetup();
     //  ManualSetup();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetNuts();
        }
    }
    Color RGB(byte r, byte g, byte b)
    {
        return new Color(r / 255f, g / 255f, b / 255f);
    }

    public void CustomSetup()
    {
        
        NutzColor = new List<Color> { Color.red, Color.yellow, Color.blue, RGB(255, 124, 237), RGB(255, 134, 0), RGB(102, 54, 0), RGB(0, 101, 1), RGB(0, 197, 255), RGB(202, 223, 218), RGB(140, 0, 130), RGB(159, 210, 21), RGB(62, 0, 149), RGB(0, 69, 64) };
        NutTab.SetActive(false);

    }

    public void SetNuts()
    {

        
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {

                if (hit.collider.tag == "Nut")
                {
                    nutIndex = 0;
                    foreach (Transform child in hit.collider.transform)
                    {
                        if (child.name.Contains("Nut"))
                        {
                            nutIndex++;
                        }
                    }
                    OpenNutTab();
                    NutTab.transform.Find("Which Bolts Text").GetComponent<TextMeshProUGUI>().text = hit.collider.name;
                }
            }
        
    }

    public void AddNuts(UnityEngine.UI.Button colorButton)
    {
        
        GameObject bolt = GameObject.Find(NutTab.transform.Find("Which Bolts Text").GetComponent<TextMeshProUGUI>().text);
        int maximumSpace = bolt.GetComponent<Boltz>().maxCapacity;
        Debug.Log(maximumSpace);
        if (nutIndex < maximumSpace)
        {
            GameObject nut = Instantiate(Nutz, new Vector3(bolt.transform.position.x, bolt.transform.position.y + 4.17f + (nutIndex * nutzHeight), bolt.transform.position.z), Quaternion.identity);
            nut.GetComponentInChildren<Renderer>().material.color = colorButton.image.color;
            nut.name = Nutz.name + " spawn:" + nutIndex;
            nut.transform.SetParent(bolt.transform);

            nutIndex++;
        }
    }
    public void ResetBolt()
    {

        GameObject bolt = GameObject.Find(NutTab.transform.Find("Which Bolts Text").GetComponent<TextMeshProUGUI>().text);
        foreach (Transform child in bolt.transform)
        {
            if (child.name.Contains("Nut"))
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        nutIndex = 0;

    }

    public void OpenNutTab()
    {
       
        NutTab.SetActive(true);

    }
    public void CloseNutTab()
    {

        NutTab.SetActive(false);
    }

    public void AddBolt ()
    {
        
        if (BoltIndex < 5)
        {
            
            GameObject SpawnedBolts = Instantiate(Boltz, new Vector3(BoltIndex * 10, 0, 0), Quaternion.identity);
            BoltzList.Add(SpawnedBolts);
            SpawnedBolts.name = Boltz.name + " spawn:" + BoltIndex;
            BoltzPositionList.Add(SpawnedBolts.transform.position);
            
        }
        if (BoltIndex < 10 && BoltIndex >= 5)
        {
            
            GameObject SpawnedBolts = Instantiate(Boltz, new Vector3((BoltIndex -5) * 10, 25, 0), Quaternion.identity);
            BoltzList.Add(SpawnedBolts);
            SpawnedBolts.name = Boltz.name + " spawn:" + BoltIndex;
            BoltzPositionList.Add(SpawnedBolts.transform.position);
            
        }
        if (BoltIndex < 15 && BoltIndex >= 10)
        {
            
            GameObject SpawnedBolts = Instantiate(Boltz, new Vector3((BoltIndex -10) * 10, 50, 0), Quaternion.identity);
            BoltzList.Add(SpawnedBolts);
            SpawnedBolts.name = Boltz.name + " spawn:" + BoltIndex;
            BoltzPositionList.Add(SpawnedBolts.transform.position);
            
        }

        if (BoltIndex >= 0 && BoltIndex < 15)
        {
            BoltIndex++;
        }
    }

    public void RemoveBolt()
    {

        if (BoltIndex > 0 && BoltIndex <= 15)
        {
            BoltIndex--;
            GameObject.Destroy(BoltzList[BoltIndex]);
            BoltzList.RemoveAt(BoltIndex);
            BoltzPositionList.RemoveAt(BoltIndex);

        }

    }

    public void ManualSetup ()
    {


        NutzColor = new List<Color> { Color.red, Color.yellow, Color.blue, RGB(255, 124, 237), RGB(255, 134, 0), RGB(102, 54, 0), RGB(0, 101, 1), RGB(0, 197, 255) };
        foreach (Color c in NutzColor)
        {

            colorCounter.Add(0);
        }
        for (int i = 0; i < 10; i++)
        {
            if (i <= 4)
            {
                GameObject spawnedBoltz = Instantiate(Boltz, new Vector3(i * 10, 0, 0), Quaternion.identity);

                BoltzList.Add(spawnedBoltz);
                BoltzPositionList.Add(BoltzList[i].transform.position);
                for (int x = 0; x < numOfNutz; x++)
                {
                    GameObject nut = Instantiate(Nutz, new Vector3(BoltzPositionList[i].x, BoltzPositionList[i].y + 4.17f + (x * nutzHeight), BoltzPositionList[i].z), Quaternion.identity);
                    nut.transform.SetParent(BoltzList[i].transform);
                    int randomInt = -1;

                    // Safety net in case all colors are exhausted
                    int maxTries = 100; // avoid infinite loop
                    int tryCount = 0;

                    while (tryCount < maxTries)
                    {
                        randomInt = Random.Range(0, NutzColor.Count);
                        if (colorCounter[randomInt] < 4)
                        {
                            colorCounter[randomInt]++;
                            Color randomColor = NutzColor[randomInt];
                            nut.GetComponentInChildren<Renderer>().material.color = randomColor;
                            break;
                        }

                        tryCount++;
                    }

                    if (tryCount == maxTries)
                    {
                        Debug.LogWarning("Could not find a valid color for nut (all colors used up?)");
                    }



                    NutzList.Add(nut);
                    NutzCount++;
                    NutzList[NutzCount - 1].name = Nutz.name + " spawn:" + NutzCount;
                }
                BoltzList[i].name = Boltz.name + " spawn:" + i;
            }
            else
            {

                GameObject spawnedBoltz = Instantiate(Boltz, new Vector3((i - 5) * 10, 25, 0), Quaternion.identity);
                BoltzList.Add(spawnedBoltz);
                BoltzPositionList.Add(BoltzList[i].transform.position);
                for (int x = 0; x < numOfNutz; x++)
                {
                    if (i <= 7)
                    {
                        GameObject nut = Instantiate(Nutz, new Vector3(BoltzPositionList[i].x, BoltzPositionList[i].y + 4.17f + (x * nutzHeight), BoltzPositionList[i].z), Quaternion.identity);
                        int randomInt = -1;

                        // Safety net in case all colors are exhausted
                        int maxTries = 100; // avoid infinite loop
                        int tryCount = 0;

                        while (tryCount < maxTries)
                        {
                            randomInt = Random.Range(0, NutzColor.Count);
                            if (colorCounter[randomInt] < 4)
                            {
                                colorCounter[randomInt]++;
                                Color randomColor = NutzColor[randomInt];
                                nut.GetComponentInChildren<Renderer>().material.color = randomColor;
                                break;
                            }

                            tryCount++;
                        }

                        if (tryCount == maxTries)
                        {
                            Debug.LogWarning("Could not find a valid color for nut (all colors used up?)");
                        }


                        nut.transform.SetParent(BoltzList[i].transform);
                        NutzList.Add(nut);

                        NutzCount++;
                        NutzList[NutzCount - 1].name = Nutz.name + " spawn:" + NutzCount;
                    }
                }
                BoltzList[i].name = Boltz.name + " spawn:" + i;
            }
        }

        SetupFinished = true;


    }

}
