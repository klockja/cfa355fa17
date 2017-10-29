﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySightScript : MonoBehaviour 
{
	private EnemyController _enemyController;

	public float viewRadius;
	[Range(0, 360)]
	public float viewAngle;

	public LayerMask targetMask;
	public LayerMask obstacleMask;

	public float meshResolution;

//	[HideInInspector]
	public List<Transform> visibleTargets = new List<Transform>();

	void Start() 
	{
		_enemyController = GetComponentInParent <EnemyController> ();
		StartCoroutine("FindTargetWithDelay", 0.2f);
	}

	void Update()
	{
		DrawFieldOfView ();
	}

	IEnumerator FindTargetWithDelay(float delay) {
		while (true) 
		{
			yield return new WaitForSeconds(delay);
			FindVisibleTargets();
		}
	}

	//Finds targets inside field of view not blocked by walls
	void FindVisibleTargets() {
		visibleTargets.Clear();
		//Adds targets in view radius to an array
		Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);
		//For every targetsInViewRadius it checks if they are inside the field of view
		for (int i = 0; i < targetsInViewRadius.Length; i++) {
			Transform target = targetsInViewRadius[i].transform;
			Vector3 dirToTarget = (target.position - transform.position).normalized;
			if (Vector3.Angle(transform.up, dirToTarget) < viewAngle / 2) {
				float dstToTarget = Vector3.Distance(transform.position, target.position);
				//If line draw from object to target is not interrupted by wall, add target to list of visible 
				//targets
				if (!Physics2D.Raycast (transform.position, dirToTarget, dstToTarget, obstacleMask))
				{
					visibleTargets.Add (target);
					_enemyController.playerInSight = true;
					_enemyController.detectedTransform = target;
				} 
				else
				{
					Debug.Log ("Player isn't in sight!");
					StartCoroutine (WaitForSeconds (3f));
					_enemyController.playerInSight = false;
					Debug.Log ("Unke should start search now");
				}
			}
		}
	}

	void DrawFieldOfView()
	{
		int stepCount = Mathf.RoundToInt (viewAngle * meshResolution);
		float stepAngleSize = viewAngle / stepCount;

		for (int i = 0; i <= stepCount; i++)
		{
			float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
			Debug.DrawLine (transform.position, transform.position + DirFromAngle (angle, true) * viewRadius, Color.red);
		}
	}

	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) 
	{
		if (!angleIsGlobal) 
		{
			angleInDegrees -= transform.eulerAngles.z;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
	}
	public IEnumerator WaitForSeconds (float seconds)
	{
		Debug.Log ("Waiting!");
		yield return new WaitForSeconds (seconds);
	}
}