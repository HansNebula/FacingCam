using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKManager : MonoBehaviour
{
    public Joint m_root, m_end;
    public TurnableBase m_base;
    public GameObject m_target;
    public float m_threshold = 0.05f;
    public float m_rate = 5f;
    public int m_steps=20;
    
    float calculateSlope(Joint joint_){
        float deltaTheta = 0.01f;
        float distance1 = getDistance(m_end.transform.position, m_target.transform.position);

        joint_.Rotate(deltaTheta);

        float distance2 = getDistance(m_end.transform.position, m_target.transform.position);
        joint_.Rotate(-deltaTheta);
        return (distance2 - distance1) / deltaTheta;
    }

    float calcBaseSlope(TurnableBase joint_){
        float deltaTheta = 0.01f;
        float distance1 = getDistance(m_end.transform.position, m_target.transform.position) - 5f;

        joint_.Rotate(deltaTheta);

        float distance2 = getDistance(m_end.transform.position, m_target.transform.position)- 5f;
        joint_.Rotate(-deltaTheta);
        return (distance2 - distance1) / deltaTheta;
    }

    void Update(){
        for(int i=0;i<m_steps;++i){
            if(getDistance(m_end.transform.position, m_target.transform.position) > m_threshold){
                Joint current = m_root;
                
                float baseSlope = calcBaseSlope(m_base);
                m_base.Rotate(-baseSlope * m_rate);

                while(current != null){
                    float slope = calculateSlope(current);
                    current.Rotate(-slope*m_rate);
                    current = current.GetChild();
                }
            }
        }
    }

    float getDistance(Vector3 point1_, Vector3 point2_){
        return Vector3.Distance(point1_, point2_);
    }

}
