using UnityEngine;
using UnityEngine.UI;

public class GuideManager : MonoBehaviour
{
    public Image guideImage; // Reference to the Image component
    public Button nextButton;
    public Button prevButton;

    private int currentIndex = 0;
    public Sprite[] guideImages; // Array to store guide images

    void Start()
    {
        UpdateGuide();
        prevButton.onClick.AddListener(PreviousPage);
        nextButton.onClick.AddListener(NextPage);
    }

    void UpdateGuide()
    {
        guideImage.sprite = guideImages[currentIndex];

        // Disable buttons at boundaries
        prevButton.interactable = currentIndex > 0;
        nextButton.interactable = currentIndex < guideImages.Length - 1;
    }

    void NextPage()
    {
        if (currentIndex < guideImages.Length - 1)
        {
            currentIndex++;
            UpdateGuide();
        }
    }

    void PreviousPage()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateGuide();
        }
    }
}
