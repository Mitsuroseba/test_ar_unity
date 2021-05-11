using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARSubsystems;

public class ARTapToPlace : MonoBehaviour
{
    public GameObject objectToPlace;
    public GameObject placementIndicator;
    
    private ARSessionOrigin arOrigin;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    
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
    private void UpdatePlacementPose()
    {
    	var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
    	var hits = new List<ARRaycastHit>();
    	var rayCastMgr = GetComponent<ARRaycastManager>();
        rayCastMgr.Raycast(screenCenter, hits, TrackableType.Planes);
        placementPoseIsValid = hits.Count > 0;	
        if (placementPoseIsValid)
        {
        	placementPose = hits[0].pose;
        }
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
}
