using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using static UnityEngine.GraphicsBuffer;

public class CubeAgent : Agent
{
    public float speedmultiplier = 0.5f;
    public float rotationmultiplier = 5;
    private bool hasTarget = false;
    public GameObject TargetObject;

    public override void OnEpisodeBegin()
    {
        // reset de positie en orientatie als de agent gevallen is
        if (this.transform.localPosition.y < 0)
        {
            this.transform.localPosition = new Vector3(0, 0.5f, 0);
            this.transform.localRotation = Quaternion.identity;
        }

        // verplaats de target naar een nieuwe willekeurige locatie 
        TargetObject.transform.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {    // Acties, size = 2    
        Vector3 controlSignal = Vector3.zero;
        controlSignal.z = actionBuffers.ContinuousActions[0];
        transform.Translate(controlSignal * speedmultiplier);
        transform.Rotate(0.0f, rotationmultiplier * actionBuffers.ContinuousActions[1], 0.0f);
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, TargetObject.transform.localPosition);
        // target bereikt    
        if (distanceToTarget < 1.42f && !hasTarget && TargetObject.activeInHierarchy)
        {
            SetReward(0.5f);
            hasTarget = true;
            //moet de object niet meeslepen
            TargetObject.SetActive(false);

        }
        else if (hasTarget && this.transform.localPosition.z <= 5.50f && !TargetObject.activeInHierarchy)
        {
            SetReward(1.0f);
            hasTarget = false;
            TargetObject.SetActive(true);
            EndEpisode();

        }

        // Van het platform gevallen?    
        if (this.transform.localPosition.y < 0)
        {
            hasTarget = false;
            TargetObject.SetActive(true);
            EndEpisode();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Agent positie   
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(this.TargetObject.transform.localPosition);

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[1] = Input.GetAxis("Vertical");
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
    }
}