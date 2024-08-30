using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class ImageRotator : MonoBehaviour
{
    public List<Sprite> images; // List to hold the images
    public UnityEngine.UI.Image imageComponent; 
    public float rotationSpeed = 90f; 
    public float fadeDuration = 1f;
    public float interval = 5f; 

    private int currentIndex = 0;
    private float timer = 0f;
    private bool isFadingOut = false;
    private bool isFadingIn = false;
    private float fadeTimer = 0f;
    private Color originalColor;

    private void Start()
    {
        if (images.Count > 0 && imageComponent != null)
        {
            imageComponent.sprite = images[currentIndex];
            originalColor = imageComponent.color;
        }
    }

    private void Update()
    {
        // Update the main timer
        timer += Time.deltaTime;

        // Check if it's time to switch images
        if (timer >= interval)
        {
            // Start the fade-out process
            isFadingOut = true;
            fadeTimer = 0f;
            timer = 0f;
        }

        // Handle fading out
        if (isFadingOut)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration);
            SetImageAlpha(alpha);

            if (fadeTimer >= fadeDuration)
            {
                isFadingOut = false;
                SwitchImage();
                isFadingIn = true;
                fadeTimer = 0f;
            }
        }

        // Handle fading in
        if (isFadingIn)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, fadeTimer / fadeDuration);
            SetImageAlpha(alpha);

            if (fadeTimer >= fadeDuration)
            {
                isFadingIn = false;
            }
        }
    }

    private void SwitchImage()
    {
        // Rotate the image by setting its rotation
        imageComponent.rectTransform.rotation *= Quaternion.Euler(0, 0, rotationSpeed);

        // Switch to the next image
        currentIndex = (currentIndex + 1) % images.Count;
        imageComponent.sprite = images[currentIndex];

        // Reset rotation if needed
        imageComponent.rectTransform.rotation = Quaternion.identity;
    }

    private void SetImageAlpha(float alpha)
    {
        Color color = imageComponent.color;
        color.a = alpha;
        imageComponent.color = color;
    }
}
