using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour{

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;
    float speed = 15f;
    float nextActionTime;
    bool isWalking;
    bool isAttack;
    float attackLength = 1.2f;
    public Slider attackSlider;
    float stayLength = 1.2f - 0.25f;
    bool isSkill;
    float skillLength = 5.5f;
    public Slider skillSlider;
    public GameObject skillEffect;
    public GameObject attackEffect;

    public int totHealthPoint = 100;
    public int nowHealthPoint = 63;
    public int totMagicPoint = 80;
    public int nowMagicPoint = 60;
    public int attackPoint = 75;
    public int defensePoint = 50;
    
    private ParticleSystem xuanfengzhan;

    void Start(){
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        attackSlider.value = attackLength;
        skillSlider.value = skillLength;
    }

    void Update() {

    }

    void FixedUpdate() {
        float keyboardHorizontal = Input.GetAxis("Horizontal");
        float keyboardVertical = Input.GetAxis("Vertical");
        float easytouchHorizontal = ETCInput.GetAxis("Horizontal");
        float easytouchVertical = ETCInput.GetAxis("Vertical");

        if (!isAttack) m_Movement.Set(keyboardHorizontal, 0f, keyboardVertical);
        else m_Movement.Set(0f, 0f, 0f);
        m_Movement.Normalize();
        bool hasHorizontalInput = !Mathf.Approximately(keyboardHorizontal, 0f) || !Mathf.Approximately(easytouchHorizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(keyboardVertical, 0f) || !Mathf.Approximately(easytouchVertical, 0f);
        isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);
        if (Time.time >= nextActionTime) {
            isAttack = false;
            isSkill = false;
            skillSlider.value = skillLength;
            attackSlider.value = attackLength;
        } else {
            if (isAttack) {
                attackSlider.value = nextActionTime - Time.time;
            } else if (isSkill) {
                skillSlider.value = nextActionTime - Time.time;
            }
        }
        Button attackButton = GameObject.Find("AttackButton").GetComponent<Button>();
        Button skillButton = GameObject.Find("SkillButton").GetComponent<Button>();
        attackButton.onClick.AddListener(Attack);
        skillButton.onClick.AddListener(Skill);
        m_Animator.SetBool("IsAttack", isAttack);
        m_Animator.SetBool("IsSkill", isSkill);
        if (isAttack == true && Time.time >= nextActionTime - stayLength) {
            Instantiate(attackEffect, gameObject.transform);
        }
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, speed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);
    }

    void Attack() {
        if (Time.time >= nextActionTime) {
            isAttack = true;
            nextActionTime = Time.time + attackLength;
        }
    }

    void Skill() {
        if (Time.time >= nextActionTime) {
            isSkill = true;
            nextActionTime = Time.time + skillLength;
            Instantiate(skillEffect, gameObject.transform);
        }
    }

    void OnAnimatorMove() {
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * 0.1f);
        m_Rigidbody.MoveRotation(m_Rotation);
    }
}
