using Catan.GameManagement;
using Catan.Players;
using Catan.ResourcePhase;
using Catan.TradePhase;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Catan.UI
{
    public class ResourceSplit : MonoBehaviour
    {
        public int wheat;
        public int wool;
        public int wood;
        public int brick;
        public int ore;

        public TextMeshProUGUI wheatTXT;
        public TextMeshProUGUI woolTXT;
        public TextMeshProUGUI woodTXT;
        public TextMeshProUGUI brickTXT;
        public TextMeshProUGUI oreTXT;

        public TextMeshProUGUI wheatTXTstored;
        public TextMeshProUGUI woolTXTstored;
        public TextMeshProUGUI woodTXTstored;
        public TextMeshProUGUI brickTXTstored;
        public TextMeshProUGUI oreTXTstored;

        public TextMeshProUGUI mainLabel;
        public Button submitButton;

        public Player player;
        public Stack<Player> playerStack;
        public int discardAmount;

        public void AddWool(int toAdd)
        {
            wool = Mathf.Clamp(wool + toAdd, 0, player.resources.Where(r => r.type == ResourcePhase.Resource.ResourceType.Wool).First().amount);
            UpdateValues();
        }

        public void AddWheat(int toAdd)
        {
            wheat = Mathf.Clamp(wheat + toAdd, 0, player.resources.Where(r => r.type == ResourcePhase.Resource.ResourceType.Grain).First().amount);
            UpdateValues();
        }

        public void AddWood(int toAdd)
        {
            wood = Mathf.Clamp(wood + toAdd, 0, player.resources.Where(r => r.type == ResourcePhase.Resource.ResourceType.Wood).First().amount);
            UpdateValues();
        }

        public void AddBrick(int toAdd)
        {
            brick = Mathf.Clamp(brick + toAdd, 0, player.resources.Where(r => r.type == ResourcePhase.Resource.ResourceType.Brick).First().amount);
            UpdateValues();
        }

        public void AddOre(int toAdd)
        {
            ore = Mathf.Clamp(ore + toAdd, 0, player.resources.Where(r => r.type == ResourcePhase.Resource.ResourceType.Ore).First().amount);
            UpdateValues();
        }

        public void UpdateValues()
        {
            wheatTXT.text = "" + wheat;
            woolTXT.text = "" + wool;
            woodTXT.text = "" + wood;
            brickTXT.text = "" + brick;
            oreTXT.text = "" + ore;

            wheatTXTstored.text = "" + player.resources.Where(r => r.type == Resource.ResourceType.Grain).First().amount;
            woolTXTstored.text = "" + player.resources.Where(r => r.type == Resource.ResourceType.Wool).First().amount;
            woodTXTstored.text = "" + player.resources.Where(r => r.type == Resource.ResourceType.Wood).First().amount;
            brickTXTstored.text = "" + player.resources.Where(r => r.type == Resource.ResourceType.Brick).First().amount;
            oreTXTstored.text = "" + player.resources.Where(r => r.type == Resource.ResourceType.Ore).First().amount;

            mainLabel.color = player.playerColor;
            mainLabel.text = player.playerName + " - Discard " + discardAmount + " Resources";
            submitButton.interactable = (wheat + wool + wood + brick + ore) == discardAmount;
        }

        public void Submit()
        {
            if (wheat + wool + wood + brick + ore == discardAmount)
            {
                List<Resource> res = new List<Resource>();
                if (wheat > 0) res.Add(new Resource(Resource.ResourceType.Grain, wheat));
                if (wool > 0) res.Add(new Resource(Resource.ResourceType.Wool, wool));
                if (wood > 0) res.Add(new Resource(Resource.ResourceType.Wood, wood));
                if (brick > 0) res.Add(new Resource(Resource.ResourceType.Brick, brick));
                if (ore > 0) res.Add(new Resource(Resource.ResourceType.Ore, ore));

                Trader.Discard(player, res.ToArray());
                gameObject.SetActive(false);
            }

            GameObject.Find("Game Manager").GetComponent<GameManager>().UIManager.SplitResources(playerStack);
        }

        public void InitializePlayer(Stack<Player> plrs)
        {
            player = plrs.Pop();
            playerStack = plrs;

            discardAmount = (int)(player.resourceSum / 2);

            wheat = 0;
            wool = 0;
            wood = 0;
            brick = 0;
            ore = 0;

            UpdateValues();

            if (player.isAI)
            {
                Resource[] discard = player.agent.ChooseDiscard(discardAmount);
                wheat = discard.Where(r => r.type == Resource.ResourceType.Grain).First().amount;
                wool = discard.Where(r => r.type == Resource.ResourceType.Wool).First().amount;
                wood = discard.Where(r => r.type == Resource.ResourceType.Wood).First().amount;
                brick = discard.Where(r => r.type == Resource.ResourceType.Brick).First().amount;
                ore = discard.Where(r => r.type == Resource.ResourceType.Ore).First().amount;
                Submit();
            }
        }
    }
}
