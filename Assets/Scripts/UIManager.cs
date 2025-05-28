using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Simulation material")]
    [SerializeField] private Renderer waterRenderer;
    private Material instanceMaterial;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI simulationText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            instanceMaterial = waterRenderer.material;

            if (instanceMaterial.GetFloat("_useGerstner") == 1f)
                simulationText.text = "Gerstner Waves";
            else
                simulationText.text = "Sinusoidal Waves";

            return;
        }

        Destroy(gameObject);
    }

    public void ToggleSimulation()
    {
        if (instanceMaterial.GetFloat("_useGerstner") == 0f)
        {
            instanceMaterial.SetFloat("_useGerstner", 1f);
            simulationText.text = "Gerstner Waves";
            return;
        }

        instanceMaterial.SetFloat("_useGerstner", 0f);
        simulationText.text = "Sinusoidal Waves";
    }
}