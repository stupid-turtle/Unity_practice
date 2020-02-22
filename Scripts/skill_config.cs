using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class skill_data {
    public int skill_id;
    public string skill_name;
    public GameObject skill_effect;
    public float skill_delay;
    public float skill_cd;
    public float skill_length;
}

[CreateAssetMenu(menuName = "Editor/Config")]
[Serializable]
public class Skill_config : ScriptableObject{
    // Start is called before the first frame update
    public List<skill_data> skill_config_list = new List<skill_data>();
}
