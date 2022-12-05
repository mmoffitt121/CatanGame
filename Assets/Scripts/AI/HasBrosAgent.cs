/// AUTHOR: Wuraola Alli, Jett Graham, Evan Griffin, Matthew Moffitt, Alex Rizzo, Brandon Villalobos
/// FILENAME: Agent.cs
/// SPECIFICATION: File containing agent activities and information
/// FOR: CS 3368 Introduction to Artificial Intelligence Section 001

using Catan.Players;
using Catan.ResourcePhase;
using Catan.TradePhase;
using Catan.BuildPhase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Catan.GameBoard;
using System.Text;

namespace Catan.AI
{
    /// <summary>
    /// AI agent class
    /// </summary>
    public class HasBrosAgent : Agent
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="plyr"></param>
        public HasBrosAgent(Player plyr) : base(plyr)
        {
            agentName = "Has Bros Agent";
        }

        /// <summary>
        /// Override void that controls the placement of the AI player's starting piece
        /// </summary>
        /// <param name="first"></param>
        public override void PlaceStartingPiece(bool first = false)
        {
            // Grab vertices from board
            TileVertex[][] verts = api.board.vertices;
            Road[][] roads = api.board.roads;

            // First, calculate expected value for every resource for every vertex.

            // Triple disjointed array of floats, and triple disjointed array of Resource Types.
            // Outermost array represents vertex row, middle array represents vertex column, and innermost array represents the expected value of each resource for a given vertex.
            float[][][] EVs = new float[verts.Length][][];
            Resource.ResourceType[][][] resources = new Resource.ResourceType[verts.Length][][];
            // Ports
            Resource.ResourceType[][] ports = new Resource.ResourceType[verts.Length][];

            // Lists representing expected values already owned by the player
            List<float> ownedEVs = new List<float>();
            List<Resource.ResourceType> ownedResources = new List<Resource.ResourceType>();
            // Ports
            Resource.ResourceType[] ownedPorts = api.Vertices.GetPlayerPorts(player);

            for (int i = 0; i < verts.Length; i++)
            {
                EVs[i] = new float[verts[i].Length][];
                resources[i] = new Resource.ResourceType[verts[i].Length][];
                ports[i] = new Resource.ResourceType[verts[i].Length];

                for (int j = 0; j < verts[i].Length; j++)
                {
                    // Check base case where building is impossible due to adjacent developed vertex.
                    (int, int) above = verts.VertexAboveVertex(i, j);
                    (int, int) below = verts.VertexBelowVertex(i, j);
                    (int, int) left = verts.VertexLeftOfVertex(i, j);
                    (int, int) right = verts.VertexRightOfVertex(i, j);

                    // Case in which the vertex is not valid to build on
                    if (above.Valid() && verts[above.Item1][above.Item2].development > 0 ||
                        below.Valid() && verts[below.Item1][below.Item2].development > 0 ||
                        left.Valid() && verts[left.Item1][left.Item2].development > 0 ||
                        right.Valid() && verts[right.Item1][right.Item2].development > 0)
                    {
                        EVs[i][j] = new float[1] { -10000 };
                        resources[i][j] = new Resource.ResourceType[] { Resource.ResourceType.None };
                        continue;
                    }

                    // Case in which the vertex is already owned
                    if (verts[i][j].development > 0 && verts[i][j].playerIndex == player.playerIndex)
                    {
                        EVs[i][j] = new float[1] { -10000 };
                        resources[i][j] = new Resource.ResourceType[] { Resource.ResourceType.None };

                        (Resource.ResourceType, float)[] ownedEV = api.board.CalculateVertexExpectedValues(i, j);
                        foreach ((Resource.ResourceType, float) ev in ownedEV)
                        {
                            ownedEVs.Add(ev.Item2);
                            ownedResources.Add(ev.Item1);
                        }

                        continue;
                    }

                    if (verts[i][j].port != null)
                    {
                        ports[i][j] = verts[i][j].port.type;
                    }

                    // Calculate expected value of resource
                    (Resource.ResourceType, float)[] expectedValues = api.board.CalculateVertexExpectedValues(i, j);
                    EVs[i][j] = new float[expectedValues.Length];
                    resources[i][j] = new Resource.ResourceType[expectedValues.Length];

                    for (int k = 0; k < expectedValues.Length; k++)
                    {
                        EVs[i][j][k] = expectedValues[k].Item2;
                        resources[i][j][k] = expectedValues[k].Item1;
                    }
                }
            }

            int baseCost = 4;
            int grainCost = 4;
            int sheepCost = 4;
            int woodCost = 4;
            int brickCost = 4;
            int oreCost = 4;

            // Calculate initial port costs
            for (int i = 0; i < ownedPorts.Length; i++)
            {
                switch (ownedPorts[i])
                {
                    case Resource.ResourceType.Any:
                        baseCost = 3;
                        break;
                    case Resource.ResourceType.Grain:
                        grainCost = 2;
                        break;
                    case Resource.ResourceType.Wool:
                        sheepCost = 2;
                        break;
                    case Resource.ResourceType.Wood:
                        woodCost = 2;
                        break;
                    case Resource.ResourceType.Brick:
                        brickCost = 2;
                        break;
                    case Resource.ResourceType.Ore:
                        oreCost = 2;
                        break;
                }
            }

            grainCost = Math.Min(baseCost, grainCost);
            sheepCost = Math.Min(baseCost, sheepCost);
            woodCost = Math.Min(baseCost, woodCost);
            brickCost = Math.Min(baseCost, brickCost);
            oreCost = Math.Min(baseCost, oreCost);

            // Calculate space with highest expected value
            List<(int, int)> excluded = new List<(int, int)>();
            bool buildResult = false;
            float highest;
            int highestI = 0;
            int highestJ = 0;

            float settlementEV;
            float cityEV;
            // Keep trying to build until we succeed, in order of value for each vertex
            while (buildResult == false)
            {
                highest = -10000f;
                highestI = 0;
                highestJ = 0;
                for (int i = 0; i < EVs.Length; i++)
                {
                    for (int j = 0; j < EVs[i].Length; j++)
                    {
                        // If we already tried this one and it failed, continue;
                        if (excluded.Contains((i, j)))
                        {
                            continue;
                        }

                        // Calculate port costs for this vertex
                        int baCost = baseCost;
                        int gCost = grainCost;
                        int sCost = sheepCost;
                        int wCost = woodCost;
                        int bCost = brickCost;
                        int oCost = oreCost;

                        if (api.board.vertices[i][j].port != null)
                        {
                            switch (api.board.vertices[i][j].port.type)
                            {
                                case Resource.ResourceType.Any:
                                    baCost = 3;
                                    break;
                                case Resource.ResourceType.Grain:
                                    gCost = 2;
                                    break;
                                case Resource.ResourceType.Wool:
                                    sCost = 2;
                                    break;
                                case Resource.ResourceType.Wood:
                                    wCost = 2;
                                    break;
                                case Resource.ResourceType.Brick:
                                    bCost = 2;
                                    break;
                                case Resource.ResourceType.Ore:
                                    oCost = 2;
                                    break;
                            }
                        }

                        gCost = Math.Min(baCost, gCost);
                        sCost = Math.Min(baCost, sCost);
                        wCost = Math.Min(baCost, wCost);
                        bCost = Math.Min(baCost, bCost);
                        oCost = Math.Min(baCost, oCost);

                        float gEV = 0f;
                        float sEV = 0f;
                        float wEV = 0f;
                        float bEV = 0f;
                        float oEV = 0f;

                        for (int k = 0; k < ownedResources.Count; k++)
                        {
                            switch (ownedResources[k])
                            {
                                case Resource.ResourceType.Grain:
                                    gEV += ownedEVs[k];
                                    break;
                                case Resource.ResourceType.Wool:
                                    sEV += ownedEVs[k];
                                    break;
                                case Resource.ResourceType.Wood:
                                    wEV += ownedEVs[k];
                                    break;
                                case Resource.ResourceType.Brick:
                                    bEV += ownedEVs[k];
                                    break;
                                case Resource.ResourceType.Ore:
                                    oEV += ownedEVs[k];
                                    break;
                            }
                        }

                        for (int k = 0; k < resources[i][j].Length; k++)
                        {
                            switch (resources[i][j][k])
                            {
                                case Resource.ResourceType.Grain:
                                    gEV += EVs[i][j][k];
                                    break;
                                case Resource.ResourceType.Wool:
                                    sEV += EVs[i][j][k];
                                    break;
                                case Resource.ResourceType.Wood:
                                    wEV += EVs[i][j][k];
                                    break;
                                case Resource.ResourceType.Brick:
                                    bEV += EVs[i][j][k];
                                    break;
                                case Resource.ResourceType.Ore:
                                    oEV += EVs[i][j][k];
                                    break;
                            }
                        }
                        float WEIGHT_MODIFIER = 2f;
                        float anyResourceForSettlementEV = (gEV / gCost / WEIGHT_MODIFIER) + (sEV / sCost / WEIGHT_MODIFIER) + (wEV / wCost / WEIGHT_MODIFIER) + (bEV / bCost / WEIGHT_MODIFIER) + (oEV / oCost);
                        float anyResourceForCityEV = (gEV / gCost / WEIGHT_MODIFIER / 2) + (sEV / sCost) + (wEV / wCost) + (bEV / bCost) + (oEV / oCost / WEIGHT_MODIFIER / 3);

                        // Expected value of building a settlement on this spot
                        float seEV = (gEV + sEV + wEV + bEV + anyResourceForSettlementEV) / 4;
                        // Expected value of building a city on this spot
                        float ciEV = (oEV * 3 + gEV * 2 + anyResourceForCityEV) / 5;

                        if (Math.Max(seEV, ciEV) > highest)
                        {
                            highest = Math.Max(seEV, ciEV);
                            settlementEV = seEV;
                            cityEV = ciEV;
                            highestI = i;
                            highestJ = j;
                        }
                    }
                }

                buildResult = api.BuildSettlement(player, highestI, highestJ, true);

                // If the build fails for whatever reason, add to list of excluded values and continue.
                if (!buildResult)
                {
                    excluded.Add((highestI, highestJ));
                }
            }

            // Builds road
            (int, int) road;
            road = api.board.vertices.RoadAboveVertex(api.board.roads, highestI, highestJ);
            if (api.BuildRoad(player, road.Item1, road.Item2, true)) return;

            road = api.board.vertices.RoadBelowVertex(api.board.roads, highestI, highestJ);
            if (api.BuildRoad(player, road.Item1, road.Item2, true)) return;

            road = api.board.vertices.RoadLeftOfVertex(api.board.roads, highestI, highestJ);
            if (api.BuildRoad(player, road.Item1, road.Item2, true)) return;

            road = api.board.vertices.RoadRightOfVertex(api.board.roads, highestI, highestJ);
            if (api.BuildRoad(player, road.Item1, road.Item2, true)) return;

            if (first)
            {
                api.board.DistributeResourcesFromVertex((highestI, highestJ));
            }
        }

