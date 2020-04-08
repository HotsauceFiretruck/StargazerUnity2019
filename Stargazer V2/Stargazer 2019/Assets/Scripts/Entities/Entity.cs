using UnityEngine;
using System;

public class Entity : MonoBehaviour {
    [NonSerialized]
    public Vector3 velocity = Vector3.zero;

    [NonSerialized]
    public Vector3 direction;

    [NonSerialized]
    public Vector3 position;

    [NonSerialized]
    public Equipment equipment;

    [NonSerialized]
    public float currentSpeed = 5.0f;

    public float turnSpeed = 10.0f;
    public float maxSpeed = 5.0f;

    public LayerMask groundLayer;

    public virtual void Death() {
        Destroy(this.gameObject);
    }
}
