//
//  YeahMobiNativeCustomEvent.m
//  Pods
//
//  Created by 최치웅 on 2017. 6. 19..
//
//

#import <CTSDK/CTSDK.h>

#import "YeahMobiNativeCustomEvent.h"
#import "YeahMobiNativeAdAdapter.h"

#import "MPInstanceProvider.h"
#import "MPLogging.h"

#import "MPNativeAd.h"

@interface YeahMobiNativeCustomEvent() <CTNativeAdDelegate>
@end

@implementation YeahMobiNativeCustomEvent

- (void)requestAdWithCustomEventInfo:(NSDictionary *)info {
    if (![info objectForKey:@"adunit_id"]) {
        MPLogError(@"AdUnit ID is required for YeahMobi native ad");
        [self.delegate nativeCustomEvent:self didFailToLoadAdWithError:nil];
        return;
    }
    
    NSString *unitId = [NSString stringWithFormat:@"%@",[info objectForKey:@"adunit_id"]];//nonnull
    
    [[CTService shareManager] loadRequestGetCTSDKConfigBySlot_id:unitId];
    [[CTService shareManager] getNativeADswithSlotId:unitId delegate:self imageWidthHightRate:CTImageWHRateOnePointNineToOne isTest:NO success:^(CTNativeAdModel *nativeModel) {
        dispatch_async(dispatch_get_main_queue(), ^{
            YeahMobiNativeAdAdapter *adAdapter = [[YeahMobiNativeAdAdapter alloc] initWithNativeAd:nativeModel];
            MPNativeAd *interfaceAd = [[MPNativeAd alloc] initWithAdAdapter:adAdapter];
            [self.delegate nativeCustomEvent:self didLoadAd:interfaceAd];
        });
    } failure:^(NSError *error) {
        dispatch_async(dispatch_get_main_queue(), ^{
            [self.delegate nativeCustomEvent:self didFailToLoadAdWithError:error];
        });
    }];
}
@end