        /// <summary>
        /// Calculates where to place the robber, maximizing the expected value damage done by the placement. If tiles are evalutated to have the same EV, then the result will be chosen randomly between these tiles.
        /// </summary>
        public override void ChooseRobberLocation()
        {
            // Grab tiles from board
            Tile[][] tiles = api.board.tiles;

            // First, calculate expected value for every resource for every vertex.

            // Disjointed array of expected values for each vertex
            float[][] EVs = new float[tiles.Length][];

            for (int i = 0; i < tiles.Length; i++)
            {
                EVs[i] = new float[tiles[i].Length];

                for (int j = 0; j < tiles[i].Length; j++)
                {
                    (int, int)[] surrounding = tiles.GetSurroundingVertices(api.board.vertices, i, j);

                    float ev = 0;

                    if (tiles[i][j].robber)
                    {
                        EVs[i][j] = -1000;
                        continue;
                    }

                    for (int k = 0; k < surrounding.Length; k++)
                    {
                        float vertexDev = surrounding[k].Valid() ? (int)api.board.vertices[surrounding[k].Item1][surrounding[k].Item2].development : 0;

                        float pModifier = 1;
                        if (api.board.vertices[surrounding[k].Item1][surrounding[k].Item2].playerIndex == player.playerIndex)
                        {
                            pModifier = -2;
                        }

                        ev += pModifier * vertexDev;
                    }

                    EVs[i][j] = ev;
                }
            }

            float evMax = 0;
            for (int i = 0; i < EVs.Length; i++)
            {
                for (int j = 0; j < EVs[i].Length; j++)
                {
                    if (EVs[i][j] > evMax)
                    {
                        evMax = EVs[i][j];
                    }
                }
            }

            List<(int, int)> possibleSpots = new List<(int, int)>();
            for (int i = 0; i < EVs.Length; i++)
            {
                for (int j = 0; j < EVs[i].Length; j++)
                {
                    if (EVs[i][j] == evMax)
                    {
                        possibleSpots.Add((i, j));
                    }
                }
            }

            int randomMaxEV = UnityEngine.Random.Range(0, possibleSpots.Count);
            api.MoveRobber(possibleSpots[randomMaxEV].Item1, possibleSpots[randomMaxEV].Item2);
        }

