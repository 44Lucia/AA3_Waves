using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FloatingObject : MonoBehaviour
{
    [Header("Floating Object Settings")]
    [SerializeField] private Transform[] floaters;
    [SerializeField] private float underWaterDrag = 3f;
    [SerializeField] private float underWaterAngularDrag = 1f;
    [SerializeField] private float airWaterDrag = 0f;
    [SerializeField] private float airWaterAngularDrag = 0.05f;
    [SerializeField] private float floatingPower = 15f;

    [Header("Water Settings")]
    [SerializeField] private float baseWaterHeight = 0f;
    [SerializeField] private float waterHeightVariation = 2f;
    [SerializeField] private float waveSpeed = 1.0f;
    private float waterHeight;

    private Rigidbody rb;
    private int floatersUnderwater;
    private bool isUnderwater;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        waterHeight = baseWaterHeight + Mathf.Sin(Time.time * waveSpeed) * (waterHeightVariation / 2f);

        floatersUnderwater = 0;
        for (int i = 0; i < floaters.Length; i++)
        {
            float diff = floaters[i].position.y - waterHeight;

            if (diff < 0)
            {
                rb.AddForceAtPosition(floatingPower * Mathf.Abs(diff) * Vector3.up, floaters[i].position, ForceMode.Force);
                floatersUnderwater++;
                if (!isUnderwater)
                {
                    isUnderwater = true;
                    UpdateState();
                }
            }
        }


        if (isUnderwater && floatersUnderwater == 0)
        {
            isUnderwater = false;
            UpdateState();
        }
    }

    private void UpdateState()
    {
        if (isUnderwater)
        {
            rb.drag = underWaterDrag;
            rb.angularDrag = underWaterAngularDrag;
            return;
        }

        rb.drag = airWaterDrag;
        rb.angularDrag = airWaterAngularDrag;
    }
}