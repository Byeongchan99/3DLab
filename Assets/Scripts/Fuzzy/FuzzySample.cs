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
     * 모든 커브를 평가하고 부동소수점 값을 반환한다.
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
     * 사용자가 입력한 체력 %에 기반해
     * 평가된 값으로 GUI를 갱신한다.
     */
    private void SetLabels()
    {
        healthyLabel.text = string.Format(labelText, healthyValue);
        hurtLabel.text = string.Format(labelText, hurtValue);
        cirticalLabel.text = string.Format(labelText, criticalValue);
    }
}