        /// <summary>
        /// Is called when it is time for the player to discard after having more than 7 cards when a 7 is rolled.
        /// </summary>
        /// <param name="discardAmount"></param>
        /// <returns></returns>
        public override Resource[] ChooseDiscard(int discardAmount)
        {
            Resource[] discard = new Resource[] { new Resource(Resource.ResourceType.Wool, 0)
            ,new Resource(Resource.ResourceType.Wood, 0)
            ,new Resource(Resource.ResourceType.Ore, 0)
            ,new Resource(Resource.ResourceType.Brick, 0)
            ,new Resource(Resource.ResourceType.Grain, 0)};

            int grain = player.resources.Where(r => r.type == Resource.ResourceType.Grain).First().amount;
            int wool = player.resources.Where(r => r.type == Resource.ResourceType.Wool).First().amount;
            int wood = player.resources.Where(r => r.type == Resource.ResourceType.Wood).First().amount;
            int brick = player.resources.Where(r => r.type == Resource.ResourceType.Brick).First().amount;
            int ore = player.resources.Where(r => r.type == Resource.ResourceType.Ore).First().amount;

            int count = 0;
            while (count != discardAmount)
            {
                float progressTowardsSettlement = (
                    grain > 0 ? 1 : 0 +
                    wool > 0 ? 1 : 0 +
                    wood > 0 ? 1 : 0 +
                    brick > 0 ? 1 : 0
                    ) / 4f;
                float progressTowardsCity = (
                    ore < 3 ? ore : 3 +
                    grain < 2 ? grain : 2
                    ) / 5f;

                if (progressTowardsSettlement > progressTowardsCity && progressTowardsCity != 0)
                {
                    if (grain > ore && grain > 0)
                    {
                        grain--;
                        discard[4].amount++;
                        count++;
                        continue;
                    }
                    else if (ore > 0)
                    {
                        ore--;
                        discard[2].amount++;
                        count++;
                        continue;
                    }
                }
                else if (progressTowardsSettlement < progressTowardsCity && progressTowardsSettlement != 0)
                {
                    if (grain > wool && grain > wood && grain > brick && grain > 0)
                    {
                        grain--;
                        discard[4].amount++;
                        count++;
                        continue;
                    }
                    else if (wool > grain && wool > wood && wool > brick && wool > 0)
                    {
                        wool--;
                        discard[0].amount++;
                        count++;
                        continue;
                    }
                    else if (wood > grain && wood > wool && wood > brick && wood > 0)
                    {
                        wood--;
                        discard[1].amount++;
                        count++;
                        continue;
                    }
                    else if (brick > 0)
                    {
                        brick--;
                        discard[3].amount++;
                        count++;
                        continue;
                    }
                }
                int rIndex = UnityEngine.Random.Range(0, discard.Length);
                int rMax = rIndex + discard.Length;
                for (int i = 0; i < rMax; i++)
                {
                    // If we're discarding less of the randomly chosen resource than the player has, add one to the total. Else, continue searching.
                    if (discard[i % discard.Length].amount < player.resources.Where(r => r.type == discard[i % discard.Length].type).First().amount)
                    {
                        discard[i % discard.Length].amount += 1;
                        count++;
                        break;
                    }
                }
            }

            return discard;
        }

