using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    public GameObject magicCirclePrefab;
    private Enemy[] enemies;
    // Start is called before the first frame update
    void Start()
    {
        enemies = gameObject.GetComponentsInChildren<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if(magicCirclePrefab != null && magicCirclePrefab.GetComponent<ParticleSystem>().isStopped){
            Destroy(magicCirclePrefab);
        }
        
    }
}
