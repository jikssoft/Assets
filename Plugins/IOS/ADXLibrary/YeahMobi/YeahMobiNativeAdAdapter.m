//
//  YeahMobiNativeAdAdapter.m
//  Pods
//
//  Created by 최치웅 on 2017. 6. 19..
//
//

#import <CTSDK/CTSDK.h>

#import "YeahMobiNativeAdAdapter.h"
#import "MPNativeAdConstants.h"

@implementation YeahMobiNativeAdAdapter

- (instancetype)initWithNativeAd:(CTNativeAdModel *)nativeAd {
    if (self = [super init]) {
        NSMutableDictionary *properties = [NSMutableDictionary dictionary];
        
        if(nativeAd != nil) {
            [properties setObject:nativeAd.title forKey:kAdTitleKey];
            [properties setObject:nativeAd.desc forKey:kAdTextKey];
            [properties setObject:nativeAd.button forKey:kAdCTATextKey];
            [properties setObject:nativeAd.image forKey:kAdMainImageKey];
            [properties setObject:nativeAd.icon forKey:kAdIconImageKey];
        }
        
        self.nativeAd = nativeAd;
        self.properties = properties;
        
        self.impressionTimer = [[MPStaticNativeAdImpressionTimer alloc] initWithRequiredSecondsForImpression:1.0 requiredViewVisibilityPercentage:0.5];
        self.impressionTimer.delegate = self;
    }
    return self;
}

- (NSURL *)defaultActionURL {
    return nil;
}

- (void)displayContentForURL:(NSURL *)URL rootViewController:(UIViewController *)controller {
    [self.nativeAd clickAdJumpToMarker];
}

#pragma mark - Impression tracking
- (void)willAttachToView:(UIView *)view {
    [self.impressionTimer startTrackingView:view];
}

- (void)trackImpression {
    [self.delegate nativeAdWillLogImpression:self];
    
    [self.nativeAd impressionForAd];
}

@end
