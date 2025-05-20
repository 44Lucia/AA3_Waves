using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Simulation Scripts")]
    [SerializeField] private GerstnerWaves gerstnerWaves;
    [SerializeField] private SinusoidalWaves sinusoidalWaves;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI simulationText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            if (gerstnerWaves.enabled) { simulationText.text = "Gerstner Waves"; }
            else { simulationText.text = "Sinusoidal Waves"; }

            return;
        }

        Destroy(gameObject);
    }

    public void ToggleSimulation()
    {
        gerstnerWaves.enabled = !gerstnerWaves.enabled;
        sinusoidalWaves.enabled = !sinusoidalWaves.enabled;

        if (gerstnerWaves.enabled) { simulationText.text = "Gerstner Waves"; }
        else { simulationText.text = "Sinusoidal Waves"; }
    }
}