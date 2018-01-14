//
//  MobvistaInterstitialCustomEvent.m
//  MoPubSampleApp
//
//  Created by CharkZhang on 2017/6/8.
//  Copyright © 2017年 MoPub. All rights reserved.
//

#import "MobvistaInterstitialCustomEvent.h"
#import "MobvistaAdapterHelper.h"

#import <MVSDK/MVSDK.h>
#import <MVSDKInterstitial/MVInterstitialAdManager.h>

@interface MobvistaInterstitialCustomEvent()<MVInterstitialAdLoadDelegate,MVInterstitialAdShowDelegate>

@property (nonatomic, copy) NSString *adUnit;

@property (nonatomic, readwrite, strong) MVInterstitialAdManager *mvInterstitialAdManager;

@end

@implementation MobvistaInterstitialCustomEvent


- (void)requestInterstitialWithCustomEventInfo:(NSDictionary *)info
{
    // The default implementation of this method does nothing. Subclasses must override this method
    // and implement code to load an interstitial here.
    NSString *appId = [info objectForKey:@"app_id"];
    NSString *appKey = [info objectForKey:@"app_key"];
    NSString *unitId = [info objectForKey:@"adunit_id"];
    
    NSString *errorMsg = nil;
    if (!appId) errorMsg = @"Invalid Mobvista appId";
    if (!appKey) errorMsg = @"Invalid Mobvista appKey";
    if (!unitId) errorMsg = @"Invalid Mobvista unitId";
    
    if (errorMsg) {
        NSError *error = [NSError errorWithDomain:kMobVistaErrorDomain code:-1500 userInfo:@{NSLocalizedDescriptionKey : errorMsg}];
        [self.delegate interstitialCustomEvent:self didFailToLoadAdWithError:error];
        return;
    }

    if (![MobvistaAdapterHelper isSDKInitialized]) {

        [[MVSDK sharedInstance] setAppID:appId ApiKey:appKey];
        
        [MobvistaAdapterHelper sdkInitialized];
    }
    
    self.adUnit = unitId;


    MVInterstitialAdCategory adCategory = MVInterstitial_AD_CATEGORY_ALL;
    if ([info objectForKey:@"adCategory"]) {
        NSString *category = [NSString stringWithFormat:@"%@",[info objectForKey:@"adCategory"]];
        adCategory = (MVInterstitialAdCategory)[category integerValue];
    }
    
    if (!_mvInterstitialAdManager) {
        _mvInterstitialAdManager = [[MVInterstitialAdManager alloc] initWithUnitID:self.adUnit adCategory:adCategory];
    }
    
    [_mvInterstitialAdManager loadWithDelegate:self];

}

- (BOOL)enableAutomaticImpressionAndClickTracking
{
    // Subclasses may override this method to return NO to perform impression and click tracking
    // manually.
    return NO;
}

- (void)showInterstitialFromRootViewController:(UIViewController *)rootViewController
{
    // The default implementation of this method does nothing. Subclasses must override this method
    // and implement code to display an interstitial here.
    [_mvInterstitialAdManager showWithDelegate:self presentingViewController:rootViewController];
}




#pragma mark - MVInterstitialAdManagerDelegate

/**
 *  This protocol defines a listener for ad Interstitial load events.
 */
//@protocol MVInterstitialAdLoadDelegate <NSObject>

- (void) onInterstitialLoadSuccess{
 
    if (self.delegate && [self.delegate respondsToSelector:@selector(interstitialCustomEvent: didLoadAd:)]) {
        [self.delegate interstitialCustomEvent:self didLoadAd:nil];
    }
}

- (void) onInterstitialLoadFail:(nonnull NSError *)error{

    if (self.delegate && [self.delegate respondsToSelector:@selector(interstitialCustomEvent: didFailToLoadAdWithError:)]) {
        [self.delegate interstitialCustomEvent:self didFailToLoadAdWithError:error];
    }
}


/**
 *  This protocol defines a listener for ad Interstitial show events.
 */
//@protocol MVInterstitialAdShowDelegate <NSObject>

- (void) onInterstitialShowSuccess{
    
    if (self.delegate && [self.delegate respondsToSelector:@selector(interstitialCustomEventWillAppear:)]) {
        [self.delegate interstitialCustomEventWillAppear:self ];
    }
    if (self.delegate && [self.delegate respondsToSelector:@selector(interstitialCustomEventDidAppear:)]) {
        [self.delegate interstitialCustomEventDidAppear:self ];
    }
    
    if (self.delegate && [self.delegate respondsToSelector:@selector(trackImpression)]) {
        [self.delegate trackImpression];
    }
}

- (void) onInterstitialShowFail:(nonnull NSError *)error{
    
}


- (void) onInterstitialClosed{
    
    if (self.delegate && [self.delegate respondsToSelector:@selector(interstitialCustomEventWillDisappear:)]) {
        [self.delegate interstitialCustomEventWillDisappear:self ];
    }
    if (self.delegate && [self.delegate respondsToSelector:@selector(interstitialCustomEventDidDisappear:)]) {
        [self.delegate interstitialCustomEventDidDisappear:self ];
    }
}


- (void) onInterstitialAdClick{
    
    if (self.delegate && [self.delegate respondsToSelector:@selector(interstitialCustomEventDidReceiveTapEvent:)]) {
        [self.delegate interstitialCustomEventDidReceiveTapEvent:self ];
    }
    
    if (self.delegate && [self.delegate respondsToSelector:@selector(trackClick)]) {
        [self.delegate trackClick];
    }
}


@end
