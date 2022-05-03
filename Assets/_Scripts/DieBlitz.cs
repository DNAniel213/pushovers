using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieBlitz : Die
{

    public override int ThrowDie(Player player)
    {
        Debug.Log("<color=cyan>[INFO] Throwing blitz die. </color>", this);
        int result = Random.Range(5,10);        
        this.gameObject.transform.position = player.transform.position;
        this.rb.AddForce((player.enemy.transform.position - transform.position) * 2, ForceMode.Impulse);

        StartCoroutine(WaitForRoll());
        return result;
    }

    public override IEnumerator WaitForRoll ()
    {
        this.cameraFollow.die = this.transform;
        this.cameraFollow.isRollingDie = true;
        yield return new WaitForSeconds(this.rollTime);
        this.OnDieThrowEndEvent.Invoke();
        
    }
}
