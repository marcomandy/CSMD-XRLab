using TMPro;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float timerDuration = 30f;
    private float currentTime;

    void Start()
    {
        currentTime = timerDuration;
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            timerText.text = currentTime.ToString("00:00:00");
        }
        else
        {
            timerText.text = "Done!";
        }
    }
}
