using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameNode
{

    public List<List<GameObject>> State;
    public List<List<GameObject>> previousState;
    public string state;
    public string previousstate;
    public float gScore;
    public float hScore;
    public float fScore => gScore + hScore;



    public GameNode(List<List<GameObject>> state, List<List<GameObject>> previousState, float gScore, float hScore)
    {

        State = state;
        this.previousState = previousState;
        this.gScore = gScore;
        this.hScore = hScore;
    }
    public GameNode(List<List<GameObject>> state, List<List<GameObject>> previousState)
    {

        State = state;
        this.previousState = previousState;
        
    }
    public GameNode(string state, string previousState, float gScore, float hScore)
    {

        this.state = state;
        this.previousstate = previousState;
        this.gScore = gScore;
        this.hScore = hScore;
    }
    public GameNode(string state, string previousState)
    {

        this.state = state;
        this.previousstate = previousState;

    }

    



}
   