        /// <summary>
        /// Chooses a player to steal from upon rolling a 7.
        /// </summary>
        /// <param name="stealFrom"></param>
        /// <returns> Index of player to steal random resource from </returns>
        public override int ChooseSteal(Player[] stealFrom)
        {
            if (stealFrom == null || stealFrom.Length <= 1) { return 0; }

            int maxIndex = 0;
            int maxCount = 0;

            for (int i = 0; i < stealFrom.Length; i++)
            {
                if (stealFrom[i].resourceSum > maxCount)
                {
                    maxIndex = i;
                    maxCount = stealFrom[i].resourceSum;
                }
            }

            return maxIndex;
        }

        private int trades;
        public override void StartTrading()
        {
            Player p = api.Players[UnityEngine.Random.Range(0, api.Players.Length)];
            if (p.resourceSum > 0 && p.playerIndex != player.playerIndex)
            {
                Resource[] senderOffer = new Resource[] { player.RandomResource() };
                Resource[] recieverOffer = new Resource[] { p.RandomResource() };
                Trader.Request(player, p, senderOffer, recieverOffer);
            }
            else
            {
                OfferResultRecieved(false);
            }
        }

        public override void OfferResultRecieved(bool accepted)
        {
            trades++;
            if (trades >= maxTrades)
            {
                trades = 0;
                base.StartTrading();
            }
            else
            {
                StartTrading();
            }
        }

