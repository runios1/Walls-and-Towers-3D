using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private PlayerMainScript playerScript;
    private Dictionary<string, Placeable> itemPrefabs;


    [Header("Placeable Prefabs")]
    [SerializeField] private Placeable towerPrefab;
    [SerializeField] private Placeable wallPrefab;
    [Header("Preview")]
    private GameObject itemPreviewInstance;
    public float previewDistance = 10.0f;
    public float previewOpacity = 0.5f;
    public Transform player;
    public float rotationSpeed = 100.0f;
    private float rotationAngle = 0.0f;
    public Transform cam;
    float turnSmoothVelocity;

    void Start()
    {
        itemPrefabs = new Dictionary<string, Placeable>()
        {
            { "Tower", towerPrefab },
            {"Wall", wallPrefab},
        };
    }
    void Update()
    {
        UpdatePreviewPosition();
    }
    public void Buy(string itemID)
    {
        if (itemPrefabs.TryGetValue(itemID, out Placeable itemPrefab))
        {
            if (playerScript.LoseCoins(itemPrefab.cost))
            {
                RemovePrieview();
                PlaceItem(itemPrefab);
            }
        }
        else
            Debug.Log($"Item {itemID} not found");

    }
    public void ShowPreview(string itemID)
    {
        RemovePrieview();
        if (itemPrefabs.TryGetValue(itemID, out Placeable itemPrefab))
        {
            Vector3 previewPosition = CalculatePreviewPostion();

            itemPreviewInstance = Instantiate(itemPrefab.prefab, previewPosition, player.rotation);
            SetOpacity(itemPreviewInstance, previewOpacity);

        }
        // Adjust the opacity of the preview object
    }

    public void PlaceItem(Placeable item)
    {
        Vector3 previewPosition = CalculatePreviewPostion();
        Vector3 adjustedPosition = new Vector3(previewPosition.x, 0, previewPosition.z);

        Instantiate(item.prefab, adjustedPosition, Quaternion.Euler(0, rotationAngle, 0)); // Use the current rotation
    }
    public void RemovePrieview()
    {
        try
        {
            Destroy(itemPreviewInstance);
        }
        catch { }
        itemPreviewInstance = null;
    }
    public void UpdatePreviewPosition()
    {
        if (itemPreviewInstance != null)
        {
            // Calculate the position ahead of the player
            Vector3 previewPosition = CalculatePreviewPostion();

            // Update the preview position
            itemPreviewInstance.transform.position = previewPosition;

            // Check for mouse wheel input to adjust rotation
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollInput != 0)
            {
                rotationAngle += scrollInput * rotationSpeed * Time.deltaTime;
            }

            // Apply the updated rotation to the preview object
            itemPreviewInstance.transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
        }
    }
    private Vector3 CalculatePreviewPostion()
    {
        //player
        // Calculate the angle based on the camera's forward direction
        Vector3 output = player.position + cam.forward.normalized * previewDistance;
        output.y = 0;

        return output;
    }
    private void SetOpacity(GameObject obj, float opacity)
    {
        // Get all renderers in the preview object
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            // Adjust the material's color alpha value to set opacity
            foreach (Material mat in renderer.materials)
            {
                Color color =
                new Color(0, 1, 0, 0.5f);
                color.a = opacity;
                mat.color = color;
                mat.SetFloat("_Mode", 2); // Ensure material uses transparent rendering mode
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = 3000;
            }
        }
    }

}
