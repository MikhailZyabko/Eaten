﻿using UnityEngine;


public class MovementSurface : MonoBehaviour
{
    private Vector3 _normal;

    public Vector3 MovementVector(Vector3 forward)
    {
        return forward - Vector3.Dot(forward, _normal) * _normal;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision);
        _normal = collision.contacts[0].normal;
    }
}