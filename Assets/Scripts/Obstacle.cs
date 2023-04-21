using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    public float moveSpeed = 10f;
    public GameObject player;
    private bool flip = false;

    // Update is called once per frame
    void Update()
    {
        Debug.Log(this.transform.position.z);
        this.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        
        if (this.transform.position.z >= -0.9 && flip == false)
        {
            this.transform.position += new Vector3(0, 0, -0.1f);
            if (this.transform.position.z == -0.9)
                flip = true;
        }
        if (flip == true && this.transform.position.z >= 0.9)
            this.transform.position += new Vector3(0, 0, 0.1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("wall"))
        {
            this.gameObject.transform.position = new Vector3(11, 0.3603692f, 0.002144069f);
            player.GetComponent<Agent>().AddReward(0.9f);
            player.GetComponent<Agent>().EndEpisode();

        }
    }
}
