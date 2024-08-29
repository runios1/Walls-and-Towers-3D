using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Viapix_HealingItem
{
    public class Viapix_HealingItem : MonoBehaviour
    {
        [SerializeField]
        float rotationSpeedX, rotationSpeedY, rotationSpeedZ;

        [SerializeField]
        float healingAmount;

        GameObject playerObj;

        private void Start()
        {
            playerObj = GameObject.FindGameObjectWithTag("Player");
        }

        void Update()
        {
            transform.Rotate(rotationSpeedX, rotationSpeedY, rotationSpeedZ);
            healingAmount = playerObj.GetComponent<PlayerMainScript>().health*0.2f;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                playerObj.GetComponent<PlayerMainScript>().HealPlayer(healingAmount);

                Destroy(gameObject);

                Debug.Log("Player HP: " + playerObj.GetComponent<PlayerMainScript>().health);
            }
        }
    }
}

