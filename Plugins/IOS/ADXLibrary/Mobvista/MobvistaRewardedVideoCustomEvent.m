//
//  MobvistaRewardVideoCustomEvent.m
//  MoPubSampleApp
//
//  Created by CharkZhang on 2017/3/21.
//  Copyright © 2017年 MoPub. All rights reserved.
//

#import "MobvistaRewardedVideoCustomEvent.h"

#import <MVSDK/MVSDK.h>
#import <MVSDKReward/MVRewardAdManager.h>

#import "MobvistaAdapterHelper.h"


#import "MoPub.h"
#import "MPLogging.h"
#import "MPRewardedVideoReward.h"

@interface MobvistaRewardedVideoCustomEvent () <MVRewardAdLoadDelegate,MVRewardAdShowDelegate>

@property (nonatomic, copy) NSString *adUnit;
@property (nonatomic, copy) NSString *rewardId;

@end

@implementation MobvistaRewardedVideoCustomEvent


- (void)requestRewardedVideoWithCustomEventInfo:(NSDictionary *)info
{
    // The default implementation of this method does nothing. Subclasses must override this method
    // and implement code to load a rewarded video here.
    NSString *appId = [info objectForKey:@"app_id"];
    NSString *appKey = [info objectForKey:@"app_key"];
    NSString *unitId = [info objectForKey:@"adunit_id"];
    
    NSString *errorMsg = nil;
    if (!appId) errorMsg = @"Invalid Mobvista appId";
    if (!appKey) errorMsg = @"Invalid Mobvista appKey";
    if (!unitId) errorMsg = @"Invalid Mobvista unitId";
    
    if (errorMsg) {
        NSError *error = [NSError errorWithDomain:kMobVistaErrorDomain code:MPRewardedVideoAdErrorInvalidAdUnitID userInfo:@{NSLocalizedDescriptionKey : errorMsg}];
        [self.delegate rewardedVideoDidFailToLoadAdForCustomEvent:self error:error];
        return;
    }

    if (![MobvistaAdapterHelper isSDKInitialized]) {

        [[MVSDK sharedInstance] setAppID:appId ApiKey:appKey];
        
        [MobvistaAdapterHelper sdkInitialized];
    }
    

    self.adUnit = unitId;
    self.rewardId = [info objectForKey:@"rewardId"];

    [[MVRewardAdManager sharedInstance] loadVideo:self.adUnit delegate:self];

    [self.delegate rewardedVideoDidLoadAdForCustomEvent:self];

}

- (BOOL)hasAdAvailable
{
    // Subclasses must override this method and implement coheck whether or not a rewarded vidoe ad
    // is available for presentation.

    return [[MVRewardAdManager sharedInstance] isVideoReadyToPlay:self.adUnit];
}

- (void)presentRewardedVideoFromViewController:(UIViewController *)viewController
{
    // The default implementation of this method does nothing. Subclasses must override this method
    // and implement code to display a rewarded video here.

    if ([self hasAdAvailable]) {

        NSString *customerId = [self.delegate customerIdForRewardedVideoCustomEvent:self];

        if ([[MVRewardAdManager sharedInstance] respondsToSelector:@selector(showVideo:withRewardId:userId:delegate:viewController:)]) {
            [[MVRewardAdManager sharedInstance] showVideo:self.adUnit withRewardId:self.rewardId userId:customerId delegate:self viewController:viewController];
        }

    } else {
        
        NSError *error = [NSError errorWithDomain:MoPubRewardedVideoAdsSDKDomain code:MPRewardedVideoAdErrorNoAdsAvailable userInfo:nil];
        [self.delegate rewardedVideoDidFailToPlayForCustomEvent:self error:error];
    }
}

- (BOOL)enableAutomaticImpressionAndClickTracking
{
    // Subclasses may override this method to return NO to perform impression and click tracking
    // manually.
    return NO;
}

