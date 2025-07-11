using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class GameState : MonoBehaviour
{
    [SerializeField]
    public List<List<GameObject>> State;
    public List<List<GameObject>> previousState;
    public List<Color> Nutcolors;
    public GameObject nutz;
    public int MoveFrom;
    public int MoveTo;
    public GameSetup gameSetup;
    // Start is called before the first frame update
    void Start()
    {
        Nutcolors = this.gameObject.GetComponent<GameSetup>().NutzColor;
        
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.T))
        {

            setGameState("XXXX|XXXX|1222|6036|6726|7310|5417|5407|5443|5310", getState());

           // setGameState("222X|XXXX|1XXX|2222|2222|2222|2222|2222|2111|2222", getState());
           // Debug.Log(SerializeState(getState()));
        }
       if (Input.GetKeyDown(KeyCode.Y))
        {


            AllNextStateFromCurrentLight(SerializeState(getState()));
           // AllNextStateFromCurrent(getState());
        }

    }
    public GameState( List<List<GameObject>> state, List<List<GameObject>> previousState, int moveFrom, int moveTo)
    {
        
        State = state;
        this.previousState = previousState;
        MoveFrom = moveFrom;
        MoveTo = moveTo;
    }

    public List<List<GameObject>> getState()
    {
        State = new List<List<GameObject>>();


        foreach (Boltz bolt in GameObject.FindObjectsOfType<Boltz>())
        {
            
            State.Add(EachBolt(bolt));


        }

        
        return State;
    }
    public List<List<GameObject>> getStateWithDebug()
    {
        State = new List<List<GameObject>>();


        foreach (Boltz bolt in GameObject.FindObjectsOfType<Boltz>())
        {

            State.Add(EachBolt(bolt));


        }

        for (int i = 0; i < State.Count; i++)
        {
            for (int j = 0; j < State[i].Count; j++)
            {
                Debug.Log(State[i][j]);
            }
        }
        return State;
    }

    public void setGameState(string hash, List<List<GameObject>> currentGameState)
    {


        string[] boltStrings = hash.Split('|');
        for (int boltIndex = 0; boltIndex < boltStrings.Length; boltIndex++)
        {

            string boltData = boltStrings[boltIndex];
            if (boltIndex >= currentGameState.Count) break;

            List<GameObject> bolt = currentGameState[boltIndex];

            for (int nutIndex = 0; nutIndex < boltData.Length; nutIndex++)
            {
                char c = boltData[nutIndex];
                if (c == 'X')
                {

                    GameObject.Destroy(bolt[nutIndex]);
                    continue;

                }
                int colorIndex = int.Parse(c.ToString());
                Color nutColor = Nutcolors[colorIndex];
                if (bolt[nutIndex] != null)
                {
                    bolt[nutIndex].GetComponentInChildren<Renderer>().material.color = nutColor;
                }
                else
                {
                    Vector3 pos = gameSetup.BoltzList[gameSetup.BoltzList.Count - boltIndex - 1].transform.position + new Vector3(0, 4.17f + nutIndex * gameSetup.nutzHeight, 0);
                    GameObject nut = Instantiate(nutz, pos, Quaternion.identity);
                    nut.GetComponentInChildren<Renderer>().material.color = nutColor;
                    nut.transform.SetParent(gameSetup.BoltzList[gameSetup.BoltzList.Count - boltIndex - 1].transform);

                }
            }
        }


    }


    public bool IsGoaLstate(string state)
    {
        string[] boltStrings = state.Split('|');
        foreach (var bolt in boltStrings)
        {
            // Skip empty bolts
            if (bolt.All(n => n == 'X'))
            {

                continue;
            }


            char targetColor = bolt[0];

            foreach (var nut in bolt)
            {
                if (nut == 'X')
                {
                    return false;
                }
                char nutcolor = nut;
                if (nutcolor != targetColor)
                {
                    return false;
                }
            }

        }

        return true;

    }
    public bool IsGoaLstate (List<List<GameObject>> state)
    {
        
        foreach(var bolt in state)
        {
            // Skip empty bolts
            if (bolt.All(n => n == null))
            {
                
                continue;
            }


            Color targetColor = bolt[0].GetComponentInChildren<Renderer>().material.color;

            foreach(var nut in bolt)
            {
                if (nut == null)
                {
                  return false;
                }
                Color nutcolor = nut.GetComponentInChildren<Renderer>().material.color;
                if (nutcolor != targetColor)
                {
                    return false;
                }
            }

        }

        return true;

    }
    public List<List<List<GameObject>>> AllNextStateFromCurrent(List<List<GameObject>> currentState)
    {
        List<List<List<GameObject>>> nextStates = new List<List<List<GameObject>>>();

        for (int from = 0; from < currentState.Count; from++)
        {
            List<GameObject> nutsToMove = GetTopSameColorBlock(currentState[from]);
            if (nutsToMove.Count == 0) continue;

            
            if (IsFullAndUniformColor(currentState[from]))
            {
                continue; // Skip this bolt, it's already solved
            }
            for (int to = 0; to < currentState.Count; to++)
            {
                if (from == to) continue;

                GameObject targetTopNut = null;
                int availableSpace = 0;

                // Find top nut and space for target bolt
                for (int i = currentState[to].Count - 1; i >= 0; i--)
                {
                    if (currentState[to][i] != null)
                    {
                        targetTopNut = currentState[to][i];
                        break;
                    }
                }
                availableSpace = currentState[to].Count(n => n == null);
                
                if (ValidMove(nutsToMove, targetTopNut, availableSpace))
                {
                    // Clone state
                    List<List<GameObject>> newState = CloneState(currentState);

                    // Move the nut
                    List<GameObject> movedBlock = RemoveTopNutBlock(newState[from]);
                    AddNutBlockToBolt(newState[to], movedBlock);

                   // Debug.Log(SerializeState(newState));
                    nextStates.Add(newState);
                }
            }      
        }
        return nextStates;
    }

    public List<string> AllNextStateFromCurrentLight(string currentState)
    {
       List<string> nextStates = new List<string>();
        string[] boltStrings = currentState.Split('|');
        for (int from = 0; from < boltStrings.Length; from++)
        {
            string nutsToMove = GetTopSameColorBlockLight(boltStrings[from]);
            if (nutsToMove.Length == 0) continue;


            if (IsFullAndUniformColorLight(boltStrings[from]))
            {
                continue; // Skip this bolt, it's already solved
            }
            for (int to = 0; to < boltStrings.Length; to++)
            {
                if (from == to) continue;

                char targetTopNut = 'X';
                int availableSpace = 0;

                // Find top nut and space for target bolt
                for (int i = boltStrings[to].Length - 1; i >= 0; i--)
                {
                    if (boltStrings[to][i] != 'X')
                    {
                        targetTopNut = boltStrings[to][i];
                        break;
                    }
                }
                availableSpace = boltStrings[to].Count(n => n == 'X');

                if (ValidMoveLight(nutsToMove, targetTopNut, availableSpace))
                {
                    // Clone state
                    string newState = CloneStateLight(currentState);
                    string[] boltStringsNew = newState.Split('|');

                    // Remove & Add must return updated bolts
                    string boltFrom = boltStringsNew[from];
                    string boltTo = boltStringsNew[to];

                    string movedBlock = RemoveTopNutBlockLight(ref boltFrom);
                    boltTo = AddNutBlockToBoltLight(boltTo, movedBlock);

                    // Save back to array
                    boltStringsNew[from] = boltFrom;
                    boltStringsNew[to] = boltTo;

                    string finalState = string.Join("|", boltStringsNew);

                   // Debug.Log(finalState);
                    nextStates.Add(finalState);

                }
            }
        }
        return nextStates;
    }
    private bool IsFullAndUniformColor(List<GameObject> bolt)
    {
        // Make sure bolt has no nulls (i.e., it's full)
        if (bolt.Any(nut => nut == null))
            return false;

        // Get the color of the first nut
        Color targetColor = bolt[0].GetComponentInChildren<Renderer>().material.color;

        // Check if all nuts have the same color
        foreach (var nut in bolt)
        {
            if (nut.GetComponentInChildren<Renderer>().material.color != targetColor)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsFullAndUniformColorLight(string bolt)
    {
        // Make sure bolt has no nulls (i.e., it's full)
        if (bolt.Any(nut => nut == 'X'))
            return false;

        // Get the color of the first nut
        char targetColor = bolt[0];

        // Check if all nuts have the same color
        foreach (var nut in bolt)
        {
            if (nut != targetColor)
            {
                return false;
            }
        }

        return true;
    }

    public string SerializeState (List<List<GameObject>> State)
    {
        string result = "";

        foreach (var bolt in State) {
        
            foreach(var nut in bolt)
            {

                if (nut == null)
                {

                    result += "X";

                }else
                {

                    Color color = nut.GetComponentInChildren<Renderer>().material.color;
                    result += ColorToCode(color);


                }

            }

            result += "|";
        
        }

        result = result.Remove(result.Length - 1);

        return result;
    }

    public string DeserializeState(List<List<GameObject>> State)
    {
        string result = "";

        foreach (var bolt in State)
        {

            foreach (var nut in bolt)
            {

                if (nut == null)
                {

                    result += "X";

                }
                else
                {

                    Color color = nut.GetComponentInChildren<Renderer>().material.color;
                    result += ColorToCode(color);


                }

            }

            result += "|";

        }


        return result;
    }
    public string ColorToCode (Color C)
    {

        for (int i = 0; i < Nutcolors.Count; i++)
        {

            if (C == Nutcolors[i])
            {

                return i.ToString();

            }

        } 
        return "?";
    }


    private List<GameObject> GetTopSameColorBlock(List<GameObject> bolt)
    {
        List<GameObject> block = new List<GameObject>();

        // Start from the top (end of the list)
        for (int i = bolt.Count - 1; i >= 0; i--)
        {
            if (bolt[i] == null)
                continue;

            if (block.Count == 0)
            {
                block.Add(bolt[i]);
            }
            else
            {
                Color currentColor = bolt[i].GetComponentInChildren<Renderer>().material.color;
                Color topColor = block[0].GetComponentInChildren<Renderer>().material.color;

                if (currentColor == topColor)
                {
                    block.Add(bolt[i]);
                }
                else
                {
                    break;
                }
            }
        }

        block.Reverse(); // Optional: keeps order from bottom to top
        return block;
    }

    private string GetTopSameColorBlockLight(string bolt)
    {
        string block = "";

        // Start from the top (end of the list)
        for (int i = bolt.Length - 1; i >= 0; i--)
        {
            if (bolt[i] == 'X')
                continue;

            if (block.Length == 0)
            {
                block += bolt[i];
            }
            else
            {

                char currentColor = bolt[i];
                char topColor = block[0];

                if (currentColor == topColor)
                {
                    block +=bolt[i];
                }
                else
                {
                    break;
                }
            }
        }

        block.Reverse(); // Optional: keeps order from bottom to top
        return block;
    }

    private List<GameObject> RemoveTopNutBlock(List<GameObject> bolt)
    {
        List<GameObject> block = new List<GameObject>();

        // Identify the top block of same-color nuts
        Color? topColor = null;

        for (int i = bolt.Count - 1; i >= 0; i--)
        {
            if (bolt[i] == null)
                continue;

            Color currentColor = bolt[i].GetComponentInChildren<Renderer>().material.color;

            if (topColor == null)
            {
                topColor = currentColor;
                block.Add(bolt[i]);
                bolt[i] = null;
            }
            else if (currentColor == topColor)
            {
                block.Add(bolt[i]);
                bolt[i] = null;
            }
            else
            {
                break;
            }
        }

        block.Reverse(); // To keep the correct bottom-to-top order
        return block;
    }

    private string RemoveTopNutBlockLight(ref string bolt)
    {
        string block = "";
        char? topColor = 'X';

        for (int i = bolt.Length - 1; i >= 0; i--)
        {
            if (bolt[i] == 'X') continue;

            char currentColor = bolt[i];
            if (topColor == 'X')
            {
                topColor = currentColor;
                block = currentColor + block;
                bolt = bolt.Substring(0, i) + 'X' + bolt.Substring(i + 1);
            }
            else if (currentColor == topColor)
            {
                block = currentColor + block;
                bolt = bolt.Substring(0, i) + 'X' + bolt.Substring(i + 1);
            }
            else break;
        }

        return block;
    }


    private void AddNutBlockToBolt(List<GameObject> bolt, List<GameObject> nuts)
    {
        int index = 0;
        for (int i = 0; i < bolt.Count && index < nuts.Count; i++)
        {
            if (bolt[i] == null)
            {
                bolt[i] = nuts[index];
                index++;
            }
        }
    }

    private string AddNutBlockToBoltLight(string bolt, string nuts)
    {
        int index = 0;
        char[] chars = bolt.ToCharArray();
        for (int i = 0; i < chars.Length && index < nuts.Length; i++)
        {
            if (chars[i] == 'X')
            {
                chars[i] = nuts[index];
                index++;
            }
        }
        return new string(chars);
    }


    private List<List<GameObject>> CloneState(List<List<GameObject>> original)
    {
        List<List<GameObject>> clone = new List<List<GameObject>>();

        foreach (var bolt in original)
        {
            List<GameObject> newBolt = new List<GameObject>();
            foreach (var nut in bolt)
            {
                newBolt.Add(nut); // shallow copy (still works for logic)
            }
            clone.Add(newBolt);
        }

        return clone;
    }

    private string CloneStateLight(string original)
    {
        string clone = "";
        string[] boltStrings = original.Split('|');
        foreach (var bolt in boltStrings)
        {
            string newBolt = "";
            foreach (var nut in bolt)
            {
                newBolt += nut; // shallow copy (still works for logic)
            }
            
            clone+= newBolt;
            clone += '|';
        }
            clone = clone.Remove(clone.Length - 1);
        return clone;
    }
    public bool ValidMove(List<GameObject> selectedNuts, GameObject targetTopNut, int availableSpace)
    {

        if (selectedNuts == null || selectedNuts.Count == 0)
            return false;

        if (availableSpace < selectedNuts.Count)
            return false;

        if (targetTopNut == null)
        {
           
            return true;
        }

        Color selectedColor = selectedNuts[0].GetComponentInChildren<Renderer>().material.color;
        Color targetColor = targetTopNut.GetComponentInChildren<Renderer>().material.color;

        return selectedColor == targetColor;
    }

    public bool ValidMoveLight(string selectedNuts, char targetTopNut, int availableSpace)
    {

        if (selectedNuts == null || selectedNuts.Length == 0)
            return false;

        if (availableSpace < selectedNuts.Length)
            return false;

        if (targetTopNut == 'X')
        {

            return true;
        }

        char selectedColor = selectedNuts[0];
        char targetColor = targetTopNut;

        return selectedColor == targetColor;
    }

    public List<GameObject> EachBolt(Boltz bolt) {

        List<GameObject> list = new List<GameObject>();
        if (bolt.TopNutz == null)
        {
            list.Add(null);
            list.Add(null);
            list.Add(null);
            list.Add(null);
            return list;
        }
        foreach (Transform child in bolt.gameObject.transform)
        {
            if (child.name.Contains("Nut"))
            {
                list.Add(child.gameObject);
                
                }
            
        }

        for (int i = 0; i < list.Count; i++)
        {
            if (list.Count < 4)
            {
                list.Add(null);
                
            }
        }
        return list;

    }

}
