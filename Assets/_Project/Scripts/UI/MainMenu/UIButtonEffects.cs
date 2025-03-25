using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Изображения кнопки")]
    [SerializeField] private Image buttonImage; // Компонент Image
    [SerializeField] private Sprite normalSprite; // Обычное изображение
    [SerializeField] private Sprite hoverSprite; // Изображение при наведении

    [Header("Звуковые эффекты")]
    [SerializeField] private AudioClip hoverSound; // Звук наведения
    [SerializeField] private AudioClip clickSound; // Звук нажатия
    private AudioSource audioSource;

    void Awake()
    {
        // Добавляем или находим AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSprite != null)
            buttonImage.sprite = hoverSprite; // Меняем фон при наведении

        if (hoverSound != null)
            audioSource.PlayOneShot(hoverSound); // Воспроизводим звук наведения
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (normalSprite != null)
            buttonImage.sprite = normalSprite; // Возвращаем обычное изображение
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickSound != null)
            audioSource.PlayOneShot(clickSound); // Воспроизводим звук нажатия
    }
}
