using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        towerPrefab.tag = "Untagged";
        wallPrefab.tag = "Untagged";
    }
    void Update()
    {
        UpdatePreviewPosition();
    }
    public bool Buy(string itemID)
    {
        if (itemPrefabs.TryGetValue(itemID, out Placeable itemPrefab))
        {
            Vector3 previewPosition = CalculatePreviewPostion();
            if (CanPlaceItem(previewPosition, itemPrefab))
            {
                if (playerScript.LoseCoins(itemPrefab.cost))
                {
                    RemovePreview();
                    itemPrefab.tag = itemID;
                    PlaceItem(itemPrefab, previewPosition);
                    return true;
                }
            }
            else
            {
                Debug.Log("Cannot place item here, something is in the way.");
            }
        }
        else
            Debug.Log($"Item {itemID} not found");

        RemovePreview();
        return false;
    }
    public void ShowPreview(string itemID)
    {
        RemovePreview();
        if (itemPrefabs.TryGetValue(itemID, out Placeable itemPrefab))
        {
            Vector3 previewPosition = CalculatePreviewPostion();

            itemPreviewInstance = Instantiate(itemPrefab.prefab, previewPosition, player.rotation);
            SetOpacity(itemPreviewInstance, previewOpacity, new Color(0, 1, 0, 0.5f)); // Default to green

        }
        // Adjust the opacity of the preview object
    }

    private bool CanPlaceItem(Vector3 position, Placeable itemPrefab)
    {
        if (itemPrefab.TryGetComponent<BoxCollider>(out var boxCollider))
        {
            Vector3 size = boxCollider.size;
            int layerMask = ~LayerMask.GetMask("Inside Prefab", "Ground", "UI", "Player", "Enemy");


            Collider[] colliders = Physics.OverlapBox(position, size / 2, Quaternion.identity, layerMask)
                .Where(collider => collider is BoxCollider).ToArray();
            bool output = colliders.Length <= 1;
            if (!output)
            {
                Debug.Log("CanPlaceItem = false Colliders: ");
                foreach (Collider collider in colliders)
                {
                    Debug.Log(collider.ToString());
                }
            }

            return output;
        }
        else
        {
            Debug.LogError("Placeable prefab does not have a BoxCollider.");
        }
        return false;
    }

    public void PlaceItem(Placeable item, Vector3 position)
    {

        // Assuming the placeable has a BoxCollider, adjust the size accordingly
        //towerPrefab.tag = "";
        GameObject placeableGameObject = Instantiate(item.prefab, position, Quaternion.Euler(0, rotationAngle, 0)); // Use the current rotation
        placeableGameObject.GetComponent<Placeable>().state = Placeable.PlaceableState.Placed;


    }
    public void RemovePreview()
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

            // Check if the item can be placed at the preview position
            bool canPlace = CanPlaceItem(previewPosition, itemPreviewInstance.GetComponent<Placeable>());

            // Set the color based on whether the item can be placed
            Color previewColor = canPlace ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);
            SetOpacity(itemPreviewInstance, previewOpacity, previewColor);


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
        // Calculate the angle based on the camera's forward direction
        Vector3 output = player.position + cam.forward.normalized * previewDistance;
        output.y = 0;

        return output;
    }
    private void SetOpacity(GameObject obj, float opacity, Color color)
    {
        // Get all renderers in the preview object
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            // Adjust the material's color alpha value to set opacity
            foreach (Material mat in renderer.materials)
            {
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
