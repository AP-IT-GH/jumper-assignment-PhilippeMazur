using UnityEngine;
using Unity.MLAgents;

public class Obstacle : MonoBehaviour
{

    public float moveSpeed = 10f;
    public GameObject player;
    private bool flip;

    // Update is called once per frame
    void Update()
    {
        Debug.Log(this.transform.position.z);
        this.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        if (flip == false)
            this.transform.Translate(-Vector3.forward * (moveSpeed + 5) * Time.deltaTime);
        else
            this.transform.Translate(Vector3.forward * (moveSpeed + 5) * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("wall"))
        {
            this.gameObject.transform.position = new Vector3(11, 0.3603692f, 0.002144069f);
            player.GetComponent<Agent>().AddReward(0.9f);
            player.GetComponent<Agent>().EndEpisode();
        }

        if (collision.gameObject.CompareTag("flip"))
            flip = true;
        if (collision.gameObject.CompareTag("flip2"))
            flip = false;
    }
}
