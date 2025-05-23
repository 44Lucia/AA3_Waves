using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Simulation material")]
    [SerializeField] private Material waterMaterial;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI simulationText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            if (waterMaterial.GetFloat("_useGerstner") == 1f) { simulationText.text = "Gerstner Waves"; }
            else { simulationText.text = "Sinusoidal Waves"; }

            return;
        }

        Destroy(gameObject);
    }

    public void ToggleSimulation()
    {
        if (waterMaterial.GetFloat("_useGerstner") == 0f)
        {
            waterMaterial.SetFloat("_useGerstner", 1f);
            simulationText.text = "Gerstner Waves";
        }
        else
        {
            waterMaterial.SetFloat("_useGerstner", 0f);
            simulationText.text = "Sinusoidal Waves";
        }
    }
}