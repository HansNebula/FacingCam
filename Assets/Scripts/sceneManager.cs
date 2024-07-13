using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour {
    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            SceneManager.LoadScene("SampleScene");
        }
    }
}