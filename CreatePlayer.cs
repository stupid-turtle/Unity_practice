using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlayer : MonoBehaviour{

    public GameObject prefab;
    private GameObject playerObject;
    // Start is called before the first frame update
    void Start(){
        playerObject = GameObject.Instantiate(prefab);
        
    }

    // Update is called once per frame
    void Update(){
        
    }
}
