using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerSkillManager playerSkillManager;

    void Awake()
    {
        playerSkillManager = GetComponent<PlayerSkillManager>();
        Init();
    }

    void Init()
    {
        
    }
}
