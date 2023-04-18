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
    { 

       if(actionBuffers.DiscreteActions[0] == 1)
        {
            Thrust();
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
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = 0;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            discreteActionsOut[0] = 1;
        }
      
    }

    private void Thrust()
    {
        if(transform.position.y <= 0.5f)
        {
            rb.AddForce(Vector3.up * Force, ForceMode.Acceleration);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("obstacle"))
        {
            collision.gameObject.transform.position = new Vector3(11, 0.3603692f, 0.002144069f);
            AddReward(-0.5f);
            EndEpisode();
        }
    }

    private void ResetPlayer()
    {
        //this.transform.position = originalPosition;
    }



}
