using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkillDataManager : MonoBehaviour
{
    // 1) ����Ʈ�� �����ϰ� �ʹٸ�:
    [SerializeField] private List<BaseSkillData> baseSkillDataList;

    // �Ǵ� Dictionary�� ���� �� ���� ����:
    private Dictionary<int, BaseSkillData> baseSkillDataMap;

    private void Awake()
    {
        // Dictionary �ʱ�ȭ
        baseSkillDataMap = new Dictionary<int, BaseSkillData>();

        // ����Ʈ�� �ε����� key�� ���� (0,1,2,3,...)
        for (int i = 0; i < baseSkillDataList.Count; i++)
        {
            baseSkillDataMap.Add(i, baseSkillDataList[i]);
        }
    }

    // modifierIndex�� �̿��� SkillModifier ��ü ���
    public BaseSkillData GetBaseSkillData(int index)
    {
        if (baseSkillDataMap.TryGetValue(index, out BaseSkillData baseSkillData))
        {
            return baseSkillData;
        }
        return null;
    }
}
