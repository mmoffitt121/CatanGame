using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Catan.Players;
using Catan.ResourcePhase;
using Catan.TradePhase;
using TMPro;

namespace Catan.UI
{
    public class TradePhaseTradeOffer : MonoBehaviour
    {
        public TradePhase tradePhase;

        public TextMeshProUGUI offerRecipient;

        public TextMeshProUGUI playerXName;
        public TextMeshProUGUI playerYName;

        public TextMeshProUGUI playerXWheat;
        public TextMeshProUGUI playerXSheep;
        public TextMeshProUGUI playerXWood;
        public TextMeshProUGUI playerXBrick;
        public TextMeshProUGUI playerXOre;

        public TextMeshProUGUI playerYWheat;
        public TextMeshProUGUI playerYSheep;
        public TextMeshProUGUI playerYWood;
        public TextMeshProUGUI playerYBrick;
        public TextMeshProUGUI playerYOre;

        private Player playerX;
        private Player playerY;

        private Resource[] pXOffer;
        private Resource[] pYOffer;


        public void Initialize(Player p1, Player p2, Resource[] p1Offer, Resource[] p2Offer)
        {
            offerRecipient.text = "Offer - " + p2.playerName;
            offerRecipient.color = p2.playerColor;

            playerXName.text = p1.playerName;
            playerXName.color = p1.primaryUIColor;

            playerYName.text = p2.playerName;
            playerYName.color = p2.primaryUIColor;

            ResetUI();

            foreach (Resource r in p1Offer)
            {
                switch (r.type)
                {
                    case Resource.ResourceType.Grain:
                        playerXWheat.text = r.amount.ToString();
                        break;
                    case Resource.ResourceType.Wool:
                        playerXSheep.text = r.amount.ToString();
                        break;
                    case Resource.ResourceType.Wood:
                        playerXWood.text = r.amount.ToString();
                        break;
                    case Resource.ResourceType.Brick:
                        playerXBrick.text = r.amount.ToString();
                        break;
                    case Resource.ResourceType.Ore:
                        playerXOre.text = r.amount.ToString();
                        break;
                    default:
                        break;
                }
            }

            foreach (Resource r in p2Offer)
            {
                switch (r.type)
                {
                    case Resource.ResourceType.Grain:
                        playerYWheat.text = r.amount.ToString();
                        break;
                    case Resource.ResourceType.Wool:
                        playerYSheep.text = r.amount.ToString();
                        break;
                    case Resource.ResourceType.Wood:
                        playerYWood.text = r.amount.ToString();
                        break;
                    case Resource.ResourceType.Brick:
                        playerYBrick.text = r.amount.ToString();
                        break;
                    case Resource.ResourceType.Ore:
                        playerYOre.text = r.amount.ToString();
                        break;
                    default:
                        break;
                }
            }

            playerX = p1;
            playerY = p2;
            pXOffer = p1Offer;
            pYOffer = p2Offer;
        }

        public void SendResponse(bool response)
        {
            if (response)
            {
                Trader.Trade(playerX, playerY, pXOffer, pYOffer);
            }
            
            if (playerX.isAI)
            {
                tradePhase.ShowTradeButton();
                playerX.agent.OfferResultRecieved(response);
            }
            else
            {
                tradePhase.ShowTradeResultWindow(response);
            }
        }

        public void ResetUI()
        {
            playerXWheat.text = "0";
            playerXSheep.text = "0";
            playerXWood.text = "0";
            playerXBrick.text = "0";
            playerXOre.text = "0";

            playerYWheat.text = "0";
            playerYSheep.text = "0";
            playerYWood.text = "0";
            playerYBrick.text = "0";
            playerYOre.text = "0";
        }
    }
}
