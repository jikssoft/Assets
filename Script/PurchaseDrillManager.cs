using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SA.IOSNative.StoreKit;
using SA.Analytics.Google;

public class PurchaseDrillManager : MonoBehaviour {

    public string[] iphone_purchase_table =
        {"com.twocm.tapdrill.purchase.default.drill",
        "com.twocm.tapdrill.purchase.burger.drill",
        "com.twocm.tapdrill.purchase.brush.drill",
        "com.twocm.tapdrill.purchase.egg.drill",
        "com.twocm.tapdrill.purchase.fakebook.drill",
        "com.twocm.tapdrill.purchase.basketballshoes.drill",
        "com.twocm.tapdrill.purchase.sunbulb.drill",
        "com.twocm.tapdrill.purchase.coffeemachine.drill",
        "com.twocm.tapdrill.purchase.dynamite.drill",
        "keyboard.drill",
        "com.twocm.tapdrill.purchase.fish.drill",
        "gold.drill",
        "green.drill",
        "com.twocm.tapdrill.purchase.gun.drill",
        "com.twocm.tapdrill.purchase.honey.drill",
        "com.twocm.tapdrill.purchase.hook.drill",
        "com.twocm.tapdrill.purchase.lance.drill",
        "laser.drill",
        "com.twocm.tapdrill.purchase.lolipop.drill",
        "com.twocm.tapdrill.purchase.speakermic.drill",
        "com.twocm.tapdrill.purchase.milk.drill",
        "com.twocm.tapdrill.purchase.moon.drill",
        "com.twocm.tapdrill.purchase.pen.drill",
        "com.twocm.tapdrill.purchase.metal.drill",
        "worldcup.drill",
        "nego.drill",
        "com.twocm.tapdrill.purchase.ufo.drill",
        "com.twocm.tapdrill.purchase.icepolarbear.drill",
        "icecoke.drill",
        "com.twocm.tapdrill.purchase.corn.drill",
        "revolver.drill",
        "skull.drill",
        "com.twocm.tapdrill.purchase.smartphone.drill",
        "com.twocm.tapdrill.purchase.soap.drill",
        "com.twocm.tapdrill.purchase.stone.drill",
        "com.twocm.tapdrill.purchase.thief.drill",
        "com.twocm.tapdrill.purchase.thor.drill",
        "com.twocm.tapdrill.purchase.white.drill",
        "com.twocm.tapdrill.purchase.wood.drill",
        "com.twocm.tapdrill.purchase.woodpecker.drill",
        "com.twocm.tapdrill.purchase.blue.drill",
        "yellow.drill",
        "com.twocm.tapdrill.purchase.sflaser.drill",
        "goldrock.drill",
        "com.twocm.tapdrill.purchase.brickman.drill",
        "com.twocm.tapdrill.purchase.rainbow.drill",
        "com.twocm.tapdrill.purchase.rolex.drill",
        "com.twocm.tapdrill.purchase.squirrel.drill",
        "com.twocm.tapdrill.purchase.alarm.drill",
        "com.twocm.tapdrill.purchase.weather.drill",
		"com.twocm.tapdrill.purchase.test",
        "com.twocm.tapdrill.purchase.corgi.drill"};

	static string iphone_add_id = ".add";

    static PurchaseDrillManager instance;

    // Use this for initialization
    void Start () {

        instance = this;

        store_inited = false;

        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            foreach (string id in iphone_purchase_table)
            {
				if (id.Equals ("com.twocm.tapdrill.purchase.default.drill") == true) {
					continue;
				}

				//Debug.Log("====== id1 : " + id);
                if (id.Contains("com.") == true)
                {
					PaymentManager.Instance.AddProductId(id + iphone_add_id);
					//Debug.Log("====== id2 : " + id);
                }
            }
            
            PaymentManager.OnStoreKitInitComplete += IPhone_PaymentManager_OnStoreKitInitComplete;
            PaymentManager.Instance.LoadStore();
        }
        else if( Application.platform == RuntimePlatform.Android)
        {
            foreach (string id in iphone_purchase_table)
            {
                if (id.Contains("com.") == true)
                {
                    GoogleProductTemplate tpl = new GoogleProductTemplate();
                    tpl.SKU = id;
                    tpl.ProductType = AN_InAppType.NonConsumable;
                    AndroidInAppPurchaseManager.Client.AddProduct(tpl);
                }
            }

            Debug.Log("============ Init InApp android");

            //listening for store initialising finish
            AndroidInAppPurchaseManager.ActionBillingSetupFinished += Android_OnBillingConnected;

            //you may use loadStore function without parameter if you have filled base64EncodedPublicKey in plugin settings
            AndroidInAppPurchaseManager.Client.Connect();

        }


