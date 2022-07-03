using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // เอามาด้วย

public class Textscritp : MonoBehaviour
{
    public TMP_Text playedPaper_Text; // text to show how many paper player has played
    public TMP_Text playedNakApproved_Text; // text show how many nak's paper player has approved
    public TMP_Text falseDecision_Text; // Text show how many times player has made wrong decision on paper
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playedPaper_Text.text = (maxPaper-currentAmountPaper+1).ToString();
        playedNakApproved_Text.text = playedNakApproved.ToString() ;
        falseDecision_Text.text = 0+"/"+3; 
    }
}
