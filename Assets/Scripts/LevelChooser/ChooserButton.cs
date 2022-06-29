using UnityEngine;
using UnityEngine.UI;

public class ChooserButton : MonoBehaviour
{
    [SerializeField] private GameObject _chooserMenu;

    public void ShowMenu()
    {
        _chooserMenu.SetActive(true);
        Debug.Log("МЕНЮ ОТКРЫЛОСь");
    }
}
