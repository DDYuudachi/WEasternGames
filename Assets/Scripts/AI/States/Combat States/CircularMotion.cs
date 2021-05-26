using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMotion : MonoBehaviour
{
    private float _speed;
    private Vector3 _centre;
    private float _angle;
    private float _radius;
    private GameObject _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _speed = 1f;
        _radius = 5f;
    }

    void Update()
    {
        _centre = _player.transform.position;
        _angle += _speed * Time.deltaTime;

        Vector3 offset = new Vector3(Mathf.Sin(_angle), 0, Mathf.Cos(_angle)) * _radius;
        transform.position = _centre + offset;
    }
}
