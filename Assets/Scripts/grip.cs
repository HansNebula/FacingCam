using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grip : MonoBehaviour
{
    public Joint r_hand, l_hand, m_end;
    public IKManager manager;
    float s;

    public float calcGripSlope(Joint joint_, Vector3 heading){
        float deltaTheta = 0.01f;
        float distance1 = manager.getDistance(m_end.transform.position, heading);

        joint_.Rotate(deltaTheta);

        float distance2 = manager.getDistance(m_end.transform.position, heading);
        joint_.Rotate(-deltaTheta);
        return (distance2 - distance1) / deltaTheta;
    }
    

    public void Rotate(float range){
        r_hand.Rotate(range);
        l_hand.Rotate(range);
        // return s<=0 ? true : false;
    }
}
