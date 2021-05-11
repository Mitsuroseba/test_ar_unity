using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARSubsystems;
using System;

[RequireComponent(typeof(ARRaycastManager))]
public class ARTapToPlace : MonoBehaviour
{
    public GameObject objectToPlace;
    public GameObject placementIndicator;

    private ARRaycastManager m_RaycastManager;
    private ARSessionOrigin arOrigin;
    private Pose placementPose;
    private bool placementPoseIsValid = false;

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
        	PlaceObject();
        }
    }
    private void PlaceObject()
    {
    	Instantiate(objectToPlace, placementPose.position, placementPose.rotation);
    }

    private void UpdatePlacementIndicator()
    {
    	 if (placementPoseIsValid)
        {
        	placementIndicator.SetActive(true);
        	placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
        	placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
    	var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
    	var hits = new List<ARRaycastHit>();

        m_RaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

}
