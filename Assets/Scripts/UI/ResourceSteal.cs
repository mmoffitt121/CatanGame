using Catan.GameManagement;
using Catan.Players;
using Catan.ResourcePhase;
using Catan.TradePhase;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Responsible for stealing a resource from a player
/// </summary>
public class ResourceSteal : MonoBehaviour
{
    /// <summary>
    /// Player button prefab
    /// </summary>
    public GameObject playerButtonPrefab;

    /// <summary>
    /// Text representing title text
    /// </summary>
    public TextMeshProUGUI titleText;

    /// <summary>
    /// Player who is doing the stealing
    /// </summary>
    public Player stealer;
    /// <summary>
    /// Possible players to steal from
    /// </summary>
    public Player[] candidates;

    /// <summary>
    /// LIst of locations on UI to put list items
    /// </summary>
    public static readonly Vector3[] locations = { new Vector3(0, 0, 0), new Vector3(0, -80, 0), new Vector3(0, 80, 0) };

    /// <summary>
    /// Clears the items in the GUI
    /// </summary>
    public void ClearItems()
    {
        foreach (Transform child in transform)
        {
            if (child.name != "TopLabel")
            {
                Destroy(child.gameObject);
            }
        }
    }

    /// <summary>
    /// Adds items to GUI
    /// </summary>
    public void AddItems()
    {
        titleText.text = stealer.playerName + " - Steal A Resource";
        titleText.color = stealer.playerColor;

        for (int i = 0; i < candidates.Length; i++)
        {
            GameObject btn = Instantiate(playerButtonPrefab);
            btn.transform.SetParent(transform);
            btn.transform.position = transform.position + locations[i];
            btn.GetComponentInChildren<TextMeshProUGUI>().text = candidates[i].playerName + ": " + candidates[i].resourceSum + " resources";

            int index = i;
            btn.GetComponent<Button>().onClick.AddListener(() => Submit(index));
            btn.GetComponent<Button>().image.color = candidates[i].primaryUIColor;
        }
    }

    /// <summary>
    /// Submits person to steal from
    /// </summary>
    /// <param name="toStealFrom"></param>
    public void Submit(int toStealFrom)
    {
        Player thief = stealer;
        Player victim = candidates[toStealFrom];

        // Function to ensure randomness between every resource, NOT every resource type
        int rand = Random.Range(0, victim.resources.Length);
        int rIndex = rand;

        for (int i = 0; i < victim.resources.Length; i++)
        {
            if (victim.resources[(i + rand) % victim.resources.Length].amount > 0)
            {
                rIndex = (i + rand) % victim.resources.Length;
            }
        }

        Trader.Trade(thief, victim, new Resource[0], new Resource[] { new Resource(victim.resources[rIndex].type, 1) });
        GameObject.Find("Game Manager").GetComponent<GameManager>().UIManager.EndSteal();
    }

    /// <summary>
    /// Initializes UI
    /// </summary>
    /// <param name="initialPlayer"></param>
    /// <param name="players"></param>
    public void InitializePlayers(Player initialPlayer, Player[] players)
    {
        stealer = initialPlayer;
        candidates = players;

        ClearItems();
        AddItems();

        if (initialPlayer.isAI)
        {
            Submit(initialPlayer.agent.ChooseSteal(candidates));
        }
    }
}
