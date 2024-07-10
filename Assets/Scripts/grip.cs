using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grip : MonoBehaviour
{
    public GameObject r_hand, l_hand;
    float s;
    
    void Update(){
        if(Input.GetKey(KeyCode.G)){
            rotate(5f);
        }
    }

    public bool rotate(float range){
        // s=range;
        if(range!=0){
            r_hand.transform.Rotate(Vector3.up * -1f);
            l_hand.transform.Rotate(Vector3.up * -1f);
            range--;
        }
        return range==0 ? true : false;
    }
}