- (void)handleAdPlayedForCustomEventNetwork
{
    // The default implementation of this method does nothing. Subclasses must override this method
    // and implement code to handle when another ad unit plays an ad for the same ad network this custom event is representing.
    
    
    // If we no longer have an ad available, report back up to the application that this ad expired.
    // We receive this message only when this custom event has reported its ad has loaded and another ad unit
    // has played a video for the same ad network.
    if (![self hasAdAvailable]) {
        [self.delegate rewardedVideoDidExpireForCustomEvent:self];
    }
}

- (void)handleCustomEventInvalidated
{
    // The default implementation of this method does nothing. Subclasses must override this method
    // and implement code to handle when the custom event is no longer needed by the rewarded video system.
    
    // no-op
}










#pragma mark GADRewardBasedVideoAdDelegate

/**
 *  Called when the ad is successfully load , and is ready to be displayed
 *
 *  @param unitId - the unitId string of the Ad that was loaded.
 */
- (void)onVideoAdLoadSuccess:(nullable NSString *)unitId{
    
    [self.delegate rewardedVideoDidLoadAdForCustomEvent:self];
}

/**
 *  Called when there was an error loading the ad.
 *
 *  @param unitId      - the unitId string of the Ad that failed to load.
 *  @param error       - error object that describes the exact error encountered when loading the ad.
 */
- (void)onVideoAdLoadFailed:(nullable NSString *)unitId error:(nonnull NSError *)error{
    
    [self.delegate rewardedVideoDidFailToLoadAdForCustomEvent:self error:error];
}

/**
 *  This protocol defines a listener for ad video show events.
 */
//MVRewardAdShowDelegate


/**
 *  Called when the ad display success
 *
 *  @param unitId - the unitId string of the Ad that display success.
 */
- (void)onVideoAdShowSuccess:(nullable NSString *)unitId{
    
    [self.delegate rewardedVideoWillAppearForCustomEvent:self];
    [self.delegate rewardedVideoDidAppearForCustomEvent:self];

    
    if ([self.delegate respondsToSelector:@selector(trackImpression)]) {
        [self.delegate trackImpression];
    } else {
        MPLogWarn(@"Delegate does not implement impression tracking callback. Impressions likely not being tracked.");
    }
    
}

/**
 *  Called when the ad failed to display for some reason
 *
 *  @param unitId      - the unitId string of the Ad that failed to be displayed.
 *  @param error       - error object that describes the exact error encountered when showing the ad.
 */
- (void)onVideoAdShowFailed:(nullable NSString *)unitId withError:(nonnull NSError *)error{
    
    [self.delegate rewardedVideoDidFailToPlayForCustomEvent:self error:error];
}

/**
 *  Called when the ad is clicked
 *
 *  @param unitId - the unitId string of the Ad clicked.
 */
- (void)onVideoAdClicked:(nullable NSString *)unitId{
    
    [self.delegate rewardedVideoDidReceiveTapEventForCustomEvent:self];

    if ([self.delegate respondsToSelector:@selector(trackClick)]) {
        [self.delegate trackClick];
    } else {
        MPLogWarn(@"Delegate does not implement click tracking callback. Clicks likely not being tracked.");
    }
}

/**
 *  Called when the ad has been dismissed from being displayed, and control will return to your app
 *
 *  @param unitId      - the unitId string of the Ad that has been dismissed
 *  @param converted   - BOOL describing whether the ad has converted
 *  @param rewardInfo  - the rewardInfo object containing an array of reward objects that should be given to your user.
 */
- (void)onVideoAdDismissed:(nullable NSString *)unitId withConverted:(BOOL)converted withRewardInfo:(nullable MVRewardAdInfo *)rewardInfo{

    [self.delegate rewardedVideoWillDisappearForCustomEvent:self];
    [self.delegate rewardedVideoDidDisappearForCustomEvent:self];

    if (!converted || !rewardInfo) {
        return;
    }

    MPRewardedVideoReward *reward = [[MPRewardedVideoReward alloc] initWithCurrencyType:rewardInfo.rewardName amount:[NSNumber numberWithInteger:rewardInfo.rewardAmount]];
    [self.delegate rewardedVideoShouldRewardUserForCustomEvent:self reward:reward];


}



@end

