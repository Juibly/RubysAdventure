using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIHandler : MonoBehaviour
{
    private VisualElement m_Healthbar;
    public static UIHandler instance { get; private set; }

    // UI dialogue window variables
    public float displayTime = 4.0f;
    private VisualElement m_NonPlayerDialogue;
    private VisualElement m_NonPlayerDialogue2;
    private float m_TimerDisplay;
    private float m_TimerDisplay2;

    // UI GameOver
    private VisualElement m_GameOver;


    // Awake is called when the script instance is being loaded (in this situation, when the game scene loads)
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        m_Healthbar = uiDocument.rootVisualElement.Q<VisualElement>("HealthBar");
        SetHealthValue(1.0f);


        m_NonPlayerDialogue = uiDocument.rootVisualElement.Q<VisualElement>("NPCDialogue");
        m_NonPlayerDialogue.style.display = DisplayStyle.None;
        m_TimerDisplay = -1.0f;

        m_NonPlayerDialogue2 = uiDocument.rootVisualElement.Q<VisualElement>("NPC2Dialogue");
        m_NonPlayerDialogue2.style.display = DisplayStyle.None;
        m_TimerDisplay2 = -1.0f;

        m_GameOver = uiDocument.rootVisualElement.Q<VisualElement>("GameOverUI");
        m_GameOver.style.display = DisplayStyle.None;

    }



    private void Update()
    {
        if (m_TimerDisplay > 0)
        {
            m_TimerDisplay -= Time.deltaTime;
            if (m_TimerDisplay < 0)
            {
                m_NonPlayerDialogue.style.display = DisplayStyle.None;
            }


        }
        if (m_TimerDisplay2 > 0)
        {
            m_TimerDisplay2 -= Time.deltaTime;
            if (m_TimerDisplay2 < 0)
            {
                m_NonPlayerDialogue2.style.display = DisplayStyle.None;
            }


        }
    }


    public void SetHealthValue(float percentage)
    {
        m_Healthbar.style.width = Length.Percent(100 * percentage);
    }


    public void DisplayDialogue()
    {
        m_NonPlayerDialogue.style.display = DisplayStyle.Flex;
        m_TimerDisplay = displayTime;
    }

    public void DisplayDialogue2()
    {
        m_NonPlayerDialogue2.style.display = DisplayStyle.Flex;
        m_TimerDisplay2 = displayTime;
    }

    public void DisplayGameOver()
    {
        m_GameOver.style.display = DisplayStyle.Flex;
    }
}