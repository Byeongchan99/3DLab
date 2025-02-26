using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillContext : MonoBehaviour
{
    public GameObject caster;
    public GameObject target;

    public float damage; // 기본 대미지
    public float damageCoefficient; // 대미지 계수
    public float cost; // 비용
    public float cooldown; // 재사용 대기시간
    public float castTime; // 시전시간
    public float duration; // 지속시간
    public float range; // 사정거리
}
