using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierManager : MonoBehaviour
{
    // 1) ����Ʈ�� �����ϰ� �ʹٸ�:
    [SerializeField] private List<SkillModifier> modifiersList;

    // �Ǵ� Dictionary�� ���� �� ���� ����:
    private Dictionary<int, SkillModifier> modifierMap;

    private void Awake()
    {
        // Dictionary �ʱ�ȭ
        modifierMap = new Dictionary<int, SkillModifier>();

        // ����Ʈ�� �ε����� key�� ���� (0,1,2,3,...)
        for (int i = 0; i < modifiersList.Count; i++)
        {
            modifierMap.Add(i, modifiersList[i]);
        }
    }

    // modifierIndex�� �̿��� SkillModifier ��ü ���
    public SkillModifier GetModifier(int index)
    {
        if (modifierMap.TryGetValue(index, out SkillModifier mod))
        {
            return mod;
        }
        return null;
    }
}
