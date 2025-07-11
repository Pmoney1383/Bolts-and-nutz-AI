using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameNode
{

    public List<List<GameObject>> State;
    public List<List<GameObject>> previousState;
    public string state;
    public string previousstate;
    public int MoveFrom;
    public int MoveTo;



    public GameNode(List<List<GameObject>> state, List<List<GameObject>> previousState, int moveFrom, int moveTo)
    {

        State = state;
        this.previousState = previousState;
        MoveFrom = moveFrom;
        MoveTo = moveTo;
    }
    public GameNode(List<List<GameObject>> state, List<List<GameObject>> previousState)
    {

        State = state;
        this.previousState = previousState;
        
    }
    public GameNode(string state, string previousState, int moveFrom, int moveTo)
    {

        this.state = state;
        this.previousstate = previousState;
        MoveFrom = moveFrom;
        MoveTo = moveTo;
    }
    public GameNode(string state, string previousState)
    {

        this.state = state;
        this.previousstate = previousState;

    }

    



}
   



