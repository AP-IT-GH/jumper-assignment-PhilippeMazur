using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class JumpCubeAgent : Agent
{

    public float Force = 15f;
    private Rigidbody rb = null;
    private Vector3 originalPosition = new Vector3(23.68312f, 0.5f, -0.004343987f);
    private bool isJumping = false;
    private GameObject obstacle = null;


    public override void Initialize()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        obstacle = GameObject.Find("Obstacle");

    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {    // Acties, size = 2    
        Vector3 controlSignal = Vector3.zero;
        controlSignal.z = actionBuffers.ContinuousActions[0];
        //transform.Translate(controlSignal * speedmultiplier);
        //transform.Rotate(0.0f, rotationmultiplier * actionBuffers.ContinuousActions[1], 0.0f);
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, obstacle.transform.localPosition);
        // target bereikt    
        if (distanceToTarget < 3f)
        {
            

        }
        else if (this.transform.localPosition.z <= -7.12f)
        {

        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Agent positie   
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(this.obstacle.transform.localPosition);

    }

    public override void OnEpisodeBegin()
    {
        ResetPlayer();
        isJumping = false;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Thrust();

        }
    }

    private void Thrust()
    {
        Debug.Log("Thrusting!");
        rb.AddForce(Vector3.up * Force, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("obstacle"))
        {
            Destroy(collision.gameObject);
            
            AddReward(-1f);
            EndEpisode();
        }else if (collision.gameObject.CompareTag("Zone"))
        {
            Debug.Log("floor");
            isJumping = false;
        }
        else
        {
            Debug.Log("air");
            AddReward(0.1f);
            isJumping = true;
        }
    }

    private void ResetPlayer()
    {
        this.transform.position = originalPosition;
    }



}
