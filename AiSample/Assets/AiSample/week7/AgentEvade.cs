using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentEvade : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Agent _target = null;

    [SerializeField]
    private float _maxSpeed = 0.2f;
    private bool _isEvade = false;
    private Vector3 _velocity = Vector3.zero;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            _isEvade = true;
        }

        if (_isEvade)
        {
            float dis = Vector3.Distance(_target.transform.position, transform.position);
            if (dis < 20)
            {
                _velocity = _velocity + evade(_target);

                transform.position = transform.position + _velocity;
            }


        }

    }
    private Vector3 evade(Agent target_agent)
    {
        Vector3 ToPursuer = target_agent.transform.position - transform.position;

        const float ThreatRange = 10.0f;

        if (ToPursuer.magnitude > ThreatRange * ThreatRange)
            return flee(target_agent.transform.position - target_agent._velocity);

        float LookAheadTime = (ToPursuer.magnitude / (target_agent._velocity.magnitude + _maxSpeed));

        return flee(target_agent.transform.position + target_agent._velocity * LookAheadTime);

    }
    private Vector3 flee(Vector3 target_pos)
    {
        // seek의 반대 방향 사용.
        Vector3 desired_velocity = ((transform.position - target_pos).normalized) * _maxSpeed;

        return (desired_velocity - _velocity);
    }
}
