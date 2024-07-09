using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum robotState{
    pose_def_,
    lifting_,
    dropping_,    
}

public enum liftSubState{
    pose_def_,
    lifting_,
    dropping_,    
}
public class IKManager : MonoBehaviour
{
    public Joint m_root, m_end;
    public TurnableBase m_base;
    public GameObject m_target, turnableBase;
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
        float distance1 = getDistance(m_end.transform.position, m_target.transform.position);

        joint_.Rotate(deltaTheta);

        float distance2 = getDistance(m_end.transform.position, m_target.transform.position);
        joint_.Rotate(-deltaTheta);
        return (distance2 - distance1) / deltaTheta;
    }

    void Update(){
        // spontan();
    }

    float getDistance(Vector3 point1_, Vector3 point2_){
        return Vector3.Distance(point1_, point2_);
    }
    //prosedural
    public void spontan(){
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

    public robotState generalState;
    public void prosedur(){
        switch(generalState){
            case robotState.pose_def_   : RobotPosDef(); break;
            case robotState.lifting_    : RobotLifting(); break;
            case robotState.dropping_   : RobotDropping(); break;
        }
    }

    public void RobotPosDef(){

    }

    public void RobotLifting(){

    }

    public void RobotDropping(){

    }

    public void gripping(){

    }

}
