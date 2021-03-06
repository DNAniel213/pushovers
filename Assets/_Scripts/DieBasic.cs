using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieBasic : Die
{
    public override int ThrowDie(Player player)
    {
        Debug.Log("<color=cyan>[INFO] Throwing normal die. </color>", this);

        this.gameObject.transform.position = new Vector3(1000,1000,1000);
        int result = Random.Range(1,6);

        StartCoroutine(WaitForRoll(player));
        this.mat.SetTextureOffset("_MainTex", this.offsets[result]);

        return result;
    }

    public override IEnumerator WaitForRoll (Player player)
    {
        yield return new WaitForSeconds(1.5f);

        this.gameObject.transform.position = player.transform.position + Vector3.up ;
        Vector3 diff = player.enemy.transform.position - (player.transform.position);
        Vector3 force = new Vector3(diff.x , 5, diff.z) * 5;
        this.rb.velocity = new Vector3(0, 2, 0);
        this.rb.AddForce(force,  ForceMode.Impulse);
        this.rb.AddTorque(new Vector3(Random.Range(20,200),Random.Range(20,200),Random.Range(5,200)));
        this.cameraFollow.die = this.transform;
        this.cameraFollow.isRollingDie = true;
        this.cameraFollow.isPlayerMoving = false;

        yield return new WaitForSeconds(this.rollTime);

        this.OnDieThrowEndEvent.Invoke();
    }
    

}
