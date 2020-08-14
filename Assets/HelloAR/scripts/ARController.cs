using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GoogleARCore;


#if UNITY_EDITOR
using input = GoogleARCore.InstantPreviewInput;
#endif

public class ARController : MonoBehaviour
{
    public GameObject GridPrefab;
    public GameObject Portal;
    public GameObject ARCamera;
    private List<DetectedPlane> m_NewTrackedPlanes = new List<DetectedPlane>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        if (Session.Status != SessionStatus.Tracking)
        {
            return;
        }
        Session.GetTrackables<DetectedPlane>(m_NewTrackedPlanes, TrackableQueryFilter.New);
        for (int i = 0; i < m_NewTrackedPlanes.Count; i++)
        {
            GameObject grid = Instantiate(GridPrefab, Vector3.zero, Quaternion.identity, transform);
            grid.GetComponent<GridVisualiser>().Initialize(m_NewTrackedPlanes[i]);
        }

        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }
        TrackableHit hit;
        if (Frame.Raycast(touch.position.x, touch.position.y, TrackableHitFlags.PlaneWithinPolygon, out hit))
        {
            Portal.SetActive(true);
            Anchor anchor = hit.Trackable.CreateAnchor(hit.Pose);
            Portal.transform.position = hit.Pose.position;

            Vector3 cameraPosition = ARCamera.transform.position;
            cameraPosition.y = hit.Pose.position.y;

            Portal.transform.LookAt(cameraPosition, Portal.transform.up);
            Portal.transform.parent = anchor.transform;
        }
    }
}
