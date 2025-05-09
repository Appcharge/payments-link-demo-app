Once you’ve added Appcharge payment links to your iOS game, the next step is to connect those payments to your in-game logic. This guide walks you through using the PurchaseManager in Unity to launch checkout flows, handle deeplink callbacks, and respond to payment outcomes directly within your game.

For details on setting up the payment links themselves, see [Accept Payments in iOS Games with Appcharge Payment Links](https://developers.appcharge.com/docs/accept-payments-in-ios-games-with-appcharge-payment-links).

## Overview

The `PurchaseManager` is a Unity MonoBehaviour singleton that acts as the bridge between your game and Appcharge’s payment system that handles:

- Launching the external payment flow
- Managing loading states through a FlowManager
- Capturing the result of the payment via deep links
- Reacting to success, failure, or cancellation statuses
- This app uses a custom networking class to simulate server to server communication

## Requirements

Before you begin, make sure your project meets the following:

- Git LFS
- XCode 13 or higher
- Supports deep linking
- Your app's Associated Domains / Intent Filters are configured properly for handling payment callbacks.

## Before you start
Make sure to install Git LFS and pull the game assets:
```
git lfs install
git lfs pull
```

## Step-by-step integration

1. Create a new GameObject in your initial scene and attach the `PurchaseManager` script to it. This ensures it’s active when the game starts.
2. In the Unity Inspector, assign your `FlowManager` component. This handles the display and hiding of loading indicators during the checkout process.
   ```csharp
   [SerializeField] private FlowManager _flowManager;
   ```
3. Handle singleton setup in Awake() and ensures it persists across scenes:
   ```csharp
   private void Awake()
   {
       if (Instance == null)
       {
           Instance = this;
           Application.deepLinkActivated += onDeepLinkActivated;
           DontDestroyOnLoad(gameObject);
       }
       else
       {
           Destroy(gameObject);
       }
   }
   ```

## Triggering a purchase

When the player initiates a purchase, call the `OpenCheckout()` method:

```csharp
PurchaseManager.Instance.OpenCheckout();
```

This method will:

- Show a loading spinner `_flowManager.ShowLoader(true)`
- Initiate a session request to the Appcharge backend
- Launch an external browser for payment via `Application.OpenURL`

## Handling deeplink callbacks

After the payment flow completes, Appcharge redirects the user back into your app with a URL like:

```csharp
https://purchase.domain.com?status=success
```

The `onDeepLinkActivated` method will automatically handle the incoming URL and log the corresponding purchase status using `PurchaseStatusUtils`.

```csharp
private void onDeepLinkActivated(string deeplinkURL) {
    Debug.Log("Deeplink URL: " + deeplinkURL);

    PurchaseStatus status = PurchaseStatusUtils.ParseStatusFromDeepLink(deeplinkURL);

    switch (status)
    {
        case PurchaseStatus.Success:
            Debug.Log("Purchase successful");
            break;
        case PurchaseStatus.Fail:
            Debug.Log("Purchase failed");
            break;
        case PurchaseStatus.Cancel:
            Debug.Log("Purchase canceled");
            break;
        case PurchaseStatus.Close:
            Debug.Log("Purchase closed");
            break;
        default:
            Debug.Log("Unknown purchase status");
            break;
    }
}
```

You can expand this switch block to show UI messages, grant items, or retry payments based on the status.

## Customization

You may expand the `switch` in `onDeepLinkActivated()` to handle UI updates or trigger in-game rewards based on the result.

## Sample Flow

1. Player taps "Buy" in the in-game's store.  
2. `PurchaseManager.OpenCheckout()` is called.  
3. A session is created and a browser is launched.  
4. Player completes or exits the payment.  
5. Deep link is triggered, and the game logs the purchase status.

## Full example code

```csharp
using Appcharge.Networking;  
using Appcharge.Utils;  
using UnityEngine;

namespace Appcharge  
{  
  public class PurchaseManager : MonoBehaviour  
  {  
    public static PurchaseManager Instance { get; private set; }   
      
    [SerializeField]private FlowManager _flowManager;

    // Use Awake life cycle to declare your deeplink function
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Application.deepLinkActivated += onDeepLinkActivated;                
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // When application shows via deeplink, this method will be triggered
    private void onDeepLinkActivated(string deeplinkURL) {
        Debug.Log("Deeplink URL: " + deeplinkURL);

        // Deeplink url will contain a status as follow:
        // https://your-redirect-url.com?status=purchase_status
        PurchaseStatus status = PurchaseStatusUtils.ParseStatusFromDeepLink(deeplinkURL);

        // Based on the status, call a relevant in-game function
        switch (status)
        {
            case PurchaseStatus.Success:
                Debug.Log("Purchase successful");
                break;
            case PurchaseStatus.Fail:
                Debug.Log("Purchase failed");
                break;
            case PurchaseStatus.Cancel:
                Debug.Log("Purchase canceled");
                break;
            case PurchaseStatus.Close:
                Debug.Log("Purchase closed");
                break;
            default:
                Debug.Log("Unknown purchase status");
                break;
        }
    }

    // The following method will open the checkout.
    // - Checkout will be opened in the default available browser
    // - The HttpRequest class is a custom class, 
    //   represents a server to sever communication example
    public void OpenCheckout() {
        _flowManager.ShowLoader(true);

        StartCoroutine(
            HttpRequest.CreateSession(
                url => {
                    Application.OpenURL(url);
                }, 
                error => {
                    Debug.Log("Failed to create session: " + error);
                }
            )
        );
    }
}
```



## License

Proprietary – © Appcharge. All rights reserved.