        public override void StartBuilding()
        {
            while (true)
            {
                bool canBuildSettlement = player.HasResource(Resource.ResourceType.Brick)
                    && player.HasResource(Resource.ResourceType.Wood)
                    && player.HasResource(Resource.ResourceType.Grain)
                    && player.HasResource(Resource.ResourceType.Wool);

                bool canBuildCity = player.HasResource(Resource.ResourceType.Grain, 2)
                    && player.HasResource(Resource.ResourceType.Ore, 3);

                bool canBuildRoad = player.HasResource(Resource.ResourceType.Brick)
                    && player.HasResource(Resource.ResourceType.Wood);

                (int, int)[] possibleSettlements = api.board.GetPossibleSettlements(player);
                (int, int)[] possibleCities = api.board.GetPossibleCities(player);
                (int, int)[] possibleRoads = api.board.GetPossibleRoads(player);

                if (canBuildSettlement && possibleSettlements.Length > 0)
                {
                    int i = UnityEngine.Random.Range(0, possibleSettlements.Length);
                    api.BuildSettlement(player, possibleSettlements[i].Item1, possibleSettlements[i].Item2);
                }
                else if (canBuildCity && possibleCities.Length > 0)
                {
                    int i = UnityEngine.Random.Range(0, possibleCities.Length);
                    api.UpgradeSettlement(player, possibleCities[i].Item1, possibleCities[i].Item2);
                }
                else if (canBuildRoad && possibleRoads.Length > 0)
                {
                    int i = UnityEngine.Random.Range(0, possibleRoads.Length);
                    api.BuildRoad(player, possibleRoads[i].Item1, possibleRoads[i].Item2);
                }
                else
                {
                    break;
                }
            }

            base.StartBuilding();
        }


    }
}
// public class BuildPhase :build
//         {
//             Player player;
//             CurrentPlayer currentPlayer;
//             LongestRoad longestroad;

