using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class MoveController : MonoBehaviour{

    public List<AnimationClip> clipList = new List<AnimationClip>();
    private Animator animator;
    PlayableGraph playableGraph;
    
    // Start is called before the first frame update
    void Start(){
        animator = this.gameObject.GetComponent<Animator>();

        playableGraph = PlayableGraph.Create();
        playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
        AnimationPlayableOutput playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", animator);
        AnimationClipPlayable clipPlayable = AnimationClipPlayable.Create(playableGraph, clipList[0]);
        playableOutput.SetSourcePlayable(clipPlayable);
        playableGraph.Play();
    }

    // Update is called once per frame
    void Update(){
        
    }

    void OnDestroy() {
        playableGraph.Destroy();
    }
}
