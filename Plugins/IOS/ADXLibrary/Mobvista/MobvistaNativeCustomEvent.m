//
//  MobvistaNativeCustomEvent.m
//  MoPubSampleApp
//
//  Created by tianye on 2016/11/10.
//  Copyright © 2016年 MoPub. All rights reserved.
//

#import "MobvistaNativeCustomEvent.h"
#import "MobvistaNativeAdAdapter.h"
#import "MPNativeAd.h"
#import "MPNativeAdError.h"

#import "MobvistaAdapterHelper.h"

#import <MVSDK/MVSDK.h>

static BOOL mVideoEnabled = NO;

@interface MobvistaNativeCustomEvent()<MVNativeAdManagerDelegate>

@property (nonatomic, readwrite, strong) MVNativeAdManager *mvNativeAdManager;
@property (nonatomic, readwrite, copy) NSString *unitId;

@property (nonatomic) BOOL videoEnabled;

@end


@implementation MobvistaNativeCustomEvent


- (void)requestAdWithCustomEventInfo:(NSDictionary *)info
{
    NSString *appId = [info objectForKey:@"app_id"];
    NSString *appKey = [info objectForKey:@"app_key"];
    NSString *unitId = [info objectForKey:@"adunit_id"];
    NSString *placementID = [info objectForKey:@"placementId"];

    NSString *errorMsg = nil;
    if (!appId) errorMsg = @"Invalid Mobvista appId";
    if (!appKey) errorMsg = @"Invalid Mobvista appKey";
    if (!unitId) errorMsg = @"Invalid Mobvista unitId";

    if (errorMsg) {
        [self.delegate nativeCustomEvent:self didFailToLoadAdWithError:MPNativeAdNSErrorForInvalidAdServerResponse(errorMsg)];
        return;
    }

    if ([info objectForKey:kMVVideoAdsEnabledKey] == nil) {
        self.videoEnabled = mVideoEnabled;
    } else {
        self.videoEnabled = [[info objectForKey:kMVVideoAdsEnabledKey] boolValue];
    }

    MVAdTemplateType reqNum = [info objectForKey:@"reqNum"] ?[[info objectForKey:@"reqNum"] integerValue]:1;
    
    self.unitId = unitId;

    if (![MobvistaAdapterHelper isSDKInitialized]) {
        
        [[MVSDK sharedInstance] setAppID:appId ApiKey:appKey];
        [MobvistaAdapterHelper sdkInitialized];
    }
    
    _mvNativeAdManager = [[MVNativeAdManager alloc] initWithUnitID:unitId fbPlacementId:placementID videoSupport:self.videoEnabled forNumAdsRequested:reqNum presentingViewController:nil];

    _mvNativeAdManager.delegate = self;
    [_mvNativeAdManager loadAds];
    
}

#pragma mark - nativeAdManager init and delegate

- (void)nativeAdsLoaded:(nullable NSArray *)nativeAds {
    MobvistaNativeAdAdapter *adAdapter = [[MobvistaNativeAdAdapter alloc] initWithNativeAds:nativeAds nativeAdManager:_mvNativeAdManager withUnitId:self.unitId videoSupport:self.videoEnabled];
    MPNativeAd *interfaceAd = [[MPNativeAd alloc] initWithAdAdapter:adAdapter];
    [self.delegate nativeCustomEvent:self didLoadAd:interfaceAd];
}

- (void)nativeAdsFailedToLoadWithError:(nonnull NSError *)error {
    [self.delegate nativeCustomEvent:self didFailToLoadAdWithError:error];
}



- (void)nativeAdDidClick:(nonnull MVCampaign *)nativeAd;
{

}

- (void)nativeAdClickUrlDidEndJump:(nullable NSURL *)finalUrl
                             error:(nullable NSError *)error{
    
}

@end
