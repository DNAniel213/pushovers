using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Player : MonoBehaviour
{
    public Tile currentTile; 
    public int moveIterator = 0;
    public float moveSpeed = 2f;
    private Rigidbody rb;
    public Player enemy;
    public Animator animator;
    public UnityEvent onEndTurnEvent, onLoseEvent;
    public bool isMyTurn;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(int moveAmount)
    {
        this.animator.SetBool("isRunning", true);
        StartCoroutine(SmoothLerp(moveSpeed, currentTile, moveAmount));
    }

    /// <summary>
    /// When player is bumped into.
    /// </summary>
    /// <param name="moveAmount"></param>
    public void GetBumped(int moveAmount)
    {
        this.animator.SetTrigger("GetFlungBackwards");
        StartCoroutine(SmoothLerp(moveSpeed, currentTile, moveAmount));
    }

    public void Bump(int moveAmount)
    {
        this.animator.SetTrigger("HitEnemy");
        this.animator.SetBool("isRunning", false);
        this.enemy.Move(moveAmount);
    }

    public IEnumerator Duck()
    {
        yield return new WaitForSeconds(1);
        this.animator.SetTrigger("Duck");


    }


    /// <summary>
    /// Callback when current player's turn.
    /// </summary>
    public abstract void MyTurn();
    /// <summary>
    /// Moves player forward or backwards.
    /// Small showcase on async function and recursive. 
    /// </summary>
    /// <param name="time">Time it takes to reach next tile.</param>
    /// <param name="tile">Which tile player started on.</param>
    /// <param name="iterator">How many more tiles to move until 0.</param>
    /// <returns></returns>
    public abstract IEnumerator SmoothLerp (float time, Tile tile, int iterator);

}
