using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinsChangeController : MonoBehaviour
{
    private enum CoinsChangeState{
        INCREMENT,
        DECREMENT,
        STOP
    }
    private float lastUpdateTimeStamp;
    public float durationToStopDisplay;
    private int changeValue;
    private TextMeshProUGUI textMesh;
    private CoinsChangeState changeState;
    // Start is called before the first frame update
    void Start()
    {
        lastUpdateTimeStamp = -1;
        changeValue = 0;
        textMesh = gameObject.GetComponentsInChildren<TextMeshProUGUI>()[1];
        Debug.Assert(textMesh != null,"textMesh is null");
        Debug.Log("textMesh: " + textMesh.name);
        changeState = CoinsChangeState.STOP;
    }

    // Update is called once per frame
    void Update()
    {
        if(lastUpdateTimeStamp != -1){
            if(!shouldAppear()){
                textMesh.text = "";
                changeValue = 0;
                changeState = CoinsChangeState.STOP;
            }
        }
    }
    private bool shouldAppear(){
        return Time.time - lastUpdateTimeStamp < durationToStopDisplay;
    }

    public void ChangeCoins(int value,bool direction){
        switch(changeState){
            case CoinsChangeState.INCREMENT:
                if(direction){
                    changeValue += value;
                }else{
                    changeValue -= value;
                }
                textMesh.color = Color.green;
                textMesh.text = "+ " + changeValue;
                textMesh.ForceMeshUpdate();
                break;
            case CoinsChangeState.DECREMENT:
                if(direction){
                    changeValue -= value;
                } else{
                    changeValue += value;
                }
                textMesh.color = Color.red;
                textMesh.text = "- " + changeValue;
                textMesh.ForceMeshUpdate();
                break;
            case CoinsChangeState.STOP:
                changeValue = value;
                changeState = direction? CoinsChangeState.INCREMENT : CoinsChangeState.DECREMENT;
                lastUpdateTimeStamp = Time.time;
                textMesh.color = changeState == CoinsChangeState.INCREMENT? Color.green : Color.red;
                textMesh.text = changeState == CoinsChangeState.INCREMENT? "+ " + changeValue : "- " + changeValue;
                textMesh.ForceMeshUpdate();
                break;
        }
    }

}
