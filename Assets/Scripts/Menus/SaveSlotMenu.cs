using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotMenu: MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] private MainMenu mainMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button backButton;

    private SaveSlot[] saveSlots;

    private bool isLoadingGame = false;

    private void Awake() 
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot) 
    {
        DisableMenuButtons();

        DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId(),isLoadingGame);
        
        mainMenu.ActivateMenu();
        this.DeactivateMenu();

    }

    public void OnBackClicked() 
    {
        mainMenu.ActivateMenu();
        this.DeactivateMenu();
    }

    public void ActivateMenu(bool isLoadingGame) 
    {
        
        this.gameObject.SetActive(true);

        
        this.isLoadingGame = isLoadingGame;

        
        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesGameData();

        backButton.interactable = true;
        foreach (SaveSlot saveSlot in saveSlots) 
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            
            saveSlot.SetData(profileData);
            if (profileData == null && isLoadingGame) 
            {
                saveSlot.SetInteractable(false);
            }
            else 
            {
                saveSlot.SetInteractable(true);
            }
        }
    }

    public void DeactivateMenu() 
    {
        this.gameObject.SetActive(false);
    }

    private void DisableMenuButtons() 
    {
        foreach (SaveSlot saveSlot in saveSlots) 
        {
            saveSlot.SetInteractable(false);
        }
        backButton.interactable = false;
    }
}