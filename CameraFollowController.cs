using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowController : MonoBehaviour{

    public float distanceAway = 1.7f;
    public float distanceUp = 1.3f;
    public float smooth = 2f;
    private Vector3 m_TargetPosition;
    Transform follow;
    
    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        follow = GameObject.FindWithTag("Player").transform;
    }

    void LateUpdate() {
        //m_TargetPosition = follow.position + Vector3.up * distanceUp - follow.forward * distanceAway;
        //transform.position = Vector3.Lerp(transform.position, m_TargetPosition, Time.deltaTime * smooth);
        transform.position = new Vector3(follow.position.x, follow.position.y + 10f, follow.position.z - 10f);
        transform.LookAt(follow);
    }
}
