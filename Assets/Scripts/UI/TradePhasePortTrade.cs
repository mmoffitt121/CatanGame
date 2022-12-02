/// AUTHOR: Matthew Moffitt
/// FILENAME: TradePhasePortTrade.cs
/// SPECIFICATION: Responsible for trade phase port trade
/// FOR: CS 3368 Introduction to Artificial Intelligence Section 001

using Catan.GameBoard;
using Catan.Players;
using Catan.UI;
using Catan.ResourcePhase;
using Catan.TradePhase;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the UI for players trading with ports
/// </summary>
public class TradePhasePortTrade : MonoBehaviour
{
    /// <summary>
    /// The initiating player
    /// </summary>
    Player player;

    // UI members
    public Toggle[] toggles;
    public TextMeshProUGUI[] costs;
    public TextMeshProUGUI[] amounts;
    public Button[] purchaseButtons;
    public TextMeshProUGUI portsLabel;

    /// <summary>
    /// Currently selected resource to trade
    /// </summary>
    public int selectedResource;

    /// <summary>
    /// Trade phase
    /// </summary>
    public TradePhase tradePhase;
    /// <summary>
    /// Board
    /// </summary>
    public Board board;

    /// <summary>
    /// Initilize window
    /// </summary>
    /// <param name="player"></param>
    public void Initialize(Player player)
    {
        this.player = player;
        portsLabel.color = player.primaryUIColor;
        toggles[0].isOn = true;
        Check(0);
    }

    /// <summary>
    /// What fires when a player checks a check box
    /// </summary>
    /// <param name="checkBox"></param>
    public void Check(int checkBox)
    {
        selectedResource = checkBox;
        UpdateUI(checkBox);
    }

    /// <summary>
    /// Updates the UI
    /// </summary>
    /// <param name="selected"></param>
    public void UpdateUI(int selected)
    {
        Resource.ResourceType resource = Resource.ResourceType.None;
        int cost = 4;

        switch (selected)
        {
            case 0:
                resource = Resource.ResourceType.Grain;
                break;
            case 1:
                resource = Resource.ResourceType.Wool;
                break;
            case 2:
                resource = Resource.ResourceType.Wood;
                break;
            case 3:
                resource = Resource.ResourceType.Brick;
                break;
            case 4:
                resource = Resource.ResourceType.Ore;
                break;
        }

        if (board.vertices.HasPort(player, resource))
        {
            cost = 2;
        }
        else if (board.vertices.HasPort(player, Resource.ResourceType.Any))
        {
            cost = 3;
        }

        foreach (TextMeshProUGUI c in costs)
        {
            c.text = "Cost: " + cost;
        }
        
        foreach (Button b in purchaseButtons)
        {
            b.interactable = player.resources.Where(r => r.type == resource).First().amount >= cost;
        }

        amounts[0].text = player.resources.Where(r => r.type == Resource.ResourceType.Grain).First().amount.ToString();
        amounts[1].text = player.resources.Where(r => r.type == Resource.ResourceType.Wool).First().amount.ToString();
        amounts[2].text = player.resources.Where(r => r.type == Resource.ResourceType.Wood).First().amount.ToString();
        amounts[3].text = player.resources.Where(r => r.type == Resource.ResourceType.Brick).First().amount.ToString();
        amounts[4].text = player.resources.Where(r => r.type == Resource.ResourceType.Ore).First().amount.ToString();
    }

    /// <summary>
    /// Handles trading resources
    /// </summary>
    /// <param name="selected"></param>
    public void Trade(int selected)
    {
        Resource.ResourceType toSell = Resource.ResourceType.None;
        Resource.ResourceType toBuy = Resource.ResourceType.None;
        int cost = 4;

        switch (selected)
        {
            case 0:
                toBuy = Resource.ResourceType.Grain;
                break;
            case 1:
                toBuy = Resource.ResourceType.Wool;
                break;
            case 2:
                toBuy = Resource.ResourceType.Wood;
                break;
            case 3:
                toBuy = Resource.ResourceType.Brick;
                break;
            case 4:
                toBuy = Resource.ResourceType.Ore;
                break;
        }

        switch (selectedResource)
        {
            case 0:
                toSell = Resource.ResourceType.Grain;
                break;
            case 1:
                toSell = Resource.ResourceType.Wool;
                break;
            case 2:
                toSell = Resource.ResourceType.Wood;
                break;
            case 3:
                toSell = Resource.ResourceType.Brick;
                break;
            case 4:
                toSell = Resource.ResourceType.Ore;
                break;
        }

        if (board.vertices.HasPort(player, toSell))
        {
            cost = 2;
        }
        else if (board.vertices.HasPort(player, Resource.ResourceType.Any))
        {
            cost = 3;
        }

        if (player.resources.Where(r => r.type == toSell).First().amount >= cost)
        {
            Trader.Trade(player, new Resource[] { new Resource(toSell, cost) }, new Resource[] { new Resource(toBuy, 1) });
        }

        UpdateUI(selectedResource);
        tradePhase.gm.UIManager.UpdateUI();
    }
}
