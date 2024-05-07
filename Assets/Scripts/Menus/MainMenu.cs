using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] private SaveSlotMenu saveSlotsMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button SaveGameButton;
    [SerializeField] private Button loadGameButton;

    public void OnLoadGameClicked() 
    {
        saveSlotsMenu.ActivateMenu(true);
        this.DeactivateMenu();
    }

    public void OnSaveGameClicked() 
    {
        saveSlotsMenu.ActivateMenu(false);
        this.DeactivateMenu();
    }

    private void DisableMenuButtons() 
    {
        SaveGameButton.interactable = false;
    }

    public void ActivateMenu() 
    {
        this.gameObject.SetActive(true);
    }

    public void DeactivateMenu() 
    {
        this.gameObject.SetActive(false);
    }
 
}
