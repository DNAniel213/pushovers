using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
    public GameObject dieButton;
    public CameraFollow cameraFollow;
    [Header("Players")]
    public PlayerHuman playerPrefab;
    public PlayerAI aiPrefab;
    public Player player, ai;
    private Player turnIdentifier;
    public GameObject playerParent;

    [Header("Dice")]
    public GameObject normalDiePrefab, turboDiePrefab;
    public Die normalDie, blitzDie;
    public int turnTick;


    [Header("WorldGeneration")]
    public WorldGen worldGen;

    public UnityEvent OnEndTurnEvent, OnDieThrowEndEvent;

    private int nextMoveAmount = 0;
    // Start is called before the first frame update
    void Start()
    {
        this.worldGen.Generate();
        this.SpawnPlayers();
        //Show dice throw
        turnIdentifier = this.player;

        OnEndTurnEvent.AddListener(this.EndTurn);
        OnDieThrowEndEvent.AddListener(this.EndDieThrow);
        this.normalDie.OnDieThrowEndEvent = this.OnDieThrowEndEvent;
        this.blitzDie.OnDieThrowEndEvent = this.OnDieThrowEndEvent;
    }

    /// <summary>
    /// Spawns players.
    /// Simple showcase of grasp of polymorphism.
    /// </summary>
    void SpawnPlayers()
    {
        Debug.Log("<color=cyan>[INFO] Spawning Players on EndTiles + 1. </color>", this);
        this.player = Instantiate(playerPrefab, worldGen.endA.NextTile.transform.position, Quaternion.identity);
        this.player.transform.Translate(0,1.4f,0);
        this.player.currentTile = this.worldGen.endA.NextTile;
        this.player.onEndTurnEvent = this.OnEndTurnEvent;
        this.player.transform.parent = this.playerParent.transform;
        this.cameraFollow.player1 = this.player.transform;

        this.ai = Instantiate(aiPrefab, worldGen.endB.PrevTile.transform.position, Quaternion.identity);
        this.ai.transform.Translate(0,1.4f,0);
        this.ai.currentTile = this.worldGen.endB.PrevTile;
        this.ai.onEndTurnEvent = this.OnEndTurnEvent;
        this.ai.transform.parent = this.playerParent.transform;
        this.cameraFollow.player2 = this.ai.transform;

        this.player.enemy = this.ai;
        this.ai.enemy = this.player; //set each other as enemies
        Debug.Log("<color=green>[SUCCESS] Players Spawned. </color>", this);
    }

    public void StartTurn()
    {
        Player currentPlayer = this.turnIdentifier;

        if(currentPlayer.moveIterator == 5) //BlitzMove
        {
            currentPlayer.moveIterator = 0;
            nextMoveAmount = blitzDie.ThrowDie(this.turnIdentifier);
        }
        else
        {
            nextMoveAmount = normalDie.ThrowDie(this.turnIdentifier);
        }
        currentPlayer.moveIterator++;
    }

    public void EndTurn()
    {
        Debug.Log("<color=green>[SUCCESS] Turn ended. </color>", this);
        this.SwitchTurn();
    }

    public void EndDieThrow()
    {
        this.turnIdentifier.Move(nextMoveAmount);
        this.cameraFollow.isRollingDie = false;
    }

    void SwitchTurn()
    {
        Debug.Log("<color=cyan>[INFO] Switched turns. </color>", this);
        if(this.turnIdentifier == this.player)
        {
            dieButton.SetActive(false);
            this.turnIdentifier = this.ai;
            this.turnIdentifier.MyTurn();
            this.StartTurn();
        }
        else
        {
            dieButton.SetActive(true);
            this.turnIdentifier = this.player;
            this.turnIdentifier.MyTurn();

        }

    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
