using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    PlayerMainScript player;
    Tower arrowTower;
    BasicWall wall;

    public void BuyTower()
    {
        player.LoseCoins(arrowTower.cost);

    }
}
