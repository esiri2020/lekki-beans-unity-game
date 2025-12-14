using UnityEngine;
using UnityEngine.UI;
using GameVanilla.Game.Common;
using GameVanilla.Game.Popups;

namespace GameVanilla.Game.UI
{
    /// <summary>
    /// Manages the shop UI and handles IAP purchases
    /// </summary>
    public class ShopUIManager : MonoBehaviour
    {
        [Header("UI References")]
        public Transform iapItemsParent;
        public GameObject iapRowPrefab;
        public Text coinsText;
        public Button closeButton;
        
        [Header("Shop Panel")]
        public GameObject shopPanel;
        
        private void Start()
        {
            SetupShop();
            UpdateCoinsDisplay();
            
            // Subscribe to coin changes
            if (PuzzleMatchManager.instance != null)
            {
                PuzzleMatchManager.instance.coinsSystem.Subscribe(OnCoinsChanged);
            }
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from coin changes
            if (PuzzleMatchManager.instance != null)
            {
                PuzzleMatchManager.instance.coinsSystem.Unsubscribe(OnCoinsChanged);
            }
        }
        
        /// <summary>
        /// Sets up the shop with IAP items
        /// </summary>
        private void SetupShop()
        {
            if (iapItemsParent == null || iapRowPrefab == null) return;
            
            // Clear existing items
            foreach (Transform child in iapItemsParent)
            {
                Destroy(child.gameObject);
            }
            
            // Create IAP rows for each item
            foreach (var item in PuzzleMatchManager.instance.gameConfig.iapItems)
            {
                var row = Instantiate(iapRowPrefab, iapItemsParent);
                var iapRow = row.GetComponent<IapRow>();
                if (iapRow != null)
                {
                    iapRow.Fill(item);
                    // Set up button click
                    var button = row.GetComponent<Button>();
                    if (button != null)
                    {
                        button.onClick.AddListener(() => OnIAPItemClicked(item));
                    }
                }
            }
        }
        
        /// <summary>
        /// Called when an IAP item is clicked
        /// </summary>
        private void OnIAPItemClicked(IapItem item)
        {
            #if UNITY_IAP
            var storeController = PuzzleMatchManager.instance.iapManager.controller;
            if (storeController != null)
            {
                storeController.InitiatePurchase(item.storeId);
            }
            #else
            // For testing in editor
            PuzzleMatchManager.instance.coinsSystem.BuyCoins(item.numCoins);
            Debug.Log($"Test purchase: {item.numCoins} coins");
            #endif
        }
        
        /// <summary>
        /// Updates the coins display
        /// </summary>
        private void UpdateCoinsDisplay()
        {
            if (coinsText != null)
            {
                var coins = PlayerPrefs.GetInt("num_coins", 0);
                coinsText.text = coins.ToString("n0");
            }
        }
        
        /// <summary>
        /// Called when coins change
        /// </summary>
        private void OnCoinsChanged(int numCoins)
        {
            UpdateCoinsDisplay();
        }
        
        /// <summary>
        /// Opens the shop panel
        /// </summary>
        public void OpenShop()
        {
            if (shopPanel != null)
            {
                shopPanel.SetActive(true);
            }
        }
        
        /// <summary>
        /// Closes the shop panel
        /// </summary>
        public void CloseShop()
        {
            if (shopPanel != null)
            {
                shopPanel.SetActive(false);
            }
        }
        
        /// <summary>
        /// Called when close button is pressed
        /// </summary>
        public void OnCloseButtonPressed()
        {
            CloseShop();
        }
    }
}

