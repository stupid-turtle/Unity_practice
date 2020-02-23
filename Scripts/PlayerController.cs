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
    bool isSkill4;
    public Slider[] skillSliders;
    int nowSkill;

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
    GameObject[] skillBox = new GameObject[20];
    bool[] skillNewTag = new bool[20];
    int runTime;

    GameObject boxGameObject = null;
    GameObject effectGameObject = null;

    public Text hpText;
    public Text mpText;
    public Text attackText;
    public Text defenseText;
    public int addHealthPoint = 0;
    public int addMagicPoint = 0;
    private float nextAddTime;
    
    private LineRenderer lineRenderer;

    void Start(){
        nowSkill = -1;
        //lineRenderer = transform.GetComponent<LineRenderer>();
        //lineRenderer.startWidth = 0.1f;
        //lineRenderer.endWidth = 0.1f;
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        SkillConfig skills = Resources.Load("Skills/WarriorSkills") as SkillConfig;
        if (skills) {
            for (int i = 0; i < skills.skill_config_list.Count; i++) {
                skill_data skillData = skills.skill_config_list[i];
                skillId[i] = skillData.skill_id;
                skillName[i] = skillData.skill_name;
                skillEffect[i] = skillData.skill_effect;
                skillDelay[i] = skillData.skill_delay;
                skillLength[i] = skillData.skill_length;
                skillStayLength[i] = skillData.skill_stay_length;
                skillDamage[i] = skillData.skill_damage;
                skillCd[i] = skillData.skill_cd;
                skillTag[i] = false;
                skillNextTime[i] = Time.time;
                skillBox[i] = skillData.skill_box;
                skillNewTag[i] = false;
            }
        }
        for (int i = 0; i < skillSliders.Length; i++) {
            skillSliders[i].maxValue = skillSliders[i].value = skillCd[i];
        }
        RoleConfig roles = Resources.Load("Role/Players") as RoleConfig;
        role_data roleData = roles.role_config_list[0];

        hpText.text = "HP: " + roleData.role_now_health_point + "/" + roleData.role_tot_health_point;
        mpText.text = "MP: " + roleData.role_now_magic_point + "/" + roleData.role_tot_magic_point;
        attackText.text = "ATK: " + roleData.role_attack_point;
        defenseText.text = "DIT: " + roleData.role_defense_point;
    }

    void Update() {
        RoleConfig roles = Resources.Load("Role/Players") as RoleConfig;
        role_data roleData = roles.role_config_list[0];
        if (Time.time >= nextAddTime) {
            roleData.role_now_health_point += addHealthPoint;
            if (roleData.role_now_health_point > roleData.role_tot_health_point) roleData.role_now_health_point = roleData.role_tot_health_point;
            roleData.role_now_magic_point += addMagicPoint;
            if (roleData.role_now_magic_point > roleData.role_tot_magic_point) roleData.role_now_magic_point = roleData.role_tot_magic_point;
            nextAddTime += 1f;
        }
        hpText.text = "HP: " + roleData.role_now_health_point + "/" + roleData.role_tot_health_point;
        mpText.text = "MP: " + roleData.role_now_magic_point + "/" + roleData.role_tot_magic_point;
    }

    void FixedUpdate() {
        float easytouchHorizontal = ETCInput.GetAxis("Horizontal");
        float easytouchVertical = ETCInput.GetAxis("Vertical");
        float nowTime = Time.time;

        if (nowTime >= skillDisTime[2]) addHealthPoint = 0;

        // Speed Change
        if (isAttack || isSkill2 || isSkill3 || isSkill4) {
            nowSpeed = 0;
        } else if (nowTime < skillDisTime[2]) {
            nowSpeed = speed * 2;
        } else {
            nowSpeed = speed;
        }
        
        // Attack Box And Effect

        if (nowSkill == 0) {
            Transform transform = gameObject.transform;
            //DrawRectangle(transform, transform.position, skillDamageParameter1[nowSkill], skillDamageParameter2[nowSkill]);
            if (!skillNewTag[0] && skillTag[0] == true && Time.time >= nextEffectTime) {
                skillNewTag[0] = true;
                boxGameObject = GameObject.Instantiate(skillBox[0], transform.position + transform.forward, transform.rotation);
                Destroy(boxGameObject, skillLength[0]);
                effectGameObject = GameObject.Instantiate(skillEffect[0], transform.position + skillEffect[0].transform.position, transform.rotation);
                Destroy(effectGameObject, skillLength[0]);
                nextEffectTime = nextActionTime;
            }
        } else if (nowSkill == 1) {
            if (!skillNewTag[1] && skillTag[1] == true && Time.time >= nextEffectTime) {
                skillNewTag[1] = true;
                //boxGameObject = GameObject.Instantiate(skillBox[1], transform.position + skillBox[1].transform.position, transform.rotation);
                //Destroy(boxGameObject, skillLength[1]);
                effectGameObject = GameObject.Instantiate(skillEffect[1], transform.position, transform.rotation);
                Destroy(effectGameObject, skillLength[1]);
                nextEffectTime = nextActionTime;
            }
            if (effectGameObject != null) {
                //Debug.Log(runTime);
                Transform transform = gameObject.transform;
                effectGameObject.transform.position = m_Rigidbody.position;
                //DrawRectangle(transform, transform.position, skillDamageParameter1[3], skillDamageParameter2[3]);
            }
        } else if (nowSkill == 2) {
            addHealthPoint = 2;
            if (!skillNewTag[2] && skillTag[2] == true && Time.time >= nextEffectTime) {
                skillNewTag[2] = true;
                effectGameObject = GameObject.Instantiate(skillEffect[2], transform.position, transform.rotation);
                Destroy(effectGameObject, skillLength[2]);
                nextEffectTime = nextActionTime;
            }
        } else if (nowSkill == 3) {
            if (!skillNewTag[3] && skillTag[3] == true && Time.time >= nextEffectTime) {
                skillNewTag[3] = true;
                //boxGameObject = GameObject.Instantiate(skillBox[3], transform.position + skillBox[3].transform.position, transform.rotation);
                //Destroy(boxGameObject, skillLength[3]);
                effectGameObject = GameObject.Instantiate(skillEffect[3], transform.position + transform.forward, transform.rotation);
                Destroy(effectGameObject, skillLength[3]);
                nextEffectTime = nextActionTime;
            }
            if (runTime < 10 && effectGameObject != null) {
                //Debug.Log(runTime);
                runTime++;
                Transform transform = gameObject.transform;
                m_Rigidbody.MovePosition(transform.position + transform.forward);
                effectGameObject.transform.position = m_Rigidbody.position;
                //DrawRectangle(transform, transform.position, skillDamageParameter1[3], skillDamageParameter2[3]);
            }
        } else if (nowSkill == 4) {
            if (!skillNewTag[4] && skillTag[4] == true && Time.time >= nextEffectTime) {
                skillNewTag[4] = true;
                boxGameObject = GameObject.Instantiate(skillBox[4], transform.position + skillBox[4].transform.position, transform.rotation);
                Destroy(boxGameObject, skillLength[4]);
                effectGameObject = GameObject.Instantiate(skillEffect[4], transform.position + transform.forward, transform.rotation);
                Destroy(effectGameObject, skillLength[4]);
                nextEffectTime = nextActionTime;
            }
            if (runTime < 45 && effectGameObject != null) {
                //Debug.Log(runTime);
                runTime++;
                Transform transform = gameObject.transform;
                effectGameObject.transform.position += effectGameObject.transform.forward / 5;
                boxGameObject.transform.position = effectGameObject.transform.position;
                //DrawRectangle(transform, transform.position, skillDamageParameter1[3], skillDamageParameter2[3]);
            }
        }


        ETCInput.SetAxisSensitivity("Horizontal", nowSpeed * Mathf.Abs(easytouchHorizontal));
        ETCInput.SetAxisSensitivity("Vertical", nowSpeed * Mathf.Abs(easytouchVertical));
        bool hasHorizontalInput = !Mathf.Approximately(easytouchHorizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(easytouchVertical, 0f);
        isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);
        if (isWalking && !isAttack && !isSkill2 && !isSkill3 && !isSkill4) {
            Vector3 forward = new Vector3(easytouchHorizontal, 0f, easytouchVertical);
            forward.Normalize();
            gameObject.transform.forward = forward;
        }
        if (Time.time >= nextActionTime && nowSkill != -1 && skillTag[nowSkill] == true) {
            if (nowSkill == 0) isAttack = false;
            else if (nowSkill == 1) isSkill1 = false;
            else if (nowSkill == 2) isSkill2 = false;
            else if (nowSkill == 3) isSkill3 = false;
            else if (nowSkill == 4) isSkill4 = false;
            nowSkill = -1;
            //lineRenderer.positionCount = 0;
            
        }
        for (int i = 0; i < skillSliders.Length; i++) {
            if (Time.time >= skillNextTime[i]) {
                //Debug.Log(skillSliders[i].maxValue + "   " + skillSliders[i].value);
                skillNewTag[i] = skillTag[i] = false;
                skillSliders[i].value = skillCd[i];
            } else {
                //Debug.Log(skillSliders[i].maxValue + "   " + skillSliders[i].value);
                skillSliders[i].value = skillNextTime[i] - Time.time;
            }
        }
        Button attackButton = GameObject.Find("AttackButton").GetComponent<Button>();
        Button skill1Button = GameObject.Find("Skill1Button").GetComponent<Button>();
        Button skill2Button = GameObject.Find("Skill2Button").GetComponent<Button>();
        Button skill3Button = GameObject.Find("Skill3Button").GetComponent<Button>();
        Button skill4Button = GameObject.Find("Skill4Button").GetComponent<Button>();
        attackButton.onClick.AddListener(Attack);
        skill1Button.onClick.AddListener(Skill1);
        skill2Button.onClick.AddListener(Skill2);
        skill3Button.onClick.AddListener(Skill3);
        skill4Button.onClick.AddListener(Skill4);
        m_Animator.SetBool("IsAttack", isAttack);
        m_Animator.SetBool("IsSkill1", isSkill1);
        m_Animator.SetBool("IsSkill2", isSkill2);
        m_Animator.SetBool("IsSkill3", isSkill3);
        m_Animator.SetBool("IsSkill4", isSkill4);
        m_Animator.SetBool("IsDie", isDie());
        if (nowSkill != -1) {
            if (skillTag[nowSkill] == false) {
                skillTag[nowSkill] = true;
                nextActionTime = Time.time + skillLength[nowSkill];
                nextEffectTime = Time.time + skillDelay[nowSkill];
                skillNextTime[nowSkill] = Time.time + skillCd[nowSkill];
                skillDisTime[nowSkill] = Time.time + skillStayLength[nowSkill];
                runTime = 0;
                effectGameObject = null;
                boxGameObject = null;
            }
        }
        /*if (nowSkill != -1 && skillTag[nowSkill] == true && Time.time >= nextEffectTime) {
            Transform transform = gameObject.transform;
            Instantiate(skillEffect[nowSkill], transform);
            nextEffectTime = nextActionTime;
        }*/
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

    void Skill4() {
        if (Time.time >= nextActionTime && skillTag[4] == false) {
            nowSkill = 4;
            isSkill4 = true;
        }
    }

    bool isDie() {
        RoleConfig roles = Resources.Load("Role/Players") as RoleConfig;
        role_data roleData = roles.role_config_list[0];
        return roleData.role_now_health_point <= 0;
    }

    void DrawRectangle(Transform transform, Vector3 position, float width, float length) {
        lineRenderer.positionCount = 5;
        lineRenderer.SetPosition(0, position - transform.right * (width / 2f));
        //Debug.Log(position - transform.right * (width / 2f));
        lineRenderer.SetPosition(1, position - transform.right * (width / 2f) + transform.forward * length);
        //Debug.Log(position - transform.right * (width / 2f) + transform.forward * length);
        lineRenderer.SetPosition(2, position + transform.right * (width / 2f) + transform.forward * length);
        //Debug.Log(position + transform.right * (width / 2f) + transform.forward * length);
        lineRenderer.SetPosition(3, position + transform.right * (width / 2f));
        //Debug.Log(position + transform.right * (width / 2f));
        lineRenderer.SetPosition(4, position - transform.right * (width / 2f));
        //Debug.Log(position - transform.right * (width / 2f));
    }

}
