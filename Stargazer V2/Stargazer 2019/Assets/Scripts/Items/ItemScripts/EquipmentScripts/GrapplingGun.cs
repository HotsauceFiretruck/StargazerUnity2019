using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : Equipment
{

    GrapplingGunData data;

    private Transform gunTip, orientation, player;
    private SpringJoint joint;
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public bool isEquipped = false;
    private Quaternion desiredRotation;
    private float rotationSpeed = 5f;

    void Awake()
    {
        itemData = Instantiate(itemData);
    }

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        data = (GrapplingGunData)itemData;
    }

    void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(orientation.position, orientation.forward, out hit, data.maxDistance, data.grappableLayer))
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            //The distance grapple will try to keep from grapple point. 
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            //Adjust these values to fit your game.
            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;
        }
    }

    void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
    }

    private Vector3 currentGrapplePosition;

    void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }

    void Update()
    {
        if(isEquipped)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartGrapple();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                StopGrapple();
            }

        }
    }

    public override void OnEquipped()
    {
        isEquipped = true;
        data = (GrapplingGunData)itemData;
        if (orientation == null) orientation = transform.parent;
        if (player == null) player = data.ownerEntity.transform;
        if (gunTip == null) gunTip = transform.GetChild(0).transform;
    }
    public override void OnUnequipped()
    {
        isEquipped = false;
        if (orientation != null) orientation = null;
        player = null;
        gunTip = null;
    }

    void LateUpdate()
    {
        DrawRope();
    }
}
