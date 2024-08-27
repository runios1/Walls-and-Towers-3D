using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class ScoreGrab : MonoBehaviour
{
    private ScoringSystem scoringSystem;
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        scoringSystem = FindObjectOfType<ScoringSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Score: " + scoringSystem.getTotalScore().ToString("F2"); 
    }
}
