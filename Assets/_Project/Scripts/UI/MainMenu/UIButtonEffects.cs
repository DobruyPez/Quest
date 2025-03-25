using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("����������� ������")]
    [SerializeField] private Image buttonImage; // ��������� Image
    [SerializeField] private Sprite normalSprite; // ������� �����������
    [SerializeField] private Sprite hoverSprite; // ����������� ��� ���������

    [Header("�������� �������")]
    [SerializeField] private AudioClip hoverSound; // ���� ���������
    [SerializeField] private AudioClip clickSound; // ���� �������
    private AudioSource audioSource;

    void Awake()
    {
        // ��������� ��� ������� AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSprite != null)
            buttonImage.sprite = hoverSprite; // ������ ��� ��� ���������

        if (hoverSound != null)
            audioSource.PlayOneShot(hoverSound); // ������������� ���� ���������
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (normalSprite != null)
            buttonImage.sprite = normalSprite; // ���������� ������� �����������
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickSound != null)
            audioSource.PlayOneShot(clickSound); // ������������� ���� �������
    }
}
