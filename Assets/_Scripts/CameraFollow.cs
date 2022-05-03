// Smooth towards the target

using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform player1, player2, world, die;
    public float smoothTime = 3F;
    private Vector3 velocity = Vector3.zero;
    public Camera cam;

    public bool isRollingDie = false;

    void Update()
    {
        if(isRollingDie)
            this.FollowDie();
        else
            this.FixedCameraFollowSmooth();
    }

    public void FollowDie()
    {
        // How many units should we keep from the players
        float zoomFactor = 0.8f;
        float followTimeDelta = smoothTime * Time.deltaTime;
    
        Vector3 midpoint = (die.position);
    
        float distance = (die.position ).magnitude;

        if(distance < 7)
         distance = 7;
    
        Vector3 cameraDestination = midpoint - cam.transform.forward * distance * zoomFactor;
        cameraDestination.y = 8;
    
        cam.transform.position = Vector3.Slerp(cam.transform.position, cameraDestination, followTimeDelta);

        Quaternion lookOnLook = Quaternion.LookRotation(this.die.position - cam.transform.position);
        cam.transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, followTimeDelta);

        if ((cameraDestination - cam.transform.position).magnitude <= 0.05f)
            cam.transform.position = cameraDestination;
    }
    public void FixedCameraFollowSmooth()
    {
        // How many units should we keep from the players
        float zoomFactor = 0.8f;
        float followTimeDelta = smoothTime * Time.deltaTime;
    
        Vector3 midpoint = (player1.position + player2.position) / 2f;
    
        float distance = (player1.position - player2.position).magnitude;

        if(distance < 10)
         distance = 10;
    
        Vector3 cameraDestination = midpoint - cam.transform.forward * distance * zoomFactor;
        cameraDestination.y = 4;
    
        cam.transform.position = Vector3.Slerp(cam.transform.position, cameraDestination, followTimeDelta);

        Quaternion lookOnLook = Quaternion.LookRotation(midpoint - cam.transform.position);
        cam.transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, followTimeDelta);
        
        if ((cameraDestination - cam.transform.position).magnitude <= 0.05f)
            cam.transform.position = cameraDestination;
    }
}