//             {
//                 public List <GameObject> deck = new List<GameObject>();
//                 private List<GameObject> cards = new List<GameObject>();
//                 private List<GameObject> resource = new List<GameObject>();

//                  private int cardsDealt=0;
//                  private bool showReset= false;
//             }
//             {

//                 double[] dblPlayerScore = new double [6];
//                 double dblAverageScore=0;
//                 int intPlayerCount=0;
//                 double[] dblLongestRoad= new double[8];

//                 InputData(ref strPlayer, ref dblLongestRoad,ref  dblPlayerScore ref strPlayer, );
//                 DisplayPlayerData(strPlayer, dblPlayerScore, intPlayerCount);
//                 CalculateAverageScore(dblPlayerScore,intPlayerCount, dblAverageScore );
//                 Console.ReadLine();
//             }
//             static void InputData(ref string[]strPlayer, ref double[] dblPlayerScore, ref int intPlayerCount,ref double[] dblLongestRoad )
//         }

// //cards
// namespace SettlersOfCatan
// {
//     enum CardType
//     {
//         soldier,
//         yearOfPlenty,
//         monopoly,
//         roadBuilding,
//         victory
//     }

//     class Card
//     {
//         CardType type;

//         public Card(CardType t)
//         {
//             type = t;
//         }

//         public void Use(Player p)
//         {
//            StartBuilding();
//         }
//     }
// }


// bool isContinue = true;
// bool isDouble;

// //CONTINUE FROM HERE AND MODIFY BELOW CODE TO OUR GAME
// do

// {

// Console.Write("Enter Player Name (Q to quit): ");

// strPlayerName[intPlayerCount] = Console.ReadLine();

// //here we check to see if the user asks to stop entering player data. If so, we flip the conditional flag, and continue to the end of the loop.

// if (strPlayerName[intPlayerCount].ToLower() == "q")
// {

// isContinue = false;

// continue;

// }

// //We are looping the entry for the score to validate the data. We do not want erronious characters present, so we parse the input to check if they are accepted as a double

// do

// {

// Console.Write("Enter the score for {0}: ", strPlayerName[intPlayerCount]);

// //try to parse input as a double. If it is acceptable, it returns a true (boolean) value

// isDouble = double.TryParse(Console.ReadLine(), out dblPlayerScore[intPlayerCount]);

// //if the attempted parse returned a false value, we display an error message and represent the entry for the score

// if (!isDouble)

// {

// Console.Write("You have entered invalid data!\n");

// }

// } while (!isDouble);

// Console.WriteLine();

// //increment the amount of players that were added

// intPlayerCount++;

// } while ((intPlayerCount <= 100) && (isContinue));

// }

// static void DisplayPlayerData(string[] strPlayerName, double[] dblPlayerScore, int intPlayerCount)

// {

// Console.WriteLine("Name\t\t\tScore");

// //loop through each array element that holds data to display the players' names and scores.

// for (int i = 0; i <= intPlayerCount - 1; i++)

// {

// Console.WriteLine("{0}\t\t\t{1}", strPlayerName[i], dblPlayerScore[i]);

// }

// }

// static void CalculateAverageScore(double[] dblPlayerscore, int intPlayerCount, ref double dblAverageScore)

// {

// double dblSumOfScores = 0;

// //loop through each element that holds data to accumulate the sum of all the player scores

// for (int i = 0; i <= intPlayerCount - 1; i++)

// {

// dblSumOfScores += dblPlayerscore[i];

// }

// //calculate the average of all the scores by taking the sum and dividing it by the playercount

// dblAverageScore = dblSumOfScores / Convert.ToDouble(intPlayerCount);

// Console.Write("\nAverage Score: ");

// Console.Write("{0}\n", dblAverageScore);

// }

// static void DisplayBelowAverage(string[] strPlayerName, double[] dblPlayerScore, int intPlayerCount, double dblAverageScore)