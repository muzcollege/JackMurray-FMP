using UnityEngine;

public class GameRespawn : MonoBehaviour
{

    public float threshold;


    void FixedUpdate()
    {
        if(transform.position.y < threshold)
        {
            // make sure to change the numbers to where you want the player to spawn in and make sure to change the threshold between 1 and 10
            transform.position = new Vector3(0f, 3.57f, 0f);
        }
    }
}
