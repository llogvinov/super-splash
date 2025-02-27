using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Action<string> InputDone;

    private string _axis;
    private float _horizontalInput;
    private float _verticalInput;

    private void Update()
    {
        if (Input.GetAxis("Horizontal") != 0f)
        {
            _horizontalInput = Input.GetAxis("Horizontal");
            _axis = _horizontalInput > 0f ? "Right" : "Left";
            InputDone?.Invoke(_axis);
            return;
        }
        
        if (Input.GetAxis("Vertical") != 0f)
        {
            _verticalInput = Input.GetAxis("Vertical");
            _axis = _verticalInput > 0f ? "Up" : "Down";
            InputDone?.Invoke(_axis);
            return;
        }
    }
}