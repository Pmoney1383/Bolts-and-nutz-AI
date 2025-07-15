using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameSetup : MonoBehaviour
{
    public GameObject Boltz3;
    public GameObject Boltz4;
    public GameObject Boltz5;
    public GameObject Nutz;
    public GameObject NutTab;
    public List<Color> NutzColor;
    public List<int> colorCounter;
    public List<GameObject> BoltzList;
    public List<GameObject> NutzList;
    public List<Vector3> BoltzPositionList;
    public Dropdown selectSize;
    public float nutzHeight = 2.62f;
    public int numOfNutz = 4;
    public int NutzCount = 0;
    public int BoltIndex = 0;
    public int nutIndex = 0;
    public bool SetupFinished = false;
    public UI GameUi;
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
        selectSize.ClearOptions();
        List<string> SizeOptions = new List<string>() { "3", "4", "5" };
        selectSize.AddOptions(SizeOptions);
        selectSize.value = 1;

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
                    int whichBolt = int.Parse (hit.collider.name.Substring(14)) + 1;
                    GameUi.SetText("editing boltz " + whichBolt);
                }
            }
        
    }

    public void AddNuts(UnityEngine.UI.Button colorButton)
    {
        
        GameObject bolt = GameObject.Find(NutTab.transform.Find("Which Bolts Text").GetComponent<TextMeshProUGUI>().text);
        int maximumSpace = bolt.GetComponent<Boltz>().maxCapacity;
       
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

    public void ChooseSizeBolt ()
    {

        int size =  int.Parse(selectSize.options[selectSize.value].text);
        GameUi.SetText("Click on bolt to edit nuts");

        if (size == 3)
        {

            foreach (GameObject bolt in BoltzList)
            {

                GameObject.Destroy(bolt);



            }

            for (int i =0; i < BoltzPositionList.Count; i++)
            {

                GameObject newBolt = Instantiate(Boltz3, BoltzPositionList[i], Quaternion.identity);
                if (newBolt.GetComponent<Boltz>() == null)
                {
                    newBolt.AddComponent<Boltz>();
                }
                newBolt.GetComponent<Boltz>().maxCapacity = 3;
                newBolt.name = Boltz3.name + " spawn:" + i;
                newBolt.tag = "Nut";
                BoltzList[i] = newBolt;



            }
            

        }
        if (size == 4)
        {

            foreach (GameObject bolt in BoltzList)
            {

                GameObject.Destroy(bolt);



            }

            for (int i = 0; i < BoltzPositionList.Count; i++)
            {

                GameObject newBolt = Instantiate(Boltz4, BoltzPositionList[i], Quaternion.identity);
                if (newBolt.GetComponent<Boltz>() == null)
                {
                    newBolt.AddComponent<Boltz>();
                }
                newBolt.GetComponent<Boltz>().maxCapacity = 4;
                newBolt.name = Boltz4.name + " spawn:" + i;
                newBolt.tag = "Nut";
                BoltzList[i] = newBolt;



            }


        }
        if (size == 5)
        {

            foreach (GameObject bolt in BoltzList)
            {

                GameObject.Destroy(bolt);



            }

            for (int i = 0; i < BoltzPositionList.Count; i++)
            {

                GameObject newBolt = Instantiate(Boltz5, BoltzPositionList[i], Quaternion.identity);
                if (newBolt.GetComponent<Boltz>() == null)
                {
                    newBolt.AddComponent<Boltz>();
                }
                newBolt.GetComponent<Boltz>().maxCapacity = 5;
                newBolt.name = Boltz5.name + " spawn:" + i;
                newBolt.tag = "Nut";
                BoltzList[i] = newBolt;



            }


        }
        CloseNutTab();
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
            
            GameObject SpawnedBolts = Instantiate(Boltz4, new Vector3(BoltIndex * 10, 0, 0), Quaternion.identity);
            BoltzList.Add(SpawnedBolts);
            SpawnedBolts.name = Boltz4.name + " spawn:" + BoltIndex;
            BoltzPositionList.Add(SpawnedBolts.transform.position);
            
        }
        if (BoltIndex < 10 && BoltIndex >= 5)
        {
            
            GameObject SpawnedBolts = Instantiate(Boltz4, new Vector3((BoltIndex -5) * 10, 25, 0), Quaternion.identity);
            BoltzList.Add(SpawnedBolts);
            SpawnedBolts.name = Boltz4.name + " spawn:" + BoltIndex;
            BoltzPositionList.Add(SpawnedBolts.transform.position);
            
        }
        if (BoltIndex < 15 && BoltIndex >= 10)
        {
            
            GameObject SpawnedBolts = Instantiate(Boltz4, new Vector3((BoltIndex -10) * 10, 50, 0), Quaternion.identity);
            BoltzList.Add(SpawnedBolts);
            SpawnedBolts.name = Boltz4.name + " spawn:" + BoltIndex;
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
                GameObject spawnedBoltz = Instantiate(Boltz4, new Vector3(i * 10, 0, 0), Quaternion.identity);

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
                BoltzList[i].name = Boltz4.name + " spawn:" + i;
            }
            else
            {

                GameObject spawnedBoltz = Instantiate(Boltz4, new Vector3((i - 5) * 10, 25, 0), Quaternion.identity);
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
                BoltzList[i].name = Boltz4.name + " spawn:" + i;
            }
        }

        SetupFinished = true;


    }

}
