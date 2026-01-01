using UnityEngine;

namespace LUP.PCR
{
    public class FarmTaskUIPresenter
    {
        private IFarmTaskUIView view;
        private FarmTaskUIModel model;
        private MainUIPresenter mainPresenter;
        private ProductableBuilding currBuilding;

        public void InitPresenter(IFarmTaskUIView view, FarmTaskUIModel model, MainUIPresenter mainPresenter)
        {
            this.view = view;
            this.model = model;
            this.mainPresenter = mainPresenter;

            view.OnClickBack += HandleBackClick;
            view.OnClickWorkRequest += HanldeWorkRequestToggle;
            view.OnClickUpgrade += HandleUpgrade;
        }

        private void HanldeWorkRequestToggle()
        {
            if(currBuilding == null)
            {
                return;
            }
            currBuilding.ToggleWorkRequest();

            UpdateUI(currBuilding);
        }

        private void HandleUpgrade()
        {
            if (currBuilding == null)
            {
                return;
            }

            currBuilding.Upgrade();

            UpdateUI(currBuilding);
        }



        private void HandleBackClick()
        {
            Hide();
            mainPresenter.Show();
        }

        public void Show()
        {
            view.Show();
        }

        public void Hide()
        {
            view.Hide();
            currBuilding = null;
        }

        public void UpdateUI(ProductableBuilding building)
        {
            currBuilding = building;
            if (currBuilding)
            {
                model.UpdateData(currBuilding);
            }

            UpdateUIData(model.uiData);
        }

        public void UpdateUIData(FarmUIData data)
        {
            view.UpdateUIStats(data);
        }

    }

}
