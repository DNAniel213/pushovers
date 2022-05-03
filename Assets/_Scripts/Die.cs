using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Die : MonoBehaviour
{
    public Material mat;
    public CameraFollow cameraFollow;
    public UnityEvent OnDieThrowEndEvent;
    public float rollTime = 3f;
    public Rigidbody rb;
    public Vector2[] offsets;
    // Start is called before the first frame update
    void Start()
    {
        this.rb = GetComponent<Rigidbody>();
        this.mat = GetComponent<Renderer>().material;
    }


    public abstract int ThrowDie(Player player);
    public abstract IEnumerator WaitForRoll (Player player);

}
