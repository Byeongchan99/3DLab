using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierManager : MonoBehaviour
{
    // 1) 리스트로 관리하고 싶다면:
    [SerializeField] private List<SkillModifier> modifiersList;

    // 또는 Dictionary를 직접 쓸 수도 있음:
    private Dictionary<int, SkillModifier> modifierMap;

    private void Awake()
    {
        // Dictionary 초기화
        modifierMap = new Dictionary<int, SkillModifier>();

        // 리스트의 인덱스를 key로 매핑 (0,1,2,3,...)
        for (int i = 0; i < modifiersList.Count; i++)
        {
            modifierMap.Add(i, modifiersList[i]);
        }
    }

    // modifierIndex를 이용해 SkillModifier 객체 얻기
    public SkillModifier GetModifier(int index)
    {
        if (modifierMap.TryGetValue(index, out SkillModifier mod))
        {
            return mod;
        }
        return null;
    }
}
