using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AgentWander : MonoBehaviour
{

    public Vector3 _velocity = Vector3.zero;

    [SerializeField]
    private float _maxSpeed = 1.0f;

    [SerializeField]
    private float _deceleration = 10.0f;

    private float radius = 5.0f;

    private float m_dwanderJitter = 1.0f;
    private float wander_Distance = 5.0f;
    [SerializeField]
    private int result;
    private int theata;
    private bool _isWander = false;
    Vector3 m_wanderTarget = Vector3.zero;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Y))
        {
            _isWander = true;
        }
        if (_isWander)
        {
            theata = Random.Range(0, 360);
            m_wanderTarget = vec3((radius * Mathf.Cos(theata)), (radius * Mathf.Sin(theata)));
            _velocity = _velocity + wander();

            transform.position = transform.position + _velocity;
        }
    }

    //private Vector3 truncate(Vector3 ve1 , Vector3 ve2)
    //{
    //    float min;
    //    float mag1 = Vector3.Magnitude(ve1);
    //    float mag2 = Vector3.Magnitude(ve2);
    //    min=Mathf.Min(mag1, mag2);

    //    if (min == mag1) { 
    //    return ve1;
    //    }else { 
    //    return ve2;
    //    }
    //}
    private Vector3 wander()
    {
        float JitterThisTimeslice = m_dwanderJitter * Time.deltaTime;

        m_wanderTarget += vec3(RandomClamped() * JitterThisTimeslice, RandomClamped() * JitterThisTimeslice);

        m_wanderTarget.Normalize();

        m_wanderTarget *= radius;

        
        Vector3 target = m_wanderTarget + vec3(wander_Distance, 0);

        Vector3 Target = transform.position + target;

        return arrive(Target);

    }

    private float RandomClamped()
    {
        result = Random.Range(-1, 2);
        print(result);
        return result;
    }
    private Vector3 vec3(float a, float b)
    {
        Vector3 z = transform.position;
        z.x = a;
        z.y = 0;
        z.z = b;

        return z;

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
