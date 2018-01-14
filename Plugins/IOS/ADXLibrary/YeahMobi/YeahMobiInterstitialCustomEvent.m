//
//  YeahMobiInterstitialCustomEvent.m
//  Pods
//
//  Created by 최치웅 on 2017. 6. 19..
//
//

#import <CTSDK/CTSDK.h>

#import "YeahMobiInterstitialCustomEvent.h"

#import "MPInstanceProvider.h"
#import "MPLogging.h"

@interface YeahMobiInterstitialCustomEvent() <CTInterstitialDelegate>
@end

@implementation YeahMobiInterstitialCustomEvent

- (void)requestInterstitialWithCustomEventInfo:(NSDictionary *)info
{
    if (![info objectForKey:@"adunit_id"]) {
        MPLogError(@"AdUnit ID is required for YeahMobi interstitial ad");
        [self.delegate interstitialCustomEvent:self didFailToLoadAdWithError:nil];
        return;
    }
    
    NSString *unitId = [NSString stringWithFormat:@"%@",[info objectForKey:@"adunit_id"]];//nonnull
    
    [[CTService shareManager] loadRequestGetCTSDKConfigBySlot_id:unitId];
    
    [[CTService shareManager] preloadInterstitialWithSlotId:unitId delegate:self isFullScreen:YES isTest:NO success:^(UIView *InterstitialView) {
        dispatch_async(dispatch_get_main_queue(), ^{
            [self.delegate interstitialCustomEvent:self didLoadAd:InterstitialView];
        });
    } failure:^(NSError *error) {
        dispatch_async(dispatch_get_main_queue(), ^{
            [self.delegate interstitialCustomEvent:self didFailToLoadAdWithError:error];
        });
    }];
}

- (void)showInterstitialFromRootViewController:(UIViewController *)rootViewController{
    [[CTService shareManager] interstitialShowWithControllerStyleFromRootViewController:rootViewController];
}

-(void)CTInterstitialDidClick:(CTInterstitial*)interstitialAD {
    [self.delegate interstitialCustomEventDidReceiveTapEvent:self];
}

-(void)CTInterstitialClosed:(CTInterstitial*)interstitialAD {
    [self.delegate interstitialCustomEventDidDisappear:self];
}

-(void)CTInterstitialWillLeaveApplication:(CTInterstitial*)interstitialAD {
    [self.delegate interstitialCustomEventWillLeaveApplication:self];
}

@end
