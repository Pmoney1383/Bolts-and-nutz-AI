using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Heuristic 
{
    public float CalculateHeuristic(string state, string previousState)
    {
        string[] bolts = state.Split('|');
        string[] currentBolts = state.Split('|');
        string[] previousBolts = previousState.Split('|');
        float score = 0f;

        foreach (var bolt in bolts)
        {
            if (bolt.All(c => c == 'X')) continue;

            HashSet<string> distinctColors = new HashSet<string>();
            int nutCount = 0;
            int emptyCount = 0;

            for (int i = 0; i < bolt.Length; i += 2)
            {
                string nut = bolt.Substring(i, 2);
                if (nut == "XX")
                {
                    emptyCount++;
                    continue;
                }
                distinctColors.Add(nut);
                nutCount++;
            }

            bool isFull = emptyCount == 0;
            bool isSorted = distinctColors.Count == 1;
            bool isSingleNut = nutCount == 1;

            // Strong reward for full & sorted
            if (isFull && isSorted)
                score -= 4.5f;

            // Mild reward for nearly sorted bolts
            else if (isSorted && nutCount >= 3)
                score -= 1f;

            // Penalize bolts with many mixed colors
            if (distinctColors.Count > 2)
                score += 1f;

            // NEW: Slight penalty for touching single-nut bolts
            if (isSingleNut)
                score += 0.8f;

            // NEW: Discourage breaking almost-sorted bolts
            if (isSorted && nutCount > 2 && emptyCount > 0)
                score -= 0.5f;

           


        }

        //  Penalize if the move was "sorted bolt -> empty bolt"
        int from = -1, to = -1;

        for (int i = 0; i < currentBolts.Length; i++)
        {

            if (currentBolts[i] != previousBolts[i])
            {
                
               
                if (previousBolts[i].All(c => c == 'X') && !currentBolts[i].All(c => c == 'X'))
                {
                    to = i;
                    
                }
                else if (!previousBolts[i].All(c => c == 'X') && currentBolts[i].All(c => c == 'X'))
                {
                    from = i;
                    
                }

               
            }

        }

        if (from != -1 && to != -1)
        {
            bool fromWasSorted = IsSortedBolt(previousBolts[from]);
            bool toWasEmpty = previousBolts[to].All(c => c == 'X');
            
            if (fromWasSorted && toWasEmpty)
            {
               // Debug.Log("worked!");
                score += 100f; //  Penalty for wasting a move
              //  Debug.Log("worked!" + score);
            }
        }
        return Mathf.Max(score, 0f);
    }

    private bool IsSortedBolt(string bolt)
    {
        if (bolt.All(c => c == 'X')) return false;

        string firstNut = bolt.Substring(0,2);
        for (int i = 0; i < bolt.Length; i += 2)
        {
            string nut = bolt.Substring(i, 2);
            if (nut == "XX") continue;
            if (nut != firstNut) return false;
        }
        return true;
    }

}
