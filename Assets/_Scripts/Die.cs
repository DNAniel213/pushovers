using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Die : MonoBehaviour
{
    public CameraFollow cameraFollow;
    public UnityEvent OnDieThrowEndEvent;
    public float rollTime = 3f;
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        this.rb = GetComponent<Rigidbody>();
    }


    public abstract int ThrowDie(Player player);
    public abstract IEnumerator WaitForRoll ();

}
