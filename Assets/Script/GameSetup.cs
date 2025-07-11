using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameSetup : MonoBehaviour
{
    public GameObject Boltz;
    public GameObject Nutz;
    public List<Color> NutzColor;
    public List<int> colorCounter;
    public List<GameObject> BoltzList;
    public List<GameObject> NutzList;
    public List<Vector3> BoltzPositionList;
    public float nutzHeight = 2.62f;
    public int numOfNutz = 4;
    public int NutzCount = 0;
    public bool SetupFinished = false;
    // Start is called before the first frame update
    void Start()
    {
        NutzColor = new List<Color> { Color.red,Color.yellow,Color.blue, RGB(255,124,237), RGB(255,134,0),RGB(102,54,0),RGB (0,101,1),RGB (0,197,255)};
        foreach (Color c in NutzColor)
        {

            colorCounter.Add(0);
        }
        for (int i = 0; i < 10; i++)
        {
            if (i <= 4)
            {
               GameObject spawnedBoltz =  Instantiate(Boltz, new Vector3(i * 10, 0, 0), Quaternion.identity); 

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
            } else
            {

               GameObject spawnedBoltz = Instantiate(Boltz, new Vector3((i-5) * 10, 25, 0), Quaternion.identity);
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

    // Update is called once per frame
    void Update()
    {
        
    }
    Color RGB(byte r, byte g, byte b)
    {
        return new Color(r / 255f, g / 255f, b / 255f);
    }

}
