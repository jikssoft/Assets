//
//  PubNativeNativeAdAdapter.h
//  AdSample
//
//  Created by 최치웅 on 2017. 7. 21..
//  Copyright © 2017년 LEE HEEDAE. All rights reserved.
//

#if __has_include(<MoPub/MoPub.h>)
    #import <MoPub/MoPub.h>
    #import <MoPub/MPStaticNativeAdImpressionTimer.h>
    #import <MoPub/MPAdDestinationDisplayAgent.h>
#else
    #import "MPNativeAdAdapter.h"
    #import "MPStaticNativeAdImpressionTimer.h"
    #import "MPAdDestinationDisplayAgent.h"
#endif

@interface PubNativeNativeAdAdapter : NSObject <MPNativeAdAdapter>

@property(nonatomic, weak) id<MPNativeAdAdapterDelegate> delegate;

@property (nonatomic, readonly) NSArray *impressionTrackerURLs;
@property (nonatomic, readonly) NSArray *clickTrackerURLs;

- (instancetype)initWithNativeAd:(NSDictionary *)nativeAd;

@end
