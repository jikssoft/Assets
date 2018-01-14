//
//  YeahMobiNativeAdAdapter.h
//  Pods
//
//  Created by 최치웅 on 2017. 6. 19..
//
//

#if __has_include(<MoPub/MoPub.h>)
    #import <MoPub/MoPub.h>
    #import <MoPub/MPStaticNativeAdImpressionTimer.h>
#else
    #import "MPNativeAdAdapter.h"
    #import "MPStaticNativeAdImpressionTimer.h"
#endif

@class CTNativeAdModel;

@interface YeahMobiNativeAdAdapter : NSObject <MPNativeAdAdapter, MPStaticNativeAdImpressionTimerDelegate>

@property(nonatomic, weak) id<MPNativeAdAdapterDelegate> delegate;

@property (nonatomic, strong) CTNativeAdModel * nativeAd;
@property (nonatomic, strong) NSDictionary * properties;
@property (nonatomic, strong) MPStaticNativeAdImpressionTimer *impressionTimer;

- (instancetype)initWithNativeAd:(CTNativeAdModel *)nativeAd;

@end
