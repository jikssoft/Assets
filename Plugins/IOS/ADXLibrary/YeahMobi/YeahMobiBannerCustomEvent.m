//
//  YeahMobiBannerCustomEvent.m
//  Pods
//
//  Created by 최치웅 on 2017. 6. 19..
//
//

#import <CTSDK/CTSDK.h>

#import "YeahMobiBannerCustomEvent.h"

#import "MPInstanceProvider.h"
#import "MPLogging.h"

@interface YeahMobiBannerCustomEvent() <CTBannerDelegate>
@end

@implementation YeahMobiBannerCustomEvent

- (void)requestAdWithSize:(CGSize)size customEventInfo:(NSDictionary *)info
{
    if (![info objectForKey:@"adunit_id"]) {
        MPLogError(@"AdUnit ID is required for YeahMobi banner ad");
        [self.delegate bannerCustomEvent:self didFailToLoadAdWithError:nil];
        return;
    }
    
    NSString *unitId = [NSString stringWithFormat:@"%@",[info objectForKey:@"adunit_id"]];//nonnull
    
    [[CTService shareManager] loadRequestGetCTSDKConfigBySlot_id:unitId];
    
    [[CTService shareManager] getBannerADswithSlotId:unitId delegate:self frame:CGRectMake(0, 0, size.width, size.height) needCloseButton:NO isTest:NO success:^(UIView *bannerView) {
        dispatch_async(dispatch_get_main_queue(), ^{
            [self.delegate bannerCustomEvent:self didLoadAd:bannerView];
        });
    } failure:^(NSError *error) {
        dispatch_async(dispatch_get_main_queue(), ^{
            [self.delegate bannerCustomEvent:self didFailToLoadAdWithError:error];
        });
    }];;
}

-(void)CTBannerDidLeaveLandingPage:(CTBanner*)banner {
    [self.delegate bannerCustomEventDidFinishAction:self];
}

-(void)CTBannerWillLeaveApplication:(CTBanner*)banner {
    [self.delegate bannerCustomEventWillLeaveApplication:self];
}

@end
