using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    private Vector3 _pickPos = Vector3.zero;


    private Vector3 _dis = Vector3.zero;
    [SerializeField]
    private float _maxSpeed = 1.0f;

    [SerializeField]
    private float _deceleration = 1.0f;

    private bool _isState = false;
    private bool _isFeel = false;
    private bool _isArrive = false;
    private bool _isSeek = false;

    public Vector3 _velocity = Vector3.zero;

    private void Update()
    {
        
        if (_isState == false)
        {
            //Seek
            if (Input.GetKey(KeyCode.Q))
            {
               
                _isSeek = true;
                _isState = true;
            }

            //Feel
            if (Input.GetKey(KeyCode.W))
            {
               
                _isFeel = true;
                _isState = true;
            }

            //Arrive
            if (Input.GetKey(KeyCode.E))
            {
              
                _isArrive = true;
                _isState = true;
            }
        }
    
        if (_isState == true) { 
            if (Input.GetMouseButtonUp(0))
        {

            Vector3 mouse_pos = Input.mousePosition;



            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenToWorldPoint(mouse_pos), -Vector3.up, out hit, 1000))
            {
                _pickPos = hit.point;
                _pickPos.y = 0.0f;
            }
        }

        float dis = Vector3.Distance(transform.position, _pickPos);
        if ( dis< 10.0f && _isFeel == true)
        {
            _velocity = _velocity + (flee(_pickPos) * Time.deltaTime);
            transform.position = transform.position + _velocity;
        }
        if (dis > 10.0f && _isFeel == true)
        {
            _isFeel = false;
            _isState = false;
        }
            if ( _isSeek == true)
            {
                _velocity = _velocity + (seek(_pickPos) * Time.deltaTime);
                transform.position = transform.position + _velocity;
            }
            if (dis < 0.1f && _isSeek == true)
            {
                _isSeek = false;
                _isState = false;
            }
            if (_isArrive == true)
            {
                _velocity = _velocity + (arrive(_pickPos) * Time.deltaTime);
                transform.position = transform.position + _velocity;
            }
            if (dis < 0.1f && _isArrive == true)
            {
                _isArrive = false;
                _isState = false;
            }
        }
   

    }
    
    public Vector3 getPosition()
    {
        return transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_pickPos, 1.0f);
    }

    private Vector3 seek(Vector3 target_pos)
    {
        // 방향 변경을 위함.
        Vector3 distance = target_pos - transform.position;
        if (distance.sqrMagnitude > 0.005f)
        {
            transform.forward = distance.normalized;
        }

        Vector3 desired_velocity = distance.normalized * _maxSpeed;

        return (desired_velocity - _velocity);
    }

    private Vector3 flee(Vector3 target_pos)
    {
        // seek의 반대 방향 사용.
       
        Vector3 desired_velocity = ((transform.position - target_pos).normalized) * _maxSpeed;

        return (desired_velocity - _velocity);
    }

    private Vector3 arrive(Vector3 target_pos)
    {
        float distance = Vector3.Distance(target_pos, transform.position);

        if (distance > 0.0f)
        {
            Vector3 to_target = target_pos - transform.position;

            float _speed = distance / _deceleration;

            // 최대 속도로 제한.
            _speed = Mathf.Min(_speed, _maxSpeed);

            Vector3 desired_velocity = to_target / distance * _speed;

            return (desired_velocity - _velocity);
        }

        return Vector3.zero;
    }
}
