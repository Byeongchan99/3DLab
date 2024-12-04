using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuzzySample : MonoBehaviour
{
    private const string labelText = "{0} true";
    public AnimationCurve critical;
    public AnimationCurve hurt;
    public AnimationCurve healthy;

    public InputField healthInput;

    public Text healthyLabel;
    public Text hurtLabel;
    public Text cirticalLabel;

    private float criticalValue = 0f;
    private float hurtValue = 0f;
    private float healthyValue = 0f;

    private void Start()
    {
        SetLabels();
    }

    /*
     * ��� Ŀ�긦 ���ϰ� �ε��Ҽ��� ���� ��ȯ�Ѵ�.
    */
    public void EvaluateStatements()
    {
        if (string.IsNullOrEmpty(healthInput.text))
        {
            return;
        }
        float inputValue = float.Parse(healthInput.text);

        healthyValue = healthy.Evaluate(inputValue);
        hurtValue = hurt.Evaluate(inputValue);
        criticalValue = critical.Evaluate(inputValue);

        SetLabels();
    }

    /*
     * ����ڰ� �Է��� ü�� %�� �����
     * �򰡵� ������ GUI�� �����Ѵ�.
     */
    private void SetLabels()
    {
        healthyLabel.text = string.Format(labelText, healthyValue);
        hurtLabel.text = string.Format(labelText, hurtValue);
        cirticalLabel.text = string.Format(labelText, criticalValue);
    }
}
