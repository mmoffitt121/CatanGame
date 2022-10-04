using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Catan.Players;
using Catan.GameManagement;

public class PlayerFiller : MonoBehaviour
{
    public GameObject Panel;

    // Start is called before the first frame update
    void Start()
    {

        var gm = GameObject.Find("Game Manager").GetComponent<GameManager>();

    }


}
