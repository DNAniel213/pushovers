using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHuman : Player
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.name = "Human";
        audioSource = this.GetComponent<AudioSource>();
    }
    public override void MyTurn()
    {
        Debug.Log("<color=cyan>[INFO] Human's turn. </color>", this);
        this.transform.rotation = Quaternion.LookRotation((this.enemy.transform.position - this.transform.position), Vector3.up);
        this.isMyTurn = true;
    }

    public override IEnumerator SmoothLerp (float time, Tile tile, int iterator )
    {
        Debug.Log($"<color=cyan>[INFO] Player moving 1 Tile out of {iterator}. </color>", this);

        Vector3 startingPos  = transform.position;
        Vector3 targetPos = Vector3.zero;
        Tile chainedTile = tile;
        if(iterator > 0)
        {
            targetPos = tile.NextTile.transform.position;
            chainedTile = tile.NextTile;
            iterator--;
        }
        else
        {
            targetPos = tile.PrevTile.transform.position;
            chainedTile = tile.PrevTile;

            if(chainedTile is TileEnd)
            {
                this.onLoseEvent.Invoke();
                yield break;
            }

            iterator++;
        }
        
        Vector3 rotationMask = new Vector3(0,1,0);
        Vector3 lookAtRotation = Quaternion.LookRotation(targetPos - this.transform.position).eulerAngles;
        this.transform.rotation = Quaternion.Euler(Vector3.Scale(lookAtRotation, rotationMask));

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
                this.animator.SetBool("isRunning", false);
                yield return new WaitForSeconds(2);
                this.isMyTurn = false;
                this.onEndTurnEvent.Invoke();
            }
            else
            {
                this.animator.SetBool("isRunning", false);
                yield return new WaitForSeconds(2);
                this.onEndTurnEvent.Invoke();
            }

        }

    }
}
