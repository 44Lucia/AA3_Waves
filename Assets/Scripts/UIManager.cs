using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Simulation material")]
    [SerializeField] private Renderer waterRenderer;
    private Material instanceMaterial;
    private static readonly int UseGerstnerID = Shader.PropertyToID("_useGerstner");

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI simulationText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            instanceMaterial = waterRenderer.material;

            if (instanceMaterial.GetFloat("_useGerstner") == 1f) { simulationText.text = "Gerstner Waves"; }
            else { simulationText.text = "Sinusoidal Waves"; }

            return;
        }

        Destroy(gameObject);
    }

    public void ToggleSimulation()
    {
        bool currentlyGerstner = instanceMaterial.GetFloat(UseGerstnerID) == 1f;
        instanceMaterial.SetFloat(UseGerstnerID, currentlyGerstner ? 0f : 1f);
        simulationText.text = currentlyGerstner ? "Sinusoidal Waves" : "Gerstner Waves";
    }
}