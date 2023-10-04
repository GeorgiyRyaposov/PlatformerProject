using Game.Gui.MainMenu;
using Game.Input;
using UnityEngine;

namespace Game.Gui.GameMenu
{
    public class GameMenuPresenter
    {
        private MainMenuView view;
        
        public void Attach(MainMenuView mainMenuView)
        {
            view = mainMenuView;
            
            view.NewGame.onClick.AddListener(NewGameClicked);
            view.SaveGame.onClick.AddListener(SaveGameClicked);
            view.LoadGame.onClick.AddListener(LoadGameClicked);
            view.Exit.onClick.AddListener(ExitClicked);
            
            view.LoadGame.interactable = ServicesMediator.GameStarter.HasSavedGame;

            InputMediator.InputEventsHolder.Paused += OnPaused;
            
            Show(false);
        }

        public void Detach()
        {
            view = null;
            InputMediator.InputEventsHolder.Paused -= OnPaused;
        }
        
        private void NewGameClicked()
        {
            ServicesMediator.GameStarter.StartNewGame();
        }
        
        private void LoadGameClicked()
        {
            ServicesMediator.GameStarter.LoadGame();
        }
        
        private void SaveGameClicked()
        {
            ServicesMediator.GameDataLoader.SaveData();
        }

        private void ExitClicked()
        {
            Application.Quit();
        }
        
        private void OnPaused()
        {
            Show(!view.gameObject.activeSelf);
        }

        private void Show(bool show)
        {
            view.gameObject.SetActive(show);
        }
    }
}