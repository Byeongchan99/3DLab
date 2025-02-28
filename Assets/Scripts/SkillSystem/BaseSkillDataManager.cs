using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkillDataManager : MonoBehaviour
{
    // 1) 리스트로 관리하고 싶다면:
    [SerializeField] private List<BaseSkillData> baseSkillDataList;

    // 또는 Dictionary를 직접 쓸 수도 있음:
    private Dictionary<int, BaseSkillData> baseSkillDataMap;

    private void Awake()
    {
        // Dictionary 초기화
        baseSkillDataMap = new Dictionary<int, BaseSkillData>();

        // 리스트의 인덱스를 key로 매핑 (0,1,2,3,...)
        for (int i = 0; i < baseSkillDataList.Count; i++)
        {
            baseSkillDataMap.Add(i, baseSkillDataList[i]);
        }
    }

    // modifierIndex를 이용해 SkillModifier 객체 얻기
    public BaseSkillData GetBaseSkillData(int index)
    {
        if (baseSkillDataMap.TryGetValue(index, out BaseSkillData baseSkillData))
        {
            return baseSkillData;
        }
        return null;
    }
}
