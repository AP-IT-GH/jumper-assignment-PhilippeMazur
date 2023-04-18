using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    public float moveSpeed = 3.5f;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("wall"))
        {
            //Destroy(this.gameObject);
            this.gameObject.transform.position = new Vector3(11, 0.3603692f, 0.002144069f);
            player.GetComponent<Agent>().AddReward(1.0f);
            player.GetComponent<Agent>().EndEpisode();

        }
    }
}