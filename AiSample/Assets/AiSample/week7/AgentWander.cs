using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AgentWander : MonoBehaviour
{

    public Vector3 _velocity = Vector3.zero;

    private float max_force = 0.1f;

    [SerializeField]
    private float _maxSpeed = 0.1f;

    [SerializeField]
    private float _deceleration = 10.0f;

    private float radius = 10.0f;

    private float m_dwanderJitter = 1.0f;
    private float wander_Distance = 10.0f;
    [SerializeField]
    private int result;
    private int theata;
    private bool _isWander = false;
    Vector3 m_wanderTarget = Vector3.zero;
    Vector3 steering = Vector3.zero;

    float timer;
    int waitTime;
    // Update is called once per frame

        void Start()
    {
        waitTime =1;
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetKey(KeyCode.Y))
        {
            _isWander = true;
        }
        if (_isWander)
        {
            theata = Random.Range(0, 360);

            //길이제한
            if (waitTime < timer) { 
            steering  = Vector3.ClampMagnitude(seek(wander())+_velocity, max_force);
            steering.y = 0;
            _velocity = steering;
                timer = 0;
            }
          
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
       
            m_wanderTarget = vec3((radius * Mathf.Cos(theata)), (radius * Mathf.Sin(theata)));
        float JitterThisTimeslice = m_dwanderJitter * Time.deltaTime;
 
        m_wanderTarget += vec3(RandomClamped() * JitterThisTimeslice, RandomClamped() * JitterThisTimeslice);

        m_wanderTarget.Normalize();

        m_wanderTarget *= radius;

        
        Vector3 target = m_wanderTarget + vec3(wander_Distance, 0);

        Vector3 Target = transform.position + target;
       
      
        return Target;

    }

    private float RandomClamped()
    {
        result = Random.Range(-1, 2);
       
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
}
