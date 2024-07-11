using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum robotState{
    pose_def_,
    lifting_,
    dropping_,    
}

public enum actionSubState{
    pose_def_,
    turning_,
    heading_,
    gripping_,
    lift_,
    loosen_,    
}
public class IKManager : MonoBehaviour
{
    public Joint m_root, m_end;
    public TurnableBase m_base;
    public grip m_grip;
    public GameObject m_target, turnableBase, redTarget, blueTarget;
    public float m_threshold = 0.05f;
    public float m_rate = 5f;
    public int m_steps=20;

    public float posDef_x = 3f, posDef_y = 3f, posDef_z=0, thresh_end = -0.025f;

    public robotState generalState;
    public actionSubState liftState;

    public bool isRed, m_robotLifting;

    float calculateSlope(Joint joint_, Vector3 heading){
        float deltaTheta = 0.01f;
        float distance1 = getDistance(m_end.transform.position, heading);

        joint_.Rotate(deltaTheta);

        float distance2 = getDistance(m_end.transform.position, heading);
        joint_.Rotate(-deltaTheta);
        return (distance2 - distance1) / deltaTheta;
    }

    float calcBaseSlope(TurnableBase joint_, Vector3 heading){
        float deltaTheta = 0.01f;
        float distance1 = getDistance(m_end.transform.position, heading);

        joint_.Rotate(deltaTheta);

        float distance2 = getDistance(m_end.transform.position, heading);
        joint_.Rotate(-deltaTheta);
        return (distance2 - distance1) / deltaTheta;
    }

    void Start(){
        generalState = robotState.pose_def_;
    }
    void Update(){
        // spontan();
        prosedur();
        generalStateButton();
    }

    float getDistance(Vector3 point1_, Vector3 point2_){
        return Vector3.Distance(point1_, point2_);
    }
    //prosedural
    public void spontan(){
        for(int i=0;i<m_steps;++i){
            if(getDistance(m_end.transform.position, m_target.transform.position) > m_threshold){
                Joint current = m_root;
                
                float baseSlope = calcBaseSlope(m_base, m_target.transform.position);
                m_base.Rotate(-baseSlope * m_rate);

                while(current != null){
                    float slope = calculateSlope(current, m_target.transform.position);
                    current.Rotate(-slope*m_rate);
                    current = current.GetChild();
                }
            }
        }
    }

    public void prosedur(){
        switch(generalState){
            case robotState.pose_def_   : RobotPosDef(); break;
            case robotState.lifting_    : RobotLifting(); break;
            case robotState.dropping_   : RobotDropping(); break;
        }
    }

    int ab=0;
    public void RobotPosDef(){
        Vector3 calib=new Vector3(m_base.transform.position.x, m_base.transform.position.y + 100f, m_base.transform.position.z);
        Vector3 heading=new Vector3(m_base.transform.position.x + posDef_x, m_base.transform.position.y + posDef_y, m_base.transform.position.z + posDef_z);

        for(int i=0;i<m_steps;++i){
            if(getDistance(m_end.transform.position, calib) > m_threshold && ab<1){
                Joint current = m_root;
                while(current != null && getDistance(m_end.transform.position, calib) > m_threshold){
                    float slope = calculateSlope(current, calib);
                    current.Rotate(-slope*100f);
                    current = current.GetChild();
                }
                ab++;
            }else if(getDistance(m_end.transform.position, heading) > m_threshold && ab!=0){
                Joint current = m_root;
                while(current != null){
                    float slope = calculateSlope(current, heading);
                    current.Rotate(-slope*m_rate);
                    current = current.GetChild();
                }
            }
        }

    }

    public float range=5f;
    public void RobotLifting(){
        if(m_robotLifting){
            switch(liftState){
                case actionSubState.pose_def_: 
                    RobotPosDef();
                    liftState = actionSubState.turning_;
                    break;
                case actionSubState.turning_:
                    if(turning(m_target.transform.position)){
                        liftState = actionSubState.heading_;
                    }
                    break;
                case actionSubState.heading_:
                    if(headingTo(m_target.transform.position)){;
                        liftState = actionSubState.gripping_;
                    }else{
                        headingTo(m_target.transform.position);
                    }
                    break;
                case actionSubState.gripping_:
                    if(m_grip.rotate(range)){
                        liftState = actionSubState.lift_;
                    }
                    break;
            }
        }
    }

    public void RobotDropping(){

    }



    public void gripping(){

    }

    public bool turning(Vector3 target){
        bool reached=false;
        float baseSlope = calcBaseSlope(m_base, target);
        for(int i=0;i<m_steps;++i){
            if(getDistance(m_end.transform.position, target) > m_threshold && baseSlope != 0){
                Joint current = m_root;

                baseSlope = calcBaseSlope(m_base, target);
                m_base.Rotate(-baseSlope * m_rate);
                // reached = false;
            }
            // reached = true;
        }
        return baseSlope == 0 ? true : false;
    }
    
    public bool headingTo(Vector3 target){
        for(int i=0;i<m_steps;++i){
            if(getDistance(m_end.transform.position, m_target.transform.position) > m_threshold){
                Joint current = m_root;
                while(current != null){
                    float slope = calculateSlope(current, m_target.transform.position);
                    current.Rotate(-slope*m_rate);
                    current = current.GetChild();
                }
            }
        }
        return getDistance(m_end.transform.position, m_target.transform.position) < m_threshold ? true : false;
    }

    public void generalStateButton(){
        if(Input.GetKeyDown(KeyCode.P)){
            generalState = robotState.pose_def_;
        }
        if(Input.GetKeyDown(KeyCode.L)){
            generalState = robotState.lifting_;
            m_robotLifting = true;
        }
        if(Input.GetKeyDown(KeyCode.D)){
            generalState = robotState.dropping_;
        }
        if(Input.GetKeyDown(KeyCode.S)){
            spontan();
        }
    }
}
