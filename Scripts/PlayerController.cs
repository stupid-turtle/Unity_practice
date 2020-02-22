using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour{

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    float speed = 5f;
    float turnSpeed = 15f;
    float nowSpeed;
    float nextActionTime;
    float nextEffectTime;
    bool isWalking;
    bool isAttack;
    bool isSkill1;
    bool isSkill2;
    bool isSkill3;
    public Slider[] skillSliders;

    int[] skillId = new int[20];
    string[] skillName = new string[20];
    GameObject[] skillEffect = new GameObject[20];
    float[] skillDelay = new float[20];
    float[] skillLength = new float[20];
    float[] skillStayLength = new float[20];
    float[] skillDisTime = new float[20];
    int[] skillDamage = new int[20];
    float[] skillCd = new float[20];
    bool[] skillTag = new bool[20];
    float[] skillNextTime = new float[20];
    int runTime;
    public int totHealthPoint = 100;
    public int nowHealthPoint = 63;
    public int addHealthPoint = 0;
    public int totMagicPoint = 80;
    public int nowMagicPoint = 60;
    public int addMagicPoint = 0;
    public int attackPoint = 75;
    public int defensePoint = 50;
    int nowSkill;

    void Start(){
        nowSkill = -1;
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        SkillConfig skills = Resources.Load("Skills/WarriorSkills") as SkillConfig;
        if (skills) {
            for (int i = 0; i < skills.skill_config_list.Count; i++) {
                skill_data data = skills.skill_config_list[i];
                skillId[i] = data.skill_id;
                skillName[i] = data.skill_name;
                skillEffect[i] = data.skill_effect;
                skillDelay[i] = data.skill_delay;
                skillLength[i] = data.skill_length;
                skillStayLength[i] = data.skill_stay_length;
                skillDamage[i] = data.skill_damage;
                skillCd[i] = data.skill_cd;
                skillTag[i] = false;
                skillNextTime[i] = Time.time;
            }
        }
        for (int i = 0; i < skillSliders.Length; i++) {
            skillSliders[i].maxValue = skillSliders[i].value = skillCd[i];
        }
    }

    void Update() {

    }

    void FixedUpdate() {
        float easytouchHorizontal = ETCInput.GetAxis("Horizontal");
        float easytouchVertical = ETCInput.GetAxis("Vertical");
        float nowTime = Time.time;
        if (isAttack || isSkill2 || isSkill3) {
            nowSpeed = 0;
        } else if (nowTime < skillDisTime[2]) {
            nowSpeed = speed * 2;
        } else {
            nowSpeed = speed;
        }
        if (nowTime < skillDisTime[3] && runTime < 10) {
            runTime++;
            Debug.Log(gameObject.transform.forward);
            //gameObject.transform.Translate(gameObject.transform.forward);
            m_Rigidbody.MovePosition(gameObject.transform.position + gameObject.transform.forward);
        }
        ETCInput.SetAxisSensitivity("Horizontal", nowSpeed * Mathf.Abs(easytouchHorizontal));
        ETCInput.SetAxisSensitivity("Vertical", nowSpeed * Mathf.Abs(easytouchVertical));
        bool hasHorizontalInput = !Mathf.Approximately(easytouchHorizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(easytouchVertical, 0f);
        isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);
        if (isWalking && !isAttack && !isSkill2 && !isSkill3) {
            Vector3 forward = new Vector3(easytouchHorizontal, 0f, easytouchVertical);
            forward.Normalize();
            gameObject.transform.forward = forward;
        }
        if (Time.time >= nextActionTime && nowSkill != -1 && skillTag[nowSkill] == true) {
            nowSkill = -1;
            for (int i = 0; i < skillSliders.Length; i++) {
                if (i == 0) isAttack = false;
                else if (i == 1) isSkill1 = false;
                else if (i == 2) isSkill2 = false;
                else if (i == 3) isSkill3 = false;
            }
        }
        for (int i = 0; i < skillSliders.Length; i++) {
            if (Time.time >= skillNextTime[i]) {
                skillTag[i] = false;
                skillSliders[i].value = skillCd[i];
            } else {
                skillSliders[i].value = skillNextTime[i] - Time.time;
            }
        }
        Button attackButton = GameObject.Find("AttackButton").GetComponent<Button>();
        Button skill1Button = GameObject.Find("Skill1Button").GetComponent<Button>();
        Button skill2Button = GameObject.Find("Skill2Button").GetComponent<Button>();
        Button skill3Button = GameObject.Find("Skill3Button").GetComponent<Button>();
        attackButton.onClick.AddListener(Attack);
        skill1Button.onClick.AddListener(Skill1);
        skill2Button.onClick.AddListener(Skill2);
        skill3Button.onClick.AddListener(Skill3);
        m_Animator.SetBool("IsAttack", isAttack);
        m_Animator.SetBool("IsSkill1", isSkill1);
        m_Animator.SetBool("IsSkill2", isSkill2);
        m_Animator.SetBool("IsSkill3", isSkill3);
        if (nowSkill != -1) {
            if (skillTag[nowSkill] == false) {
                skillTag[nowSkill] = true;
                nextActionTime = Time.time + skillLength[nowSkill];
                nextEffectTime = Time.time + skillDelay[nowSkill];
                skillNextTime[nowSkill] = Time.time + skillCd[nowSkill];
                skillDisTime[nowSkill] = Time.time + skillStayLength[nowSkill];
                runTime = 0;
            }
        }
        if (nowSkill != -1 && skillTag[nowSkill] == true && Time.time >= nextEffectTime) {
            Instantiate(skillEffect[nowSkill], gameObject.transform);
            nextEffectTime = nextActionTime;
        }
    }

    void Attack() {
        if (Time.time >= nextActionTime && skillTag[0] == false) {
            nowSkill = 0;
            isAttack = true;
        }
    }

    void Skill1() {
        if (Time.time >= nextActionTime && skillTag[1] == false) {
            nowSkill = 1;
            isSkill1 = true;
        }
    }

    void Skill2() {
        if (Time.time >= nextActionTime && skillTag[2] == false) {
            nowSkill = 2;
            isSkill2 = true;
        }
    }

    void Skill3() {
        if (Time.time >= nextActionTime && skillTag[3] == false) {
            nowSkill = 3;
            isSkill3 = true;
        }
    }
    
}