		local_currency = "USD$ 0.99";
	}


    static bool store_inited;
    void IPhone_PaymentManager_OnStoreKitInitComplete(SA.Common.Models.Result obj)
    {
        if (obj.IsSucceeded)
        {
            int ProductsCount = 0;
            foreach (Product product in PaymentManager.Instance.Products)
            {
                if (product.IsAvailable)
                {
                    ProductsCount++;
                    Debug.Log("price : " + product.Price);
                    Debug.Log("localizedPrice : " + product.LocalizedPrice);
                    Debug.Log("currencySymbol : " + product.CurrencySymbol);
                    Debug.Log("currencyCode : " + product.CurrencyCode);
                    Debug.Log("IsAvailable : " + product.IsAvailable);
                    local_currency = product.LocalizedPrice;
                }
            }
            store_inited = true;

            RestoreDrill(false);

            Debug.Log("Product Count: " + ProductsCount + " / " + PaymentManager.Instance.IsInAppPurchasesEnabled);
        }
        else
        {
            Debug.Log("Init Failed: " + obj.Error.Code + " / " + obj.Error.Message);
        }
    }

    private static void Android_OnBillingConnected(BillingResult result)
    {
        AndroidInAppPurchaseManager.ActionBillingSetupFinished -= Android_OnBillingConnected;

        if (result.IsSuccess)
        {
            AndroidInAppPurchaseManager.ActionRetrieveProducsFinished += Android_OnRetrieveProductsFinised;

            //Store connection is Successful. Next we loading product and customer purchasing details
            AndroidInAppPurchaseManager.Client.RetrieveProducDetails();
        }

        Debug.Log("Android Inapp purchased = Response: " + result.Response.ToString() + " " + result.Message);
    }

    private static void Android_OnRetrieveProductsFinised(BillingResult result)
    {
        AndroidInAppPurchaseManager.ActionRetrieveProducsFinished -= Android_OnRetrieveProductsFinised;


        if (result.IsSuccess)
        {
            foreach (GoogleProductTemplate tpl in AndroidInAppPurchaseManager.Client.Inventory.Products)
            {
                //Debug.Log("price : " + tpl.Price);
                //Debug.Log("localizedPrice : " + tpl.LocalizedPrice);
                //Debug.Log("PriceCurrencyCode : " + tpl.PriceCurrencyCode);
                //Debug.Log("PriceAmountMicros : " + tpl.PriceAmountMicros );
                //Debug.Log("OriginalJson : " + tpl.OriginalJson);
                instance.local_currency = tpl.LocalizedPrice;
            }

            store_inited = true;
            instance.RestoreDrill(false);

            
        }
        else
        {
            AndroidMessage.Create("Connection Response", result.Response.ToString() + " " + result.Message);
        }

        //Debug.Log("Connection Response: " + result.Response.ToString() + " " + result.Message);

    }

    string local_currency;

	static int drill_index;

	public LoadingController loading_controller;

	public void PurchaseDrill(int index)
	{
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            PaymentManager.OnTransactionComplete -= IPhone_OnTransactionComplete;
            PaymentManager.OnTransactionComplete += IPhone_PaymentManager_OnTransactionComplete;
			PaymentManager.Instance.BuyProduct(iphone_purchase_table[index] + iphone_add_id);
        }
        else if(Application.platform == RuntimePlatform.Android)
        {
            AndroidInAppPurchaseManager.ActionProductPurchased += Android_OnProductPurchased;
            AndroidInAppPurchaseManager.Client.Purchase(iphone_purchase_table[index]);
        }

        drill_index = index;
        loading_controller.ShowLoading ();

        

    }

	public DrillShopList drill_shop_list;
	void IPhone_PaymentManager_OnTransactionComplete (PurchaseResult obj)
	{
		PaymentManager.OnTransactionComplete -= IPhone_PaymentManager_OnTransactionComplete;

		Debug.Log ("OnTranscation Complete : " + obj.ProductIdentifier);
		Debug.Log ("OnTransaction Complete : state : " + obj.State);


		loading_controller.HideLoading ();

		switch(obj.State) {
		case PurchaseState.Purchased:
		case PurchaseState.Restored:
                //Our product been successfully purchased or restored
                //So we need to provide content to our user 
                //depends on productIdentifier

                
            drill_shop_list.CompletePurchaseDrill ();
			Manager.Client.SendTransactionHit("purchase drill", iphone_purchase_table[drill_index] + iphone_add_id, "$", 0.99f, 0f, 0f);


            break;
		case PurchaseState.Deferred:
			//iOS 8 introduces Ask to Buy, which lets 
			//parents approve any purchases initiated by children
			//You should update your UI to reflect this 
			//deferred state, and expect another Transaction 
			//Complete  to be called again with a new transaction state 
			//reflecting the parent's decision or after the 
			//transaction times out. Avoid blocking your UI 
			//or gameplay while waiting for the transaction to be updated.
			break;
		case PurchaseState.Failed:
			//Our purchase flow is failed.
			//We can unlock interface and report user that the purchase is failed. 
			Debug.Log("Transaction failed with error, code: " + obj.Error.Code);
			Debug.Log("Transaction failed with error, description: " + obj.Error.Message);
			break;
		}

		if(obj.State == PurchaseState.Failed) {
			IOSNativePopUpManager.showMessage("Transaction Failed", "Error code: " + obj.Error.Code 
				+ "\n" + "Error description:" + obj.Error.Message);
		} 


	}

    private static void Android_OnProductPurchased(BillingResult result)
    {
        AndroidInAppPurchaseManager.ActionProductPurchased -= Android_OnProductPurchased;

        instance.loading_controller.HideLoading();

        if (result.IsSuccess)
        {
            instance.drill_shop_list.CompletePurchaseDrill();
            Manager.Client.SendTransactionHit("purchase drill", instance.iphone_purchase_table[drill_index], "$", 0.99f, 0f, 0f);
        }
        else
        {
            AndroidMessage.Create("Product Purchase Failed", result.Response.ToString() + " " + result.Message);
        }

        Debug.Log("Purchased Response: " + result.Response.ToString() + " " + result.Message);
    }

    public string GetLocalizedPrice()
	{
		return local_currency;
	}

	

	private bool complete_restored_message;
	public void RestoreDrill(bool complete_message)
	{
        if(store_inited == false)
        {
            return;
        }

		if (store_inited == true) 
		{
			GameObject systemObj = GameObject.FindGameObjectWithTag("System");
			GameDataSystem data_system = systemObj.GetComponent<GameDataSystem>();
			data_system.ResetPurchaseDrill ();
		}
        
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            complete_restored_message = complete_message;
            PaymentManager.OnTransactionComplete += IPhone_OnTransactionComplete;
            PaymentManager.OnRestoreComplete += IPhone_OnRestoreComplete;

            PaymentManager.Instance.RestorePurchases();
        }
        else if(Application.platform == RuntimePlatform.Android)
        {

            GameObject systemObj = GameObject.FindGameObjectWithTag("System");
            GameDataSystem data_system = systemObj.GetComponent<GameDataSystem>();

            for (int i = 0; i < iphone_purchase_table.Length; i++)
            {
                if (iphone_purchase_table[i].Contains("com.") == true)
                {
                    if (AndroidInAppPurchaseManager.Client.Inventory.IsProductPurchased(iphone_purchase_table[i]))
                    {
                        data_system.RestoreDrill(i);
                    }
                }
            }

            if (complete_message == true)
            {
                AndroidMessage.Create("Success", "Restore Compleated");
            }
        }
	}

    void IPhone_OnRestoreComplete(RestoreResult res)
    {
		if (res.IsSucceeded && complete_restored_message)
        {
            IOSNativePopUpManager.showMessage("Success", "Restore Compleated");
        }
        else
        {
            IOSNativePopUpManager.showMessage("Error: " + res.Error.Code, res.Error.Message);
        }
    }

    void IPhone_OnTransactionComplete(PurchaseResult result)
    {

        ISN_Logger.Log("OnTransactionComplete: " + result.ProductIdentifier);
        ISN_Logger.Log("OnTransactionComplete: state: " + result.State);


        switch (result.State)
        {
            case PurchaseState.Purchased:
                int index = 0;
                for (index = 0; index < iphone_purchase_table.Length; index++)
                {
                    if (result.ProductIdentifier.Equals(iphone_purchase_table[index] + iphone_add_id) == true)
                    {
                        break;
                    }
                }

                if (index < iphone_purchase_table.Length)
                {
                    GameObject systemObj = GameObject.FindGameObjectWithTag("System");
                    GameDataSystem data_system = systemObj.GetComponent<GameDataSystem>();
                    data_system.PurchaseDrill(index);
                }

                break;
            case PurchaseState.Restored:
                //Our product been succsesly purchased or restored
                //So we need to provide content to our user depends on productIdentifier

                index = 0;
                for(index = 0; index <  iphone_purchase_table.Length; index++)
                {
					if(result.ProductIdentifier.Equals(iphone_purchase_table[index] + iphone_add_id) == true)
                    {
                        break;
                    }
                }
                
                if(index < iphone_purchase_table.Length)
                {
                    GameObject systemObj = GameObject.FindGameObjectWithTag("System");
                    GameDataSystem data_system = systemObj.GetComponent<GameDataSystem>();
                    data_system.RestoreDrill(index);
                }

                break;
            case PurchaseState.Deferred:
                //iOS 8 introduces Ask to Buy, which lets parents approve any purchases initiated by children
                //You should update your UI to reflect this deferred state, and expect another Transaction Complete  to be called again with a new transaction state 
                //reflecting the parent’s decision or after the transaction times out. Avoid blocking your UI or gameplay while waiting for the transaction to be updated.
                break;
            case PurchaseState.Failed:
                //Our purchase flow is failed.
                //We can unlock intrefase and repor user that the purchase is failed. 
                ISN_Logger.Log("Transaction failed with error, code: " + result.Error.Code);
                ISN_Logger.Log("Transaction failed with error, description: " + result.Error.Message);


                break;
        }

        if (result.State == PurchaseState.Failed)
        {
            IOSNativePopUpManager.showMessage("Transaction Failed", "Error code: " + result.Error.Code + "\n" + "Error description:" + result.Error.Message);
        }
       
    }


    // Update is called once per frame
    void Update () {
		
	}
}
