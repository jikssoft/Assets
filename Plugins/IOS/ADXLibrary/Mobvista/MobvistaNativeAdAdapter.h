//
//  MobvistaNativeAdAdapter.h
//  MoPubSampleApp
//
//  Created by tianye on 2016/11/10.
//  Copyright © 2016年 MoPub. All rights reserved.
//

#import <Foundation/Foundation.h>
#if __has_include(<MoPub/MoPub.h>)
#import <MoPub/MoPub.h>
#else
#import "MPNativeAdAdapter.h"
#endif

@class  MVNativeAdManager;

extern NSString *const kMVVideoAdsEnabledKey;

@interface MobvistaNativeAdAdapter : NSObject <MPNativeAdAdapter>
@property (nonatomic, weak) id<MPNativeAdAdapterDelegate> delegate;
@property (nonatomic, readonly) NSArray *nativeAds;

- (instancetype)initWithNativeAds:(NSArray *)nativeAds nativeAdManager:(MVNativeAdManager *)nativeAdManager withUnitId:(NSString *)unitId videoSupport:(BOOL)videoSupport;

@end
