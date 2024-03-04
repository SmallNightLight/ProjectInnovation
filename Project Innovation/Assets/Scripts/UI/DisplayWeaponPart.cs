using ScriptableArchitecture.Data;
using UnityEngine;
using UnityEngine.UI;

public class DisplayWeaponPart : MonoBehaviour
{
    [SerializeField] private Image _barrelpart;
    [SerializeField] private Image _basepart;
    [SerializeField] private Image _stockpart;

    public void AddPart(WeaponPartData part)
    {
        switch (part.PartType)
        {
            case WeaponPartType.Barrel:
                _barrelpart.sprite = part.Icon;
                break;
            case WeaponPartType.Base:
                _basepart.sprite = part.Icon;
                break;
            case WeaponPartType.Stock:
                _stockpart.sprite = part.Icon;
                break;
        }
    }
}