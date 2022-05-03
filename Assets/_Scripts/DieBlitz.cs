using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieBlitz : Die
{

    public override int ThrowDie(Player player)
    {
        Debug.Log("<color=cyan>[INFO] Throwing blitz die. </color>", this);
        int result = Random.Range(5,10);        
        this.gameObject.transform.position = new Vector3(1000,1000,1000);

        StartCoroutine(WaitForRoll(player));
        this.mat.SetTextureOffset("_MainTex", this.offsets[result]);

        return result;
    }

    public override IEnumerator WaitForRoll (Player player)
    {
        yield return new WaitForSeconds(1.5f);
        
        this.gameObject.transform.position = player.transform.position;
        this.rb.AddForce((player.enemy.transform.position - transform.position),  ForceMode.Impulse);
        this.rb.AddTorque(new Vector3(Random.Range(0,5),Random.Range(0,5),Random.Range(0,5)));
        this.cameraFollow.die = this.transform;
        this.cameraFollow.isRollingDie = true;

        yield return new WaitForSeconds(this.rollTime);

        this.OnDieThrowEndEvent.Invoke();
        
    }
}
