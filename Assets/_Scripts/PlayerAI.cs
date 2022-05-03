using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAI : Player
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.name = "AI";
        
    }

    public override void MyTurn()
    {
        Debug.Log("<color=cyan>[INFO] AI's turn. </color>", this);
        this.isMyTurn = true;

    }
    public override void Move(int moveAmount)
    {
        StartCoroutine(SmoothLerp(moveSpeed, currentTile, moveAmount));
    }
    public override void Bump(int moveAmount)
    {
        this.enemy.Move(moveAmount);
    }
    public override IEnumerator SmoothLerp (float time, Tile tile, int iterator )
    {
        Debug.Log($"<color=cyan>[INFO] AI moving 1 Tile out of {iterator} </color>", this);
        Vector3 startingPos  = transform.position;
        Vector3 targetPos = Vector3.zero;
        Tile chainedTile = tile;
        if(iterator > 0)
        {
            targetPos = tile.PrevTile.transform.position;
            chainedTile = tile.PrevTile;
            iterator--;
        }
        else
        {
            targetPos = tile.NextTile.transform.position;
            chainedTile = tile.NextTile;
            iterator++;
        }

        if(chainedTile == this.enemy.currentTile)
        {
            this.Bump((iterator + 2) * -1);
        }
        else
        {
            this.currentTile = chainedTile;
            targetPos.y = startingPos.y;
            float elapsedTime = 0;
            
            while (elapsedTime < time)
            {
                transform.position = Vector3.Lerp(startingPos, targetPos, (elapsedTime / time));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPos;
            if(iterator != 0)
                StartCoroutine(SmoothLerp(time, chainedTile, iterator));
            else if(this.isMyTurn)
            {
                this.isMyTurn = false;
                this.onEndTurnEvent.Invoke();
            }
            else
            {
                this.onEndTurnEvent.Invoke();
            }
        }
    }
}
