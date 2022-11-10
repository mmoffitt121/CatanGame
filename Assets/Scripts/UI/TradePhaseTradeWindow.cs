using Catan.Players;
using Catan.ResourcePhase;
using Catan.TradePhase;
using Catan.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradePhaseTradeWindow : MonoBehaviour
{
    public TradePhase tradePhase;

    public TextMeshProUGUI playerXLabel;
    public TextMeshProUGUI playerYLabel;
    public Button offerButton;

    public TextMeshProUGUI playerXTotalWheatDisplay;
    public TextMeshProUGUI playerXTotalSheepDisplay;
    public TextMeshProUGUI playerXTotalWoodDisplay;
    public TextMeshProUGUI playerXTotalBrickDisplay;
    public TextMeshProUGUI playerXTotalOreDisplay;

    public TextMeshProUGUI playerYTotalWheatDisplay;
    public TextMeshProUGUI playerYTotalSheepDisplay;
    public TextMeshProUGUI playerYTotalWoodDisplay;
    public TextMeshProUGUI playerYTotalBrickDisplay;
    public TextMeshProUGUI playerYTotalOreDisplay;

    public TextMeshProUGUI playerXWheatDisplay;
    public TextMeshProUGUI playerXSheepDisplay;
    public TextMeshProUGUI playerXWoodDisplay;
    public TextMeshProUGUI playerXBrickDisplay;
    public TextMeshProUGUI playerXOreDisplay;

    public TextMeshProUGUI playerYWheatDisplay;
    public TextMeshProUGUI playerYSheepDisplay;
    public TextMeshProUGUI playerYWoodDisplay;
    public TextMeshProUGUI playerYBrickDisplay;
    public TextMeshProUGUI playerYOreDisplay;

    public Resource[] playerXOffer;
    public Resource[] playerYOffer;

    Player x;
    Player y;

    public Resource.ResourceType[] resourceTypes = new Resource.ResourceType[]
    {
        Resource.ResourceType.Grain, 
        Resource.ResourceType.Wool, 
        Resource.ResourceType.Wood, 
        Resource.ResourceType.Brick, 
        Resource.ResourceType.Ore
    };

    public void Initialize()
    {
        x = tradePhase.gm.currentPlayer;
        playerXLabel.text = x.playerName;
        playerXLabel.color = x.primaryUIColor;

        y = tradePhase.selectedPlayer;
        playerYLabel.text = y.playerName;
        playerYLabel.color = y.primaryUIColor;

        playerXOffer = new Resource[]
        {
            new Resource(Resource.ResourceType.Grain, 0),
            new Resource(Resource.ResourceType.Wool, 0),
            new Resource(Resource.ResourceType.Wood, 0),
            new Resource(Resource.ResourceType.Brick, 0),
            new Resource(Resource.ResourceType.Ore, 0)
        };
        playerYOffer = new Resource[]
        {
            new Resource(Resource.ResourceType.Grain, 0),
            new Resource(Resource.ResourceType.Wool, 0),
            new Resource(Resource.ResourceType.Wood, 0),
            new Resource(Resource.ResourceType.Brick, 0),
            new Resource(Resource.ResourceType.Ore, 0)
        };

        UpdateUI();
    }

    public void UpdateUI()
    {
        playerXTotalWheatDisplay.text = x.resources.Where(r => r.type == Resource.ResourceType.Grain).First().amount.ToString();
        playerXTotalSheepDisplay.text = x.resources.Where(r => r.type == Resource.ResourceType.Wool).First().amount.ToString();
        playerXTotalWoodDisplay.text = x.resources.Where(r => r.type == Resource.ResourceType.Wood).First().amount.ToString();
        playerXTotalBrickDisplay.text = x.resources.Where(r => r.type == Resource.ResourceType.Brick).First().amount.ToString();
        playerXTotalOreDisplay.text = x.resources.Where(r => r.type == Resource.ResourceType.Ore).First().amount.ToString();

        playerYTotalWheatDisplay.text = y.resources.Where(r => r.type == Resource.ResourceType.Grain).First().amount.ToString();
        playerYTotalSheepDisplay.text = y.resources.Where(r => r.type == Resource.ResourceType.Wool).First().amount.ToString();
        playerYTotalWoodDisplay.text = y.resources.Where(r => r.type == Resource.ResourceType.Wood).First().amount.ToString();
        playerYTotalBrickDisplay.text = y.resources.Where(r => r.type == Resource.ResourceType.Brick).First().amount.ToString();
        playerYTotalOreDisplay.text = y.resources.Where(r => r.type == Resource.ResourceType.Ore).First().amount.ToString();

        playerXWheatDisplay.text = playerXOffer[0].amount.ToString();
        playerXSheepDisplay.text = playerXOffer[1].amount.ToString();
        playerXWoodDisplay.text = playerXOffer[2].amount.ToString();
        playerXBrickDisplay.text = playerXOffer[3].amount.ToString();
        playerXOreDisplay.text = playerXOffer[4].amount.ToString();

        playerYWheatDisplay.text = playerYOffer[0].amount.ToString();
        playerYSheepDisplay.text = playerYOffer[1].amount.ToString();
        playerYWoodDisplay.text = playerYOffer[2].amount.ToString();
        playerYBrickDisplay.text = playerYOffer[3].amount.ToString();
        playerYOreDisplay.text = playerYOffer[4].amount.ToString();
    }

    public void SetValue(TradePhaseButtonArgs args)
    {
        if (args.pY)
        {
            playerYOffer[args.resource].amount += args.change;
            if (playerYOffer[args.resource].amount < 0) 
            { 
                playerYOffer[args.resource].amount = 0;
            }
            if (playerYOffer[args.resource].amount > y.resources.Where(r => r.type == resourceTypes[args.resource]).First().amount) 
            { 
                playerYOffer[args.resource].amount = y.resources.Where(r => r.type == resourceTypes[args.resource]).First().amount; 
            }
        }
        else
        {
            playerXOffer[args.resource].amount += args.change;
            if (playerXOffer[args.resource].amount < 0) 
            { 
                playerXOffer[args.resource].amount = 0; 
            }
            if (playerXOffer[args.resource].amount > x.resources.Where(r => r.type == resourceTypes[args.resource]).First().amount)
            {
                playerXOffer[args.resource].amount = x.resources.Where(r => r.type == resourceTypes[args.resource]).First().amount;
            }
        }

        UpdateUI();
    }

    public async void Offer()
    {
        bool successful = tradePhase.Offer(x, y, playerXOffer, playerYOffer);

        if (successful)
        {
            Debug.Log("success");
        }
    }
}
