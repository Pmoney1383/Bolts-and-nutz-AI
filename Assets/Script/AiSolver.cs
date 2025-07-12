using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AiSolver : MonoBehaviour
{
    public  Queue <List<List<GameObject>>> queue = new Queue <List<List<GameObject>>>();
    public List<List<List<GameObject>>> AllNextState = new List<List<List<GameObject>>>();
    public Queue<string> queueLight = new Queue<string>();
    public List<string> AllNextStateLight = new List<string>();
    public List<List<GameObject>> RootgameState;
    public string RootGameStateLight;
    public GameState GameState;
    public List<GameNode> gameNodes = new List<GameNode>();
    public GameNode GoalState;
    public List<string> path = new List<string>();
    public bool GoalReached = false;
    public bool RootReached = false;
    // Start is called before the first frame update
    void Start()
    {
        
        GameState = this.GetComponent<GameState>();
       
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(UpdateText());
             StartCoroutine(GetAllPossibleStateLight());
            //  StartCoroutine(GetAllPossibleState());
            //  Debug.Log(GameState.SerializeState(gameNodes[gameNodes.Count - 1].State));
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (path.Count > 0)
            {
                StartCoroutine(ShowHowToSolve());

            }
            

        }
    }
    public IEnumerator UpdateText()
    {
        string text = "";

        UI gameUI = GameObject.Find("Canvas").GetComponent<UI>();  
        gameUI.SetText (text);
        while (!GoalReached)
        {
            text = "solving this make take a few minutes";
            gameUI.SetText(text);
            yield return new WaitForSeconds(1);
            text = "solving this make take a few minutes .";
            gameUI.SetText(text);
            yield return new WaitForSeconds(1);
            text = "solving this make take a few minutes ..";
            gameUI.SetText(text);
            yield return new WaitForSeconds(1);
            text = "solving this make take a few minutes ...";
            gameUI.SetText(text);
            yield return new WaitForSeconds(1);

        }
        text = "";
        gameUI.SetText(text);
    }
    public IEnumerator GetAllPossibleState ()
    {
        HashSet<string> visited = new HashSet<string>();
        RootgameState = GameState.getState();
        queue.Enqueue(RootgameState);
        visited.Add(GameState.SerializeState(RootgameState));
        int safetyCounter = 0;
        int maxIterations = 1000000; // Cap this to something safe while testing

        while (queue.Count > 0 && safetyCounter < maxIterations && !GoalReached)
        {

            List<List<GameObject>> currentGameState = queue.Dequeue();
            
            
            

            AllNextState = GameState.AllNextStateFromCurrent(currentGameState);
            foreach (var node in AllNextState)
            {
                if (GameState.IsGoaLstate(node))
                {
                    Debug.Log(" Goal state reached at iteration: " + safetyCounter);
                    Debug.Log(GameState.SerializeState(node));
                    GoalReached = true;
                    break;
                }
                string hash = GameState.SerializeState(node);
                
                if (!visited.Contains(hash))
                {
                    visited.Add(hash);
                    queue.Enqueue(node);
                    //gameNodes.Add(new GameNode(node, currentGameState));
                   // Debug.Log(hash);
                } else
                {
                    if (GameState.IsGoaLstate(node))
                    {

                        Debug.Log("goal was skipped by mistake");

                    }
                    Debug.Log("some state were duplicate");

                }
                

            }
            safetyCounter++;

            // Let Unity breathe every 500 iterations
            if (safetyCounter % 500 == 0)
            {
                Debug.Log(safetyCounter);
                Resources.UnloadUnusedAssets();
                yield return null;
            }
        }
        Debug.Log("Total Possible moves from initial state:" + gameNodes.Count);
        
    }


    public IEnumerator GetAllPossibleStateLight()
    {
        HashSet<string> visited = new HashSet<string>();
        RootgameState = GameState.getState();
        RootGameStateLight = GameState.SerializeState(RootgameState);
        queueLight.Enqueue(GameState.SerializeState(RootgameState));
        visited.Add(GameState.SerializeState(RootgameState));
        
        int safetyCounter = 0;
        int maxIterations = 100000000;

        while (queueLight.Count > 0 && safetyCounter < maxIterations && !GoalReached)
        {
            string currentGameState = queueLight.Dequeue();

           

            

            AllNextStateLight = GameState.AllNextStateFromCurrentLight(currentGameState);

            foreach (var node in AllNextStateLight)
            {
                if (GameState.IsGoaLstate(node))
                {
                    Debug.Log(" Goal state reached at iteration: " + safetyCounter);
                    Debug.Log(node);
                    GameState.setGameState(node, GameState.getState());
                    gameNodes.Add(new GameNode(node, currentGameState));
                    GoalReached = true;
                    break;
                }

                if (!visited.Contains(node))
                {
                    visited.Add(node);
                    queueLight.Enqueue(node);
                    gameNodes.Add(new GameNode(node, currentGameState));
                }
                else
                {
                    // Optional debugging
                    // Debug.Log("some state were duplicate");
                }
            }

            safetyCounter++;
            
            // Let Unity breathe every 500 iterations
            if (safetyCounter % 500 == 0)
            {
               // Debug.Log(safetyCounter);
                
                GameState.setGameState(currentGameState, GameState.getState());
                Resources.UnloadUnusedAssets();
                yield return null;
            }
        }

        Debug.Log("Total states checked: " + visited.Count);
        Debug.Log("total state added" + gameNodes.Count);
        FindPath();
    }
    
    public void FindPath()
    {
        // Step 1: Find the goal state
        for (int i = gameNodes.Count - 1; i >= 0; i--)
        {
            if (GameState.IsGoaLstate(gameNodes[i].state))
            {
                GoalState = gameNodes[i];
                break;
            }
        }

        if (GoalState == null)
        {
            Debug.LogError("Goal state not found!");
            return;
        }

        // Step 2: Trace path back to root
        string current = GoalState.state;
        while (!RootReached)
        {
            path.Add(current);

            if (current == RootGameStateLight)
            {
                RootReached = true;
                path.Reverse();
                Debug.Log("Path length: " + path.Count);
                break;
            }

            GameNode node = gameNodes.Find(n => n.state == current);
            if (node == null)
            {
                Debug.LogError("No GameNode found for state: " + current);
                break;
            }

            current = node.previousstate;
        }
    }

   public  IEnumerator  ShowHowToSolve()
    {

        bool allSolved = false;
        int index = 0;
        int maxIndex = path.Count;


        GameState.setGameState(path[0], GameState.getState());

        yield return null;
    
        while (!allSolved)
        {

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (index < maxIndex -1)
                {
                    index++;
                    GameState.setGameState(path[index], GameState.getState());
                    
                    yield return null;

                } else
                {


                    Debug.LogWarning("no more state exist");

                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (0 < index)
                {
                    index--;
                    GameState.setGameState(path[index], GameState.getState());
                    
                    yield return null;

                } else
                {

                    Debug.LogWarning("root state reached");

                }
            }

            if (Input.GetKeyDown(KeyCode.Backspace)) {
                Debug.Log("the solver reached the end");
                allSolved = true;
            }


            yield return null;
        }




    }


}
