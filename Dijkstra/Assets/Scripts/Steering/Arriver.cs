using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arriver : Kinematic
{
	Arrive myMoveType;
	LookWhereGoing myRotateType;

	// Start is called before the first frame update
	protected virtual void Start()
	{
		myMoveType = new Arrive();
		myMoveType.character = this;
		myMoveType.target = myTarget;

		myRotateType = new LookWhereGoing();
		myRotateType.character = this;
		myRotateType.target = myTarget;
		myRotateType.maxRotation = maxAngularVelocity;
	}

	// Update is called once per frame
	protected override void Update()
	{
		steeringUpdate = new SteeringOutput {
			linear = myMoveType.getSteering().linear,
			angular = myRotateType.getSteering().angular
		};
		base.Update();
	}

	public void SetTarget(GameObject target)
	{
		myTarget = target;
		myMoveType.target = target;
		myRotateType.target = target;
	}
}
