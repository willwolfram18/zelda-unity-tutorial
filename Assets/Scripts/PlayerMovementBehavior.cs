using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerMovementBehavior : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;

    private Vector2 _positionChange;
    
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    public void OnMovementSupplied(InputAction.CallbackContext context) =>
        _positionChange = context.ReadValue<Vector2>();

    private void FixedUpdate()
    {
        MoveCharacter(Time.deltaTime);
    }

    private void MoveCharacter(float deltaTime)
    {
        _animator.SetBool("IsMoving", _positionChange != Vector2.zero);
        if (_positionChange == Vector2.zero)
        {
            return;
        }
        
        _rigidbody.MovePosition(
            (Vector2)transform.position + (_positionChange * _speed * deltaTime)
        );
        
        _animator.SetFloat("MoveX", _positionChange.x);
        _animator.SetFloat("MoveY", _positionChange.y);
    }
}
