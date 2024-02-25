using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : Kinematic
{
	Seek myMoveType;
	LookWhereGoing mySeekRotateType;

	// Start is called before the first frame update
	protected virtual void Start()
	{
		myMoveType = new Seek {
			character = this,
			target = myTarget,
			flee = false
		};

		mySeekRotateType = new LookWhereGoing {
			character = this,
			target = myTarget,
			maxRotation = maxAngularVelocity
		};
	}

	// Update is called once per frame
	protected override void Update()
	{
		steeringUpdate = new SteeringOutput {
			linear = myMoveType.getSteering().linear,
			angular = mySeekRotateType.getSteering().angular
		};
		base.Update();
	}

	public void SetTarget(GameObject target)
	{
		myTarget = target;
		myMoveType.target = target;
		mySeekRotateType.target = target;
	}
}